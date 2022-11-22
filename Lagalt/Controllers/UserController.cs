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
    [Route("User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserRepo userRepo;
        private IMapper _mapper;

        public UserController(UserRepo repo, IMapper mapper)
        {
            this.userRepo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// List all users
        /// </summary>
        [EnableCors("AllowAllHeaders")]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var users = await userRepo.GetAllUsers();
            if (users == null) return NotFound("No Users in storage");
            return Ok(users);
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [EnableCors("AllowAllHeaders")]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var users = await userRepo.GetAllUsers();
            var user = users.Find(u => u.Id == id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        /// <summary>
        /// Adds a user to the db
        /// </summary>
        /// <param name="user">User to be created</param>
        /// <returns></returns>
        [EnableCors("AllowAllHeaders")]
        [HttpPost]
        public async Task<ActionResult<List<User>>> AddUser(UserCreateDTO user)
        {
            User domainUser = _mapper.Map<User>(user);
            domainUser.IsPublic = true;
            domainUser = await userRepo.AddUser(domainUser);
            //return Ok(users);

            return CreatedAtAction("Get", new { id = domainUser.Id }, _mapper.Map<UserReadDTO>(domainUser));
        }

        /// <summary>
        /// Updates user
        /// </summary>
        /// <param name="request"></param>
        [EnableCors("AllowAllHeaders")]
        [HttpPut("{id}")]
        public async Task<ActionResult<List<User>>> UpdateUser(UserEditDTO request, string id)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            if (!userRepo.UserExists(id))
            {
                return NotFound();
            }
            User domainUser = _mapper.Map<User>(request);
            await userRepo.UpdateUser(domainUser);
            return NoContent();

        }

        /// <summary>
        /// Deletes user by id
        /// </summary>
        /// <param name="id"></param>
        [EnableCors("AllowAllHeaders")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<User>>> DeleteUser(string id)
        {
            if (!userRepo.UserExists(id))
            {
                return NotFound();
            }
            await userRepo.DeleteUser(id);
            return NoContent();
        }


        /// <summary>
        /// Takes user by id and updates their projects
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectIds"></param>
        /// <returns></returns>
        [EnableCors("AllowAllHeaders")]
        [HttpPut("{id}/projects")]
        public async Task<IActionResult> UpdateUserProjects(string id, List<int> projectIds)
        {
            if (!userRepo.UserExists(id))
            {
                return NotFound();
            }
            try
            {
                await userRepo.UpdateUserProjects(id, projectIds);
            }
            catch (Exception e)
            {

                return BadRequest("Invalid project");
            }
            return NoContent();
        }

        /// <summary>
        /// Handles user login or register
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [EnableCors("AllowAllHeaders")]
        [HttpPost("login")]
        public async Task<IActionResult> LoginOrRegister([FromBody] UserCreateDTO user)
        {

            if (userRepo.UserExists(user.MicrosoftId))
            {
                return Ok(await Get(user.MicrosoftId));
            }
            else
            {
                return Ok(await AddUser(user));
            }
        }

        /// <summary>
        /// Takes a user by id and updates their biography
        /// </summary>
        /// <param name="biogtraphy"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [EnableCors("AllowAllHeaders")]
        [HttpPut("{id}/bio")]
        public async Task<IActionResult> EditBiography([FromBody] string biogtraphy, string id)
        {
            if (userRepo.UserExists(id))
            {
                User user = await userRepo.GetUser(id);
                user.Biography = biogtraphy;
                await userRepo.UpdateUser(user);
                return NoContent();
            }
            else return BadRequest();
        }

        /// <summary>
        /// Takes users id and edits their skills
        /// </summary>
        /// <param name="skills"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [EnableCors("AllowAllHeaders")]
        [HttpPut("{id}/skills")]
        public async Task<IActionResult> EditSkills([FromBody] string[] skills, string id)
        {
            if (userRepo.UserExists(id))
            {
                User user = await userRepo.GetUser(id);
                user.Skills = skills;
                await userRepo.UpdateUser(user);
                return NoContent();
            }
            else return BadRequest();
        }

        /// <summary>
        /// Takes users id and lists their skills
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [EnableCors("AllowAllHeaders")]
        [HttpGet("{id}/skills")]
        public async Task<ActionResult<List<string>>> GetSkills(string id)
        {
            if (userRepo.UserExists(id))
            {
                User user = await userRepo.GetUser(id);

                var skills = user.Skills;
                if (skills != null)
                {
                    return skills.ToList();
                }
                else return new List<string>();
            }
            else return BadRequest("User not found");
        }

    }
}