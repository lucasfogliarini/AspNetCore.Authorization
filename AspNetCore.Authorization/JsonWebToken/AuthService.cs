using AspNetCore.Authorization.Controllers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;

namespace AspNetCore.Authorization.JsonWebToken
{
    public class AuthService(JwtConfiguration jwtConfiguration) : IAuthService
    {
        private readonly JwtConfiguration _jwtConfiguration = jwtConfiguration;

        public Jwt CreateJwt(AuthenticationInput authenticationInput)
        {
            var claimsIdentity = Authorize(authenticationInput);
            var securityTokenDescriptor = CreateTokenDescriptor(claimsIdentity);
            var jwt = new Jwt
            {
                User = authenticationInput.User,
                JwToken = GenerateToken(securityTokenDescriptor),
                ExpiresAt = securityTokenDescriptor.Expires.GetValueOrDefault()
            };

            return jwt;
        }
        public static SymmetricSecurityKey GetSymmetricSecurityKey(string securityKey)
        {
            var key = Encoding.ASCII.GetBytes(securityKey!);
            return new SymmetricSecurityKey(key);
        }

        private ClaimsIdentity Authorize(AuthenticationInput authenticationInput)
        {
            ClaimsIdentity claimsIdentity = new(
            [
                //new(ClaimTypes.Name, name)
                new(ClaimTypes.Actor, authenticationInput.User),
                new(ClaimTypes.Role, "Admin"),
                new(AuthenticationClaimTypes.API_CLAIM, "weather_api"),
                new(AuthenticationClaimTypes.RESOURCE_CLAIM, nameof(WeatherForecastController.Get5WeatherForecasts)),
            ]);
            return claimsIdentity;
        }
        private SecurityTokenDescriptor CreateTokenDescriptor(ClaimsIdentity claimsIdentity)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //more info in https://balta.io/artigos/aspnet-5-autenticacao-autorizacao-bearer-jwt#autorizando
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMonths(6),
                SigningCredentials = new SigningCredentials(GetSymmetricSecurityKey(_jwtConfiguration.SecurityKey), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenDescriptor;
        }
        private static string GenerateToken(SecurityTokenDescriptor securityTokenDescriptor)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(securityTokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
    }
}
