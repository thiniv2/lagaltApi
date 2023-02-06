using AutoMapper;
using Lagalt.Models;
using Lagalt.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lagalt.Repositories
{
    public class ProjectRepo
	{
		private readonly LagaltContext dbContext;
		private readonly IMapper mapper;
		public ProjectRepo(LagaltContext db, IMapper mapper)
		{
			dbContext = db;
			this.mapper = mapper;
		}

		public async Task<List<ProjectDTO>> GetAllProjects()
		{
			List<Project> projects = await dbContext.Projects
				.Include(p => p.Users)
				.Include(p => p.Applicants)
				
				.ToListAsync();
			if (projects.Count == 0) return null;

			return projects.Select(p => mapper.Map<ProjectDTO>(p)).ToList();
		}

		public async Task<List<ProjectBannerDTO>> GetAllProjectBanners()
		{
			List<Project> projects = await dbContext.Projects
				.ToListAsync();

			if (projects.Count == 0) return null;

			foreach (var project in projects)
				project.TotalSkills = project.Skillset.Count();

			return projects.Select(p => mapper.Map<ProjectBannerDTO>(p)).ToList();
		}

		public async Task<ProjectDTO> GetProjectById(int id)
		{
            // Throws Error if project by that id is not found
			Project project = await dbContext.Projects.Where(p => p.Id == id).Include(p => p.Users).FirstAsync();

			if (project == null) return null;
			List<Applicant> applicants = new();
			applicants = await dbContext.Applicants.Where(a => a.ProjectID == id).ToListAsync();
			var dto = mapper.Map<ProjectDTO>(project);
			dto.Applicants = applicants;
			Console.WriteLine(dto.Applicants.FirstOrDefault());
			return dto;
		}

		public async Task<ProjectBannerDTO> GetProjectBannerById(int id)
		{
			Project project = await dbContext.Projects.FindAsync(id);
			if (project == null || project.Skillset == null) return null;

			project.TotalSkills = project.Skillset.Count();
			return mapper.Map<ProjectBannerDTO>(project);
		}

		public async Task<ProjectDetailsDTO> GetProjectDetailsById(int id)
		{
			Project project = await dbContext.Projects.FindAsync(id);
			if (project == null) return null;

			return mapper.Map<ProjectDetailsDTO>(project);
		}

		public bool ProjectExists(int id)
		{
			return dbContext.Projects.Any(p => p.Id == id);
		}
		public async Task<Project> AddProject(ProjectCreateDTO projectCreateDto)
		{
			var project = mapper.Map<Project>(projectCreateDto);
			dbContext.Projects.Add(project);
			await dbContext.SaveChangesAsync();
			return project;
		}

		public async Task<List<ProjectDTO>> RemoveProject(int id)
		{
			Project project = await dbContext.Projects.FindAsync(id);
			if (project == null) return null;
			dbContext.Projects.Remove(project);
			await dbContext.SaveChangesAsync();
			return await GetAllProjects();
		}

		public async Task<Project> EditProject(ProjectDTO projectDto)
		{
			Project project = await dbContext.Projects.FindAsync(projectDto.Id);
			if (project == null) return null;
			dbContext.Entry(project).State = EntityState.Detached;

			project = mapper.Map<Project>(projectDto);
			dbContext.Projects.Update(project);
			await dbContext.SaveChangesAsync();
			return project;
		}

		public async Task<List<ProjectDTO>> UpdateProjectUsers(int projectId, List<string> users)
		{
			Project projectToUpdate = await dbContext.Projects
				.Include(p => p.Users).Where(p => p.Id == projectId).FirstAsync();
			List<User> usersToUpdate = new();
			foreach (var userId in users)
			{
				User user = await dbContext.Users.Where(u => u.Id == userId).FirstAsync();
				if (user == null)
					// Record doesnt exist: https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/ms229021(v=vs.100)?redirectedfrom=MSDN
					throw new KeyNotFoundException();
				usersToUpdate.Add(user);
			}
			projectToUpdate.Users = usersToUpdate;
			await dbContext.SaveChangesAsync();
			return await GetAllProjects();
		}
		// Could be used if list of applicants needs to be added.
		public async Task UpdateProjectApplicants(int projectId, List<string> applicants)
        {
			Project projectToUpdate = await dbContext.Projects.Include(p => p.Applicants).Where(p => p.Id == projectId).FirstAsync();
			List<Applicant> applicantsToUpdate = new();
			foreach (var applicantId in applicants)
			{
				Applicant applicant = await dbContext.Applicants.FindAsync(applicantId);
				if (applicant == null)
					// Record doesnt exist: https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/ms229021(v=vs.100)?redirectedfrom=MSDN
					throw new KeyNotFoundException();
				applicantsToUpdate.Add(applicant);
			}
			projectToUpdate.Applicants = applicantsToUpdate;
			await dbContext.SaveChangesAsync();
		}
		//Used in project controller addprojectapplicant. Takes one User, creates an applicant and adds it to project.
		public async Task AddProjectApplicant(int projectId, string userId, string letter)
        {
			Project projectToUpdate = await dbContext.Projects.Include(p => p.Applicants).Where(p => p.Id == projectId).FirstAsync();
			
			var applicantsToUpdate = projectToUpdate.Applicants;
			// Find the user based on the Id, create an applicant based on the found user and and add it to the project and to the database
			if(dbContext.Users.Any(u=>u.Id == userId) && !projectToUpdate.Applicants.Any(a=>a.UserId == userId))
            {
				var userToApplicantUser = await dbContext.Users.FirstAsync(u => u.Id == userId);
				Applicant applicant = new Applicant { UserId = userToApplicantUser.Id, Letter =  letter, Project = projectToUpdate, ProjectID = projectToUpdate.Id, Username = userToApplicantUser.Username};
				applicant.Project = projectToUpdate;
				dbContext.Applicants.Add(applicant);
				await dbContext.SaveChangesAsync();
				Applicant applicantToAdd = await dbContext.Applicants.Where(a => a.UserId == userId).FirstAsync();
				Console.WriteLine("-----------------------" + applicantToAdd.Id + "------------------------");
				applicantsToUpdate.Add(applicantToAdd);
				projectToUpdate.Applicants = applicantsToUpdate;
				dbContext.Entry(projectToUpdate).State = EntityState.Modified;
				await dbContext.SaveChangesAsync();
			}
            else
            {
				throw new KeyNotFoundException();
            }
			
        }
		// Used in project controller deleteprojectapplicant. Takes one applicant and deletes it from project and database
		public async Task DeleteProjectApplicant(int projectId, string applicantId)
        {
			Project projectToUpdate = await dbContext.Projects.Include(p => p.Applicants).Where(p => p.Id == projectId).FirstAsync();
			ICollection<Applicant> applicantsToUpdate = projectToUpdate.Applicants;
			//Applicant applicant = await dbContext.Applicants.FindAsync(applicantId);
			Applicant applicant = await dbContext.Applicants.Where(a => a.UserId == applicantId).FirstAsync();
			if (applicant == null)
				// Record doesnt exist: https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/ms229021(v=vs.100)?redirectedfrom=MSDN
				throw new KeyNotFoundException();
			applicantsToUpdate.Remove(applicant);
			// Deletes the applicant from the Applicant table in the database
			dbContext.Applicants.Remove(applicant);
			projectToUpdate.Applicants = applicantsToUpdate;
			dbContext.Entry(projectToUpdate).State = EntityState.Modified;


			await dbContext.SaveChangesAsync();
		}
		public async Task<List<ProjectBannerDTO>> GetProjectsByOwner(string ownerId)
		{
			 List<Project> projects = await dbContext.Projects.Where(p => p.Owner == ownerId).ToListAsync();
			return mapper.Map<List<ProjectBannerDTO>>(projects);
		}

	}
}
