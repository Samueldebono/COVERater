using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using API.Traidy.Services;
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
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
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

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public ActionResult<AuthResponse> Authenticate([FromBody] AuthBinding model)
        {
            var user = _CoveraterRepository.Authenticate(model.UserName);
            // return null if user not found
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            //password check
            var parts = user.Password.Split('.', 3);
            var passwordHash = new PasswordHasher(Convert.ToInt32(parts[0]));
            var passwordCheck = passwordHash.Check(user.Password, model.Password);

            if (!passwordCheck.Verified)
                return BadRequest("Username or password is incorrect");

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var AuthTime = DateTime.Now;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, model.UserName),
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
                RoleType = user.RoleType,
                ExpiryDate = DateTime.Now.AddMinutes(120),
                UserStats = user.UserStats,
                UserName = user.UserName,
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
            return Ok(response);
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
        [HttpPost("authUser")]
        public async Task<IActionResult> CreateAuthUserAsync(AuthBinding binding)
        {
            var user = new UserStats()
            {
                CreatedUtc = DateTime.UtcNow
            };
          var newUser = _CoveraterRepository.CreateUser(user);
            _CoveraterRepository.Save();
          
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            var temp = _rdm.Next(_min, _max);
            var passwordHash = new PasswordHasher(temp);
            binding.Password = passwordHash.RandomPassword();
            var tempPassHash = passwordHash.Hash(binding.Password);

            var authUser = new AuthUsers()
            {
                UserName = binding.UserName,
                Password = tempPassHash,
                Email = binding.Email,
                ExperienceLevel = binding.Experience.Value,
                RoleType = binding.RoleType.HasValue ? binding.RoleType.Value : (byte)1,
                HashUser = Guid.NewGuid()
            };

            //authUser.SetUserId(newUser.UserId);
            _CoveraterRepository.CreateAuthUsers(authUser);
            var emailer = new SendgridService(_CoveraterRepository);
            await emailer.SendActivationEmailAsync(authUser, binding.Password);

            if (authUser.RoleType == 1)
            {
               await emailer.AddContact(authUser);
            }
            

            return Ok();
        }

        [Authorize]
        [HttpPost("authUser/experience")]
        public async Task<IActionResult> CreateAuthUserAsync(UserExperienceUpdateBinding binding)
        {

            var newUser = _CoveraterRepository.GetAuthUsers(binding.UserId);

          if (newUser == null)
          {
              return NotFound();
          }

          newUser.ExperienceLevel = binding.Experience;
          _CoveraterRepository.Save();

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("reset/password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordBinding binding)
        {
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            var passwordHash = new PasswordHasher(_rdm.Next(_min, _max));
            var password = passwordHash.RandomPassword();
            var tempPassHash = passwordHash.Hash(password);

            var authUser = new AuthUsers()
            {
               Password = tempPassHash,
                Email = binding.Email
            };
            
            _CoveraterRepository.ResetPassword(authUser);
            var emailer = new SendgridService(_CoveraterRepository);
            await emailer.SendResetPasswordAsync(authUser, password);

            return Ok();
        }


        [AllowAnonymous]
        [HttpGet("authUserExists")]
        public IActionResult AuthUserExists(string email)
        {
            return Ok(_CoveraterRepository.AuthUsers(email) != null);

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

            return Ok();

        }


    }
}