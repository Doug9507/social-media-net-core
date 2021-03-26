using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.Core.CustomEntities;
using SocialMedia.Core.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialMedia.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public UserLoginResponse GetToken(UserLoginRequest userLoginRequest)
        {
            var userLoginResponse = new UserLoginResponse();
            userLoginResponse.User = userLoginRequest.User;
            if (IsUserValid())
            {
                userLoginResponse.Token = GenerateToken();
            }
            return userLoginResponse;
        }

        private string GenerateToken()
        {
            //Header
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));
            var signinCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(signinCredentials);


            var claims = new[]
            {
                new Claim(ClaimTypes.Name,"Doug95"),
                new Claim(ClaimTypes.Email,"dpbarbaran7@gmail.com"),
                new Claim(ClaimTypes.MobilePhone,"955700369")
            };
            //Payload
            var payload = new JwtPayload(
                _configuration["Authentication:Issuer"],
                _configuration["Authentication:Audience"],
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(3)
            );

            //Signature
            var tokenGenerate = new JwtSecurityToken(header, payload);

            var token = new JwtSecurityTokenHandler().WriteToken(tokenGenerate);



            return token;
        }

        private bool IsUserValid()
        {
            return true;
        }
    }
}
