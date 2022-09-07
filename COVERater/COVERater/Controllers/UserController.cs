
using System;
using System.Collections.Generic;
using System.Linq;
using COVERater.API.Models;
using AutoMapper;
using COVERater.API.Dto;
using COVERater.API.Services;
using Microsoft.AspNetCore.Mvc;
using COVERater.API.Bindings;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;

namespace COVERater.API.Controllers
{
    [Route("api/V1")]
    [ApiController]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 0)]
    [HttpCacheValidation(MustRevalidate = true)]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly ICoveraterRepository _CoveraterRepository;
        private readonly IMapper _mapper;



        public UserController(ICoveraterRepository CoveraterRepository, IMapper mapper)
        {
            _CoveraterRepository = CoveraterRepository ?? throw new ArgumentNullException(nameof(CoveraterRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Authorize]
        [HttpGet("users", Name = "GetUsers")]
        public  ActionResult<List<UserDto>> GetUsers()
        {
            var user = _CoveraterRepository.GetUsers();
            if (user == null)
            {
                return NotFound();
            }
            var results = _mapper.Map<List<UserDto>>(user);
            return Ok(results);
        }

        [Authorize]
        [HttpGet("users/results", Name = "GetUsersResults")]
        public ActionResult<List<UsersResultDto>> GetUsersResults()
        {

            var authUsers = _CoveraterRepository.GetAuthUsers();
            if (authUsers == null)
            {
                return NotFound();
            }

            var results = new List<UsersResultDto>();

            foreach (var authUser in authUsers)
            {
                var users = authUser.UserStats.Where(x => x.FinishedPhaseUtc != null);

                foreach (var user in users)
                {
                    if (user.Phase != 2)
                    {
                        List<decimal> diff = new List<decimal>();
                        foreach (var guess in user.Guesses)
                        {
                            diff.Add(Math.Round((Math.Abs(guess.GuessPercentage - guess.SubImage.CoverageRate) * 100),
                                2));
                        }

                        decimal total = 0;
                        foreach (var d in diff)
                        {
                            total = total + d;
                        }

                        user.FinishingPercentPhase = Math.Round((total / user.PictureCycledPhase.Value), 2);
                    }

                    var mappedUser = new UsersResultDto
                    {
                        UserId = user.UserId,
                        CreatedUtc = user.CreatedUtc,
                        Guesses = user.Guesses,
                        FinishingPercentPhase = user.FinishingPercentPhase,
                        FinishedPhaseUtc = user.FinishedPhaseUtc,
                        Role = authUser.RoleType,
                        Email = authUser.Email,
                        Experience = authUser.ExperienceLevel,
                        PictureCycledPhase = user.PictureCycledPhase,
                        Phase = user.Phase,
                        TimePhase = user.TimePhase

                };
                    results.Add(mappedUser);
                }


            }
            
            return Ok(results);
        }

        [HttpGet("user/hash/{id}", Name = "GetUserHash")]
        [Authorize]
        public  ActionResult<UserDto> GetUsers(Guid id)
        {
            var user = _CoveraterRepository.GetUser(id);

            if (user == null)
            {
                return NotFound();
            }

            var results = _mapper.Map<UserDto>(user);
            return Ok(results);
        }

        [Authorize]
        [HttpGet("user/{id}", Name = "GetUser")]
        public  ActionResult<UserDto> GetUsers(int id)
        {
            //var user = _CoveraterRepository.GetUser(id);

            var authUser = _CoveraterRepository.GetAuthUsers(id);

            if (authUser == null)
                return NotFound();

            var user = authUser.UserStats.OrderByDescending(x => x.CreatedUtc).FirstOrDefault();
            //if (user == null)
            //    return NotFound();

            //if (user == null)
            //{
            //    return NotFound();
            //}

            var results = _mapper.Map<UserDto>(user);
            return Ok(results);
        }

        [Authorize]
        [HttpPost("user")]
        public ActionResult<UserDto> CreateUser(CreateUserBinding binding)
        {
            var users = _CoveraterRepository.GetAuthUsers(binding.RoleId);
            var user = new UserStats()
            {
                CreatedUtc = DateTime.UtcNow,
                FinishingPercentPhase = (decimal)1.0,
                Phase = binding.Phase,
                Guesses = new List<UsersGuess>()
            };
            users.UserStats.Add(user);
            _CoveraterRepository.Save();

            var results = _mapper.Map<UserDto>(user);
            return Ok(results);
        }

        [Authorize]
        [HttpPost("user/hash/{id}")]
        public ActionResult<UserDto> UpdateUser(Guid id, [FromBody] UpdateUserBinding binding)
        {
            var user = _CoveraterRepository.GetUser(id);

            if (user == null)
                return NotFound();

            user.FinishedPhaseUtc = binding.FinishedUtc;
            user.TimePhase = binding.Time;
            user.FinishingPercentPhase = binding.FinishingPercent;
            user.PictureCycledPhase = binding.PictureCycled;
            user.Phase = binding.Phase;
           

            _CoveraterRepository.Save();
            var results = _mapper.Map<UserDto>(user);
            return Ok(results);
        }

        [Authorize]
        [HttpPost("user/{id}", Name = "UpdateUser")]
        public IActionResult UpdateUserViaId(int id, [FromBody] UpdateUserBinding binding)
        {

            var authUser = _CoveraterRepository.GetAuthUsers(id);
      
            if (authUser == null)
                return NotFound();

            var userStats = authUser.UserStats.OrderByDescending(x => x.CreatedUtc).FirstOrDefault();
            if (userStats == null)
                return NotFound();

            userStats.FinishedPhaseUtc = binding.FinishedUtc;
            userStats.TimePhase = userStats.TimePhase == null ? binding.Time : userStats.TimePhase;
            userStats.FinishingPercentPhase = binding.FinishingPercent;
            userStats.PictureCycledPhase = binding.PictureCycled;
            userStats.Phase = binding.Phase;

            _CoveraterRepository.Save();

            var results = _mapper.Map<UserDto>(userStats);
            return Ok(results);
        }

        [Authorize]
        [HttpGet("users/emails", Name = "GetUserEmails")]
        public IActionResult GetUserEmails()
        {

            var userEmails = _CoveraterRepository.GetUserEmails();

            if (userEmails == null)
                return NotFound();
         

            var results = _mapper.Map<List<UserEmailDto>>(userEmails);
            return Ok(results);
        }

        [Authorize]
        [HttpPost("users/email", Name = "CreateUserEmails")]
        public IActionResult CreateUserEmails([FromBody] CreateUserEmailBinding binding)
        {
            var email = _mapper.Map<UserEmails>(binding);
            var userEmails = _CoveraterRepository.CreateUserEmail(email);
            _CoveraterRepository.Save();
            var results = _mapper.Map<UserEmailDto>(userEmails);

            return Ok(results);
        }
    }
}