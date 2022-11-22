using AutoMapper;
using System;
using Microsoft.EntityFrameworkCore;
using Lagalt.Models;
using Microsoft.AspNetCore.Identity;

namespace Lagalt.Repositories
{
    public class UserRepo
    {
        private readonly LagaltContext dbContext;
        private readonly IMapper mapper;
        public UserRepo(LagaltContext db, IMapper mapper)
        {
            dbContext = db;
            this.mapper = mapper;
        }


        public async Task<User> GetUser(string id)
        {
            return await dbContext.Users.FirstAsync(u => u.Id == id);
        }

        public async Task<List<User>> GetAllUsers()
        {
            List<User> users = await dbContext.Users
                .Include(u => u.Projects)
                .Include(u => u.CollaborationProjects)
                .ToListAsync();
            if (users.Count == 0) return null;

            return users.Select(user => mapper.Map<User>(user)).ToList();
        }
        public async Task<User> AddUser(User user)
        {
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return user;
        }
        public async Task UpdateUser(User user)
        {
            dbContext.Entry(user).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
        }
        public bool UserExists(string id)
        {
            return dbContext.Users.Any(u => u.Id == id);
        }
        public async Task DeleteUser(string id)
        {
            var user = await dbContext.Users.FindAsync(id);
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
        }
        public async Task UpdateUserProjects(string userId, List<int> projects)
        {
            User userToUpdate = await dbContext.Users
                .Include(u => u.Projects).Where(u => u.Id == userId).FirstAsync();
            List<Project> projectsToUpdate = new();
            foreach (var projectId in projects)
            {
                Project proj = await dbContext.Projects.FindAsync(projectId);
                if (proj == null)
                    // Record doesnt exist: https://docs.microsoft.com/en-us/previous-versions/dotnet/netframework-4.0/ms229021(v=vs.100)?redirectedfrom=MSDN
                    throw new KeyNotFoundException();
                projectsToUpdate.Add(proj);
            }
            userToUpdate.Projects = projectsToUpdate;
            await dbContext.SaveChangesAsync();
        }
    }
}
