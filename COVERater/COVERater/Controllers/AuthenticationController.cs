using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using COVERater.API.Bindings;
using COVERater.API.Dto;
using COVERater.API.Helpers;
using COVERater.API.Models;
using COVERater.API.Services;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
namespace COVERater.API.Controllers
{
    [Route("api/V1")]
    [ApiController]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 1)]
    [HttpCacheValidation(MustRevalidate = true)]
    [Produces("application/json",
        "application/vnd.marvin.hateoas+json")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ICoveraterRepository _CoveraterRepository;
        private readonly AppSettings _appSettings;
        private IConfiguration _config;
        private IMapper _mapper;

        public AuthenticationController(ICoveraterRepository CoveraterRepository, IMapper mapper,
            IOptions<AppSettings> appSettings, IConfiguration config)
        {
            _CoveraterRepository = CoveraterRepository ?? throw new ArgumentNullException(nameof(CoveraterRepository));
            _appSettings = appSettings.Value;
            _config = config;
            _mapper = mapper;
        }

        /// <summary>
        /// update Login logging
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userHash"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("token/{id}/{userHash}")]
        public IActionResult CreateToken(int id, string userHash)
        {
            var token = _CoveraterRepository.GetToken(id);
            token.UserGuid = userHash;
            _CoveraterRepository.Save();

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> CreateAuthUserAsync()
        {

            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            var temp = _rdm.Next(_min, _max);
            var passwordHash = new PasswordHasher(temp);
            var tempPassword = "";
            tempPassword = passwordHash.RandomPassword();
            var tempPassHash = passwordHash.Hash(tempPassword);
            var tempUsername = Guid.NewGuid().ToString();

            var authUser = new AuthUsers()
            {
                UserName = tempUsername,
                Password = tempPassHash,
                Email = tempUsername,
                ExperienceLevel = 1,
                RoleType = 1,
                HashUser = Guid.NewGuid()
            };
            
            var user =_CoveraterRepository.CreateAuthUsers(authUser);

              var log =  await Log(new Log()
                {
                    CreatedDate = DateTime.UtcNow,
                    UserId = authUser.Email,
                    Function = "CreateAuthUserAsync",
                    Before = JsonSerializer.Serialize(new
                    {
                        authUser.RoleType,
                        authUser.RoleId,
                        authUser.ExperienceLevel
                    })

                });

              // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var AuthTime = DateTime.Now;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, tempUsername),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, AuthTime.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _appSettings.Issuer,
                audience: _appSettings.Issuer,
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new AuthResponse()
            {
                BearerToken = encodeToken,
                RoleType = 1,
                ExpiryDate = DateTime.Now.AddMinutes(120),
                UserStats = user.UserStats,
                UserName = tempUsername,
                RoleId = user.RoleId
            };

            var accessToken = new Token
            {
                UserGuid = user.HashUser.Value.ToString(), //unknown at this point
                CreateTime = AuthTime,
                ExpiresTime = DateTime.Now.AddMinutes(120),
                RoleId = user.RoleType,
                UserId = user.RoleId
            };
            //create login token
            var tokenResult = _CoveraterRepository.CreateToken(accessToken);
            _CoveraterRepository.Save();

            response.AccessId = tokenResult.TokenId;

            Log(new Log()
            {
                CreatedDate = DateTime.UtcNow,
                UserId = response.UserName,
                Function = "Authenticate",
                Before = JsonSerializer.Serialize(response)

            });

            return Ok(response);
        }

        [Authorize]
        [HttpGet("authUser/experience/{id}")]
        public async Task<IActionResult> GetAuthUserExperienceAsync(int id)
        {

            var authUser = _CoveraterRepository.GetAuthUsers(id);

            if (authUser == null)
            {
                return NotFound();
            }
            var results = _mapper.Map<AuthUserResultsDto>(authUser);

           await Log(new Log()
            {
                CreatedDate = DateTime.UtcNow,
                UserId = authUser.Email,
                Function = "GetAuthUserExperienceAsync",
               Before = JsonSerializer.Serialize(new
               {
                   authUser.RoleType,
                   authUser.RoleId,
                   authUser.ExperienceLevel,
                   authUser.UserStats.Count
               })

           });
          
            return Ok(results);
        }


        [Authorize]
        [HttpPost("authUsers/results")]
        public async Task<IActionResult> GetAuthUsers()
        {
            var authUsers =_CoveraterRepository.GetAuthUsers();
            if (authUsers == null)
            {
                return NotFound();
            }

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
                }
                
            }

            var results = _mapper.Map<List<AuthUserResultsDto>>(authUsers);

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