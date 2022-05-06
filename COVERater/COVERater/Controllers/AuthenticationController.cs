using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using COVERater.API.Bindings;
using COVERater.API.Helpers;
using COVERater.API.Models;
using COVERater.API.Services;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;
namespace COVERater.API.Controllers
{
    [Route("api")]
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

        public AuthenticationController(ICoveraterRepository CoveraterRepository, IMapper mapper,
            IOptions<AppSettings> appSettings, IConfiguration config)
        {
            _CoveraterRepository = CoveraterRepository ?? throw new ArgumentNullException(nameof(CoveraterRepository));
            _appSettings = appSettings.Value;
            _config = config;
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
            var passwordHash = new PasswordHasher(10000);
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
                new Claim(JwtRegisteredClaimNames.Sub,"Guest" /*model.UserName*/),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, AuthTime.ToString()) 
            };

            var token = new JwtSecurityToken(
                issuer: _appSettings.Issuer,
                audience: _appSettings.Issuer,
                claims,
                expires:DateTime.Now.AddMinutes(120),
                signingCredentials:credentials);

            var encodeToken = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new AuthResponse()
            {
                BearerToken = encodeToken,
                RoleType = user.RoleType,
                ExpiryDate = DateTime.Now.AddMinutes(120)
            };

            var accessToken = new Token
            {
                UserGuid = "",//unknown at this point
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

        //If need to create new password
        //[AllowAnonymous]
        //[HttpPost("hash/authenticate")]
        //public IActionResult Authenticate(string password)
        //{
        //    var passwordHash = new PasswordHasher(4859);
        //    var temp = passwordHash.Hash(password);
        //    return Ok(temp);
        //}


    }
}