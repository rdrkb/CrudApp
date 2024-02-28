using A.Contracts.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace C.Business.Security
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public TokenService(IConfiguration configuration)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]));

            // Setup token validation parameters
            _tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,
                ValidateIssuer = false, // Set to true if you have an issuer
                ValidateAudience = false, // Set to true if you have an audience
                ClockSkew = TimeSpan.Zero // This will make the token expiration exact
            };
        }

        public string CreateToken(AccountModel accountModel)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("username", accountModel.Username),
                    new Claim("role", accountModel.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GetUsernameFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // Validate the token and extract claims
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out _);

                // Retrieve the username claim
                var usernameClaim = principal.FindFirst("username");

                if (usernameClaim != null)
                {
                    return usernameClaim.Value;
                }
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions here
                Console.WriteLine($"Token validation failed: {ex.Message}");
            }

            // Return null if username claim is not found or if token is invalid
            return null;
        }
    }
}
