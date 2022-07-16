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
    //[HttpCacheValidation(MustRevalidate = true)]
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

        //[Authorize]
        [HttpGet("usersGuess", Name = "GetUsersGuess")]
        public ActionResult<List<UsersGuess>> GetUserGuess()
        {
            var guesses = _CoveraterRepository.GetUserGuesses();

            if (guesses == null)
                return NotFound();
            var result = _mapper.Map<List<UsersGuess>>(guesses);
            return Ok(result);
        }
        //[Authorize]
        [HttpPost("usersGuess", Name = "CreateUsersGuess")]
        public IActionResult CreateUserGuess([FromBody]UserGuessBinding binding)
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

            };

            //userStats.Guesses.Add(userGuess);
            _CoveraterRepository.CreateUserGuess(userGuess);
           var results = _CoveraterRepository.Save();

            return Ok(userGuess);
        }
        //[Authorize]
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
            var result = userGuesses.Where(x => x.UserId == userStats.UserId).ToList(); //userStats.Guesses;
            //if (phase.HasValue)
            //   return (IActionResult)result.Where(x => x.Phase == phase.Value);

            return Ok(result);
        }
    }
}