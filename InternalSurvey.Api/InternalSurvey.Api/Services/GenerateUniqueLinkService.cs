using InternalSurvey.Api.Exceptions;
using InternalSurvey.Api.Helpers;
using InternalSurvey.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Services
{
    public class GenerateUniqueLinkService : IGenerateUniqueLinkService
    {
        private readonly IConfiguration _configuration;


        public GenerateUniqueLinkService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateUniqueLink(string email, int surveyId, string appUrl, DateTime endDate)
        {
            var claims = new List<Claim>
            {
                new Claim("Email", email),
                new Claim("SurveyId", surveyId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:UniqueLinkSecret")));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = endDate;

            var token = new JwtSecurityToken(
                "issuer",
                "issuer",
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            var genToken = new JwtSecurityTokenHandler().WriteToken(token);
            var uniqueLink = appUrl + "?token=" + genToken;
            return uniqueLink;
        }

        public List<Claim> GetClaims(string token)
        {
            var claims = new List<Claim>();
            try
            {
                var isValidToken = ValidateToken(token);
                if (isValidToken)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                    claims = securityToken.Claims.ToList();
                    return claims;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return claims;
        }


        private bool ValidateToken(string authToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters();

                SecurityToken validatedToken;
                tokenHandler.ValidateToken(authToken, validationParameters, out validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidIssuer = "issuer",
                ValidAudience = "issuer",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("AppSettings:UniqueLinkSecret")))
            };
        }

    }
}
