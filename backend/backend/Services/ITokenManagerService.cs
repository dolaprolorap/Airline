namespace backend.Services
{
    public interface ITokenManagerService
    {
        bool UpdateTokensByEmail(string email, out Tuple<string, string>? tokens, out Tuple<DateTime, DateTime>? expDates);
        bool UpdateTokensByRefreshToken(string email, string refreshToken, out Tuple<string, string>? tokens, out Tuple<DateTime, DateTime>? expDates);
        bool IsRefreshTokenExpired(string email);
    }
}
