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
    [Route("api")]
    [ApiController]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 10)]
    [HttpCacheValidation(MustRevalidate = true)]
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

            if (guesses == null || !guesses.Any())
                return NotFound();
            var result = _mapper.Map<List<UsersGuess>>(guesses);
            return Ok(result);
        }
        [Authorize]
        [HttpPost("usersGuess", Name = "CreateUsersGuess")]
        public IActionResult CreateUserGuess([FromBody]UserGuessBinding binding)
        {

            var userGuess = new UsersGuess
            {
                GuessPercentage = binding.GuessPercentage,
                ImageId = binding.ImageId,
                UserId = binding.UserId,
                Phase = binding.Phase
            };

            var newUserGuess = _CoveraterRepository.CreateUserGuess(userGuess);
            _CoveraterRepository.Save();

            return Ok(newUserGuess);
        }
        [Authorize]
        [HttpGet("usersGuess/{id}/{phase?}")]
        public IActionResult GetUserGuess(int id, byte? phase)
        {

            var userGuesses = _CoveraterRepository.GetUserGuesses();
            var result = userGuesses.Where(x => x.UserId == id);
            if (phase.HasValue)
                result = result.Where(x => x.Phase == phase.Value);

            return Ok(result);
        }
    }
}