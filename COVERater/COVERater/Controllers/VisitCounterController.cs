
using COVERater.API.Models;
using COVERater.API.Services;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace COVERater.API.Controllers
{
    [Route("api/V1")]
    [ApiController]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 1)]
    [Produces("application/json")]
    public class VisitCounterController : ControllerBase
    {

        private readonly ICoveraterRepository _CoveraterRepository;


        public VisitCounterController(ICoveraterRepository CoveraterRepository)
        {
            _CoveraterRepository =
                CoveraterRepository ?? throw new ArgumentNullException(nameof(CoveraterRepository));
        }

        [AllowAnonymous]
        [HttpPost("visitCounter", Name = "GetVisitCounter")]
        public async Task<ActionResult<int>> GetVisitCounter()
        {
            var visitCounter = await _CoveraterRepository.GetUpdateVisitCount();
           return Ok(visitCounter.Count);
        }



    }
}
