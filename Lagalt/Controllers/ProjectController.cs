using Lagalt.Models.DTO;
using Lagalt.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace Lagalt.Controllers
{
    [Route("project")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private ProjectRepo projectRepo;

        public ProjectController(ProjectRepo repo)
        {
            this.projectRepo = repo;
        }

        /// <summary>
        /// List all projects
        /// </summary>
        [HttpGet]
        [EnableCors("AllowAllHeaders")]
        public async Task<ActionResult> Get()
        {
            var projects = await projectRepo.GetAllProjects();
            if (projects == null) return NotFound("No Projects in storage");
            return Ok(projects);
        }

        /// <summary>
        /// List all project banners
        /// </summary>
        /// <returns></returns>
		[EnableCors("AllowAllHeaders")]
        [HttpGet("AllBanners")]
        public async Task<ActionResult> GetBanners()
        {
            var projects = await projectRepo.GetAllProjectBanners();
            if (projects == null) return NotFound("No Projects in storage");
            return Ok(projects);
        }

        /// <summary>
        /// Show project by id
        /// </summary>
        /// <param name="id"></param>

        [HttpGet("{id}")]
        [EnableCors("AllowAllHeaders")]
        public async Task<ActionResult<ProjectDTO>> GetById(int id)
        {
            var project = await projectRepo.GetProjectById(id);
            if (project == null) return NotFound("Project not found");
            return Ok(project);
        }

        /// <summary>
        /// Get projects banner by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet("{id}/Banner")]
        [EnableCors("AllowAllHeaders")]
        public async Task<ActionResult<ProjectBannerDTO>> GetBannerById(int id)
        {
            var project = await projectRepo.GetProjectBannerById(id);
            if (project == null) return NotFound("Project not found");
            return Ok(project);
        }

        /// <summary>
        /// Get projects details by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet("{id}/Details")]
        [EnableCors("AllowAllHeaders")]
        public async Task<ActionResult<ProjectDetailsDTO>> GetDetailsById(int id)
        {
            var project = await projectRepo.GetProjectDetailsById(id);
            if (project == null) return NotFound("Project not found");
            return Ok(project);
        }

        /// <summary>
        /// Add new project
        /// </summary>
        /// <param name="project"></param>
        [HttpPost]
        [EnableCors("AllowAllHeaders")]
        public async Task<ActionResult<ProjectCreateDTO>> AddProject(ProjectCreateDTO project)
        {
            var projects = await projectRepo.AddProject(project);
            return Ok(projects);
        }

        /// <summary>
        /// Update a project
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ProjectDTO>> UpdateProject(ProjectDTO projectDto)
        {
            var project = await projectRepo.EditProject(projectDto);
            if (project == null) return NotFound("Project not found");
            return Ok(project);
        }

        /// <summary>
        /// Delete project by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ActionResult<List<ProjectDTO>>> DeleteProject(int id)
        {
            var projects = await projectRepo.RemoveProject(id);
            if (projects == null) return NotFound("Project not found");
            return Ok(projects);
        }

        /// <summary>
        /// Update user in the project
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
		[HttpPut("{id}/users")]
        [EnableCors("AllowAllHeaders")]
        public async Task<IActionResult> UpdateProjectUsers(int id, List<string> userIds)
        {
            if (!projectRepo.ProjectExists(id))
                return NotFound();
            try
            {
                await projectRepo.UpdateProjectUsers(id, userIds);
            }
            catch (Exception e)
            {
                return BadRequest("Invalid user");
            }
            return NoContent();
        }

        /// <summary>
        /// Lists the owner of the project
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpGet("owner/{id}")]
        [EnableCors("AllowAllHeaders")]
        public async Task<ActionResult<List<ProjectBannerDTO>>> GetProjectsByOwnerId(string id)
        {
            try
            {
                var projects = await projectRepo.GetProjectsByOwner(id);
                return projects;

            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        /// <summary>
        /// Update applicants in the project
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userIds"></param>
        /// <returns></returns>
		[HttpPut("{id}/applicants")]
        [EnableCors("AllowAllHeaders")]
        public async Task<IActionResult> UpdateProjectApplicants(int id, List<string> userIds)
        {
			if (!projectRepo.ProjectExists(id))
				return NotFound();
			try
			{
				await projectRepo.UpdateProjectApplicants(id, userIds);
			}
			catch 
			{
				return BadRequest("Invalid applicant");
			}
			return NoContent();
		}

        /// <summary>
        /// Takes one User, creates an applicant and adds it to project.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="applicant"></param>
        /// <returns></returns>
		[HttpPut("{id}/addApplicant")]
		[EnableCors("AllowAllHeaders")]
		public async Task<IActionResult> AddProjectApplicant(int id, [FromBody] ApplicantDTO applicant)
		{
			if (!projectRepo.ProjectExists(id))
				return NotFound();
			try
			{
				await projectRepo.AddProjectApplicant(id, applicant.UserId, applicant.Letter);
			}
			catch
			{
				return BadRequest("Invalid applicant");
			}
			return NoContent();
		}

        /// <summary>
        /// Takes one applicant and deletes it from project and database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
		[HttpPut("{id}/deleteApplicant")]
		[EnableCors("AllowAllHeaders")]
		public async Task<IActionResult> DeleteProjectApplicant(int id,[FromBody] string userId)
		{
			if (!projectRepo.ProjectExists(id))
				return NotFound();
			try
			{
				await projectRepo.DeleteProjectApplicant(id, userId);
			}
			catch
			{
				return BadRequest("Invalid applicant");
			}
			return NoContent();
		}

        /// <summary>
        /// Updates the projects progress
        /// </summary>
        /// <param name="progress"></param>
        /// <param name="id"></param>
        /// <returns></returns>
		[HttpPut("{id}/progress")]
		[EnableCors("AllowAllHeaders")]
		public async Task<IActionResult> EditProgress([FromBody] string progress, int id)
		{
			if (projectRepo.ProjectExists(id))
			{
				ProjectDTO project = await projectRepo.GetProjectById(id);
				project.Progress = progress;
				await projectRepo.EditProject(project);
				return Ok(project);
			}
			else return BadRequest();
		}
	}
}
