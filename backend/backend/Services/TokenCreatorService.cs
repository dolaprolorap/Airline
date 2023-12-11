using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services
{
    public class TokenCreatorService : ITokenCreatorService
    {
        IConfiguration _config;

        private TimeSpan _accessTokenLifetime = new TimeSpan(0, 5, 0);
        private TimeSpan _refreshTokenLifetime = new TimeSpan(14, 0, 0 ,0);

        public TokenCreatorService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateAccessToken(string email, string role, out DateTime expDate)
        {
            string? jwtKey = _config.GetSection("JWTKey").Value;
            if (jwtKey == null) throw new ArgumentNullException("JWTKey has value null");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            expDate = DateTime.Now.Add(_accessTokenLifetime);
            IEnumerable<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, role)
            };

            var tokenOptions = new JwtSecurityToken(
                issuer: "http://localhost:5033",
                audience: "http://localhost:5033",
                claims: claims,
                expires: expDate,
                signingCredentials: credentials
            );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }

        public string GenerateRefreshToken(out DateTime expDate)
        {
            var randomBytes = new byte[32];
            expDate = DateTime.Now.Add(_refreshTokenLifetime);
            using (var rand = RandomNumberGenerator.Create())
            {
                rand.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }

        public ClaimsPrincipal GetPrincipal(string accessToken)
        {
            string? jwtKey = _config.GetSection("JWTKey").Value;
            if (jwtKey == null) throw new ArgumentNullException("JWTKey has value null");

            var validationParams = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(accessToken, validationParams, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Token invalid");

            return principal;
        }
    }
}