
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using COVERater.API.Models;
using AutoMapper;
using COVERater.API.Dto;
using COVERater.API.Services;
using Microsoft.AspNetCore.Mvc;
using COVERater.API.Bindings;
using COVERater.API.Helpers;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Extensions;

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
        [HttpGet("user/{id}", Name = "GetUser")]
        public ActionResult<UserDto> GetUsers(int id)
        {
            var authUser = _CoveraterRepository.GetAuthUsers(id);

            if (authUser == null)
                return NotFound();

            var user = authUser.UserStats.OrderByDescending(x => x.CreatedUtc).FirstOrDefault(x =>(!x.Deleted.HasValue || x.Deleted == false));


            Log(new Log()
            {
                CreatedDate = DateTime.UtcNow,
                UserId = authUser.Email,
                Function = "GetUsers",
                Before = JsonSerializer.Serialize(new
                {
                    authUser.RoleType,
                    authUser.RoleId,
                    authUser.ExperienceLevel,
                    authUser.UserStats.Count
                })

            });

            UserDto results = new UserDto();

            if (user == null)
            {
                var newUser = new UserStats()
                {
                    CreatedUtc = DateTime.UtcNow,
                    FinishingPercentPhase = (decimal)1.0,
                    Phase = 1,
                    Guesses = new List<UsersGuess>()
                };
                authUser.UserStats.Add(newUser);
                _CoveraterRepository.Save();

                Log(new Log()
                {
                    CreatedDate = DateTime.UtcNow,
                    UserId = authUser.Email,
                    Function = "GetUsers-Inner",
                    Before = JsonSerializer.Serialize(new
                    {
                        authUser.RoleType,authUser.RoleId,authUser.ExperienceLevel, authUser.UserStats.Count
                    })

                });
                results = _mapper.Map<UserDto>(newUser);
                results.Experience = authUser.ExperienceLevel;
                return Ok(results);

            }
            else
            {

                results = _mapper.Map<UserDto>(user);
                results.Experience = authUser.ExperienceLevel;
                return Ok(results);
            }
        }

        [Authorize]
        [HttpGet("user/{id}/{userId}", Name = "GetUserStats")]
        public ActionResult<UserDto> GetUserStats(int id, int userId)
        {

            var authUser = _CoveraterRepository.GetAuthUsers(id);

            if (authUser == null)
                return NotFound();

            var user = authUser.UserStats.FirstOrDefault(x => x.UserId == userId);
            if (user == null)
                return NotFound();

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

            UserStats userStats = null; 
            if(binding.UserStatsId.HasValue)
                userStats = authUser.UserStats.FirstOrDefault(x=>x.UserId == binding.UserStatsId.Value);
            else
                userStats = authUser.UserStats.OrderByDescending(x => x.CreatedUtc).FirstOrDefault();
            if (userStats == null)
                return NotFound();
            if (binding.FinishedUtc.HasValue)
                userStats.FinishedPhaseUtc = binding.FinishedUtc.Value;
            if (binding.Time.HasValue)
                userStats.TimePhase = userStats.TimePhase == null ? binding.Time.Value : userStats.TimePhase;
            if (binding.FinishingPercent.HasValue)
                userStats.FinishingPercentPhase = binding.FinishingPercent.Value;
            if (binding.PictureCycled.HasValue)
                userStats.PictureCycledPhase = binding.PictureCycled.Value;
            if (binding.Phase.HasValue)
                userStats.Phase = binding.Phase.Value;
            if (binding.Deleted.HasValue)
            {
                userStats.Deleted = binding.Deleted.Value;
                userStats.DeletedDate = DateTime.UtcNow;
            }


            _CoveraterRepository.Save();


            Log(new Log()
            {
                CreatedDate = DateTime.UtcNow,
                UserId = authUser.Email,
                Function = "UpdateUserViaId",
                Before = JsonSerializer.Serialize(new
                {
                    authUser.RoleType,
                    authUser.RoleId,
                    authUser.ExperienceLevel,
                    authUser.UserStats.Count
                })

            });


            var results = _mapper.Map<UserDto>(userStats);
            return Ok(results);
        }

        private async Task<bool> Log(Log log)
        {

            try
            {
                await _CoveraterRepository.Log(log);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }
    }
}