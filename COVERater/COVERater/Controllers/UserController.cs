
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

        //[Authorize]
        [HttpGet("user/{id}", Name = "GetUser")]
        public  ActionResult<UserDto> GetUsers(int id)
        {
            //var user = _CoveraterRepository.GetUser(id);

            var authUser = _CoveraterRepository.GetAuthUsers(id);

            if (authUser == null)
                return NotFound();

            var user = authUser.UserStats.OrderByDescending(x => x.CreatedUtc).FirstOrDefault();
            if (user == null)
                return NotFound();

            //if (user == null)
            //{
            //    return NotFound();
            //}

            var results = _mapper.Map<UserDto>(user);
            return Ok(results);
        }

        //[Authorize]
        [HttpPost("user")]
        public ActionResult<UserDto> CreateUser(CreateUserBinding binding)
        {
            var users = _CoveraterRepository.GetAuthUsers(binding.RoleId);
            var user = new UserStats()
            {
                CreatedUtc = DateTime.UtcNow,
                FinishingPercentPhase = (decimal)1.0
            };
            users.UserStats.Add(user);
            _CoveraterRepository.Save();

            var results = _mapper.Map<UserDto>(user);
            return Ok(results);
        }

        //[Authorize]
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

        //[Authorize]
        [HttpPost("user/{id}", Name = "UpdateUser")]
        public IActionResult UpdateUserViaId(int id, [FromBody] UpdateUserBinding binding)
        {

            var authUser = _CoveraterRepository.GetAuthUsers(id);
      
            if (authUser == null)
                return NotFound();

            var userStats = authUser.UserStats.OrderByDescending(x => x.CreatedUtc).FirstOrDefault();
            if (userStats == null)
                return NotFound();

            //var userAdd = new UserStats()
            //{

            userStats.FinishedPhaseUtc = binding.FinishedUtc;
            userStats.TimePhase = binding.Time;
            userStats.FinishingPercentPhase = binding.FinishingPercent;
            userStats.PictureCycledPhase = binding.PictureCycled;
            userStats.Phase = binding.Phase;
            userStats.Guesses = new List<UsersGuess>();
            //};


            //authUser.UserStats.Add(userAdd);
            //switch (binding.Phase)
            //{
            //    case 0:
            //        user.FinishedPhase1Utc = binding.FinishedUtc;
            //        user.TimePhase1 = binding.Time;
            //        user.FinishingPercentPhase1 = binding.FinishingPercent;
            //        user.PictureCycledPhase1 = binding.PictureCycled;
            //        break;
            //    case 1:
            //        user.FinishedPhase2Utc = binding.FinishedUtc;
            //        user.TimePhase2 = binding.Time;
            //        user.FinishingPercentPhase2 = binding.FinishingPercent;
            //        user.PictureCycledPhase2 = binding.PictureCycled;
            //        break;
            //    case 2:
            //        user.FinishedPhase3Utc = binding.FinishedUtc;
            //        user.TimePhase3 = binding.Time;
            //        user.FinishingPercentPhase3 = binding.FinishingPercent;
            //        user.PictureCycledPhase3 = binding.PictureCycled;
            //        break;
            //}

            //_CoveraterRepository.CreateUser(userAdd);
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