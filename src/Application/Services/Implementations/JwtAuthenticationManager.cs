using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Domain.Common;
using Domain.Entities.Identities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ShareLoanApp.Application.Helpers;
using ShareLoanApp.Application.Services.Interfaces;
using ShareLoanApp.Domain.Entities;

namespace ShareLoanApp.Application.Services.Implementations
{
    public class JwtAuthenticationManager: IJwtAuthenticationManager
    {
        private readonly IConfiguration _configuration;
        public JwtAuthenticationManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public TokenReturnHelper Authenticate(User user, IList<string> roles = null)
        {
            var roleClaims = new List<Claim>();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypeHelper.Email, user.Email),
                new Claim(ClaimTypeHelper.UserId, user.Id.ToString()),
                new Claim(ClaimTypeHelper.FullName, $"{user.FirstName} {user.LastName}"),
               // new Claim(ClaimTypeHelper.OrganizationId, user.OrganizationId.ToString()),
            };

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            claims.AddRange(roleClaims);

            var jwtSettings = _configuration.GetSection("JwtSettings");
            var jwtUserSecret = jwtSettings.GetSection("Secret").Value;
            var tokenExpireIn = string.IsNullOrEmpty(jwtSettings.GetSection("TokenLifespan").Value) ? int.Parse(jwtSettings.GetSection("TokenLifespan").Value) : 7;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtUserSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(tokenExpireIn),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return new TokenReturnHelper
            {
                ExpiresIn = tokenDescriptor.Expires,
                AccessToken = jwt
            };
        }
        public string GenerateRefreshToken(Guid userId)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var jwtUserSecret = jwtSettings.GetSection("Secret").Value;
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(jwtUserSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypeHelper.UserId, userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);

            return jwt;
        }
        public bool ValidateRefreshToken(string refreshToken)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var jwtUserSecret = jwtSettings.GetSection("Secret").Value;

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtUserSecret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            {
                return false;
            }

            var expiryAt = jwtSecurityToken.ValidTo;
            if (DateTime.UtcNow > expiryAt)
                return false;
            return true;
        }
        public Guid GetUserIdFromAccessToken(string accessToken)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var jwtUserSecret = jwtSettings.GetSection("Secret").Value;

            var tokenValidationParamters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtUserSecret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParamters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null ||!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                                                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new RestException(HttpStatusCode.BadRequest, ResponseMessages.InvalidToken);
            }

            var userId = principal.FindFirst(ClaimTypeHelper.UserId)?.Value;

            if(userId == null)
                throw new RestException(HttpStatusCode.BadRequest, $"{ResponseMessages.MissingClaim} {ClaimTypeHelper.UserId}");

            return Guid.Parse(userId);
        }
    }
    public class TokenReturnHelper
    {
        public string AccessToken { get; set; }
        public DateTime? ExpiresIn { get; set; }
    }
}
