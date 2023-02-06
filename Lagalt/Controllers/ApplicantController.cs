using AutoMapper;
using Lagalt.Models;
using Lagalt.Models.DTO;
using Lagalt.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lagalt.Controllers
{
	[Route("applicant")]
	[ApiController]
	public class ApplicantController : ControllerBase
    {
		private ApplicantRepo applicantRepo;
		private IMapper _mapper;

		public ApplicantController(ApplicantRepo repo, IMapper mapper)
		{
			this.applicantRepo = repo;
			_mapper = mapper;
		}

        /// <summary>
        /// List all applicants
        /// </summary>
        /// <returns></returns>
		[EnableCors("AllowAllHeaders")]
		[HttpGet]
		public async Task<ActionResult> Get()
		{
			var applicants = await applicantRepo.GetAllApplicants();
			if (applicants == null) return NotFound("No Applicants in storage");
			return Ok(applicants);
		}

        /// <summary>
        /// Get applicant by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
		[EnableCors("AllowAllHeaders")]
		[HttpGet("{id}")]
		public async Task<ActionResult<Applicant>> Get(int id)
        {
			var applicant = await applicantRepo.GetApplicant(id);
			if (applicant == null) return NotFound("No applicant found");
			return Ok(applicant);
        }
	}
}
