using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using COVERater.API.Models;
using AutoMapper;
using COVERater.API.Bindings;
using COVERater.API.Services;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace COVERater.API.Controllers
{
    [Route("api/V1")]
    [ApiController]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 1)]
    [Produces("application/json")]
    public class UserGuessController : ControllerBase
    {

        private readonly ICoveraterRepository _CoveraterRepository;
        private readonly IMapper _mapper;

        public UserGuessController(ICoveraterRepository CoveraterRepository, IMapper mapper)
        {
            _CoveraterRepository =
                CoveraterRepository ?? throw new ArgumentNullException(nameof(CoveraterRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [Authorize]
        [HttpGet("usersGuess", Name = "GetUsersGuess")]
        public ActionResult<List<UsersGuess>> GetUserGuess()
        {
            var guesses = _CoveraterRepository.GetUserGuesses();

            if (guesses == null)
                return NotFound();
            var result = _mapper.Map<List<UsersGuess>>(guesses);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("usersGuess", Name = "CreateUsersGuess")]
        public async Task<IActionResult> CreateUserGuess([FromBody]UserGuessBinding binding)
        {
            var auth = _CoveraterRepository.GetAuthUsers(binding.RoleId);
            if (auth == null)
                return NotFound();
            var authUserStats = auth.UserStats.OrderByDescending(x => x.CreatedUtc).FirstOrDefault();
            if (authUserStats == null)
                return NotFound();

            var userStats = _CoveraterRepository.GetUser(authUserStats.UserId);


            var userGuess = new UsersGuess
            {
                GuessPercentage = binding.GuessPercentage,
                SubImageId = binding.SubImageId,
                Phase = binding.Phase,
                GuessTimeUtc = DateTime.UtcNow,
                UserId = userStats.UserId,  
                UserStats = userStats

            };
            
            await _CoveraterRepository.CreateUserGuess(userGuess);

            return Ok(userGuess);
        }
        [Authorize]
        [HttpCacheIgnore]
        [HttpGet("usersGuess/{id}/{phase?}")]
        public async Task<IActionResult> GetUserGuess(int id, byte? phase)
        {
            var auth = _CoveraterRepository.GetAuthUsers(id);
            if (auth == null)
                return NotFound();
            var userStats = auth.UserStats.OrderByDescending(x => x.CreatedUtc).FirstOrDefault();
            if (userStats == null)
                return NotFound();

            var userGuesses = await _CoveraterRepository.GetUserGuesses();
            var result = userGuesses.Where(x => x.UserId == userStats.UserId).OrderByDescending(x=>x.UsersGuessId).ToList(); //userStats.Guesses;
            if (phase.HasValue && phase.Value == 2)
                return Ok(result.Take(10));

            return Ok(result);
        }

        [Authorize]
        [HttpCacheIgnore]
        [HttpGet("getGuessesById/{userStatId}")]
        public async Task<IActionResult> GetSingleUserGuess(int userStatId)
        {
            var userGuesses = await _CoveraterRepository.GetUserGuesses();
            var result = userGuesses.Where(x => x.UserId == userStatId).ToList(); //userStats.Guesses;
         
            return Ok(result);
        }
    }
}