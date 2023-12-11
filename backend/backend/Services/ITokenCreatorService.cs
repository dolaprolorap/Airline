using System.Security.Claims;

namespace backend.Services
{
    public interface ITokenCreatorService
    {
        string GenerateAccessToken(string email, string role, out DateTime expDate);
        string GenerateRefreshToken(out DateTime expDate);
        ClaimsPrincipal GetPrincipal(string accessToken);
    }
}