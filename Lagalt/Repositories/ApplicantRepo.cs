using AutoMapper;
using System;
using Microsoft.EntityFrameworkCore;
using Lagalt.Models;
namespace Lagalt.Repositories
{
    public class ApplicantRepo
    {
        private readonly LagaltContext dbContext;
        private readonly IMapper mapper;
        public ApplicantRepo(LagaltContext db, IMapper mapper)
        {
            dbContext = db;
            this.mapper = mapper;
        }
        public async Task<List<Applicant>> GetAllApplicants()
        {
            List<Applicant> applicants = await dbContext.Applicants
                .ToListAsync();
            if (applicants.Count == 0) return null;
           

            return applicants.Select(applicant => mapper.Map<Applicant>(applicant)).ToList();
        }
        public async Task<Applicant> GetApplicant(int id)
        {
            return await dbContext.Applicants.FirstAsync(a => a.Id == id);
        }
        public async Task<Applicant> AddApplicant(Applicant applicant)
        {
            dbContext.Applicants.Add(applicant);
            await dbContext.SaveChangesAsync();
            return applicant;
        }
        public bool ApplicantExists(int id)
        {
            return dbContext.Applicants.Any(u => u.Id == id);
        }
        public async Task DeleteApplicant(string id)
        {
            var applicant = await dbContext.Applicants.FindAsync(id);
            dbContext.Applicants.Remove(applicant);
            await dbContext.SaveChangesAsync();
        }
        
    }
}
