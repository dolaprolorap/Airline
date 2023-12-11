using backend.DataAccess.Repository;
using backend.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class TokenManagerService : ITokenManagerService
    {
        private IUnitOfWork _unitOfWork;
        private ITokenCreatorService _tokenCreator;
        private ILogger<TokenManagerService> _logger;

        public TokenManagerService(IUnitOfWork unit, ITokenCreatorService tokenCreator, ILogger<TokenManagerService> logger)
        {
            _unitOfWork = unit;
            _tokenCreator = tokenCreator;
            _logger = logger;
        }

        public bool UpdateTokensByEmail(string email, out Tuple<string, string>? tokens, out Tuple<DateTime, DateTime>? expDates)
        {
            var queryableUserToken = _unitOfWork.TokenRepo.ReadWhere(t => t.User.Email == email);
            if (!queryableUserToken.Any())
            {
                _logger.LogError("Try update token on not existing login: {0}", email);
                tokens = null;
                expDates = null;
                return false;
            }

            var userToken = queryableUserToken.First();

            _unitOfWork.UserRepo.ReadWhere(user => user.Email == email).Load();
            var user = userToken.User;
            if (user == null)
            {
                _logger.LogError("Cant find user with this email: {0}", email);
                tokens = null;
                expDates = null;
                return false;
            }

            _unitOfWork.RoleRepo.ReadWhere(role => role.Id == user.RoleId).Load();
            var role = user.Role;
            if (role == null)
            {
                _logger.LogError("Cant find role with this id: {0}", user.RoleId);
                tokens = null;
                expDates = null;
                return false;
            }

            DateTime accessTokenExpDate;
            string accessToken = _tokenCreator.GenerateAccessToken(email, role.Title, out accessTokenExpDate);

            DateTime refreshTokenExpDate;
            string refreshToken = _tokenCreator.GenerateRefreshToken(out refreshTokenExpDate);

            userToken.AccessToken = accessToken;
            userToken.RefreshToken = refreshToken;
            userToken.RefreshTokenExpireDate = refreshTokenExpDate;

            _unitOfWork.TokenRepo.Update(userToken);
            _unitOfWork.Save();

            tokens = new Tuple<string, string>(accessToken, refreshToken);
            expDates = new Tuple<DateTime, DateTime>(accessTokenExpDate, refreshTokenExpDate);

            return true;
        }

        public bool UpdateTokensByRefreshToken(string email, string refreshToken, out Tuple<string, string>? tokens, out Tuple<DateTime, DateTime>? expDates)
        {
            var queryableUserToken = _unitOfWork.TokenRepo.ReadWhere(t => t.User.Email == email);
            _unitOfWork.UserRepo.ReadWhere(user => user.Email == email).Load();

            if (!queryableUserToken.Any())
            {
                _logger.LogError("Try update token on not existing user email: {0}", email);
                tokens = null;
                expDates = null;
                return false;
            }

            var userToken = queryableUserToken.First();

            if (userToken.RefreshToken != refreshToken)
            {
                _logger.LogError("Refresh token mismatching: {0} (in db) - {1} (given)", userToken.RefreshToken, refreshToken);
                tokens = null;
                expDates = null;
                return false;
            }

            if (userToken.RefreshTokenExpireDate < DateTime.Now)
            {
                _logger.LogError("Refresh token invalidated due to exp date: {0} (exp date) - {1} (now)", 
                    userToken.RefreshTokenExpireDate, DateTime.Now);
                tokens = null;
                expDates = null;
                return false;
            }

            _unitOfWork.UserRepo.ReadWhere(user => user.Email == email).Load();
            var user = userToken.User;
            if (user == null)
            {
                _logger.LogError("Cant find user with this email: {0}", email);
                tokens = null;
                expDates = null;
                return false;
            }

            _unitOfWork.RoleRepo.ReadWhere(role => role.Id == user.RoleId).Load();
            var role = user.Role;
            if (role == null)
            {
                _logger.LogError("Cant find role with this id: {0}", user.RoleId);
                tokens = null;
                expDates = null;
                return false;
            }

            DateTime newAccessTokenExpDate;
            string newAccessToken = _tokenCreator.GenerateAccessToken(userToken.User.Email, role.Title, out newAccessTokenExpDate);

            DateTime newRefreshTokenExpDate;
            string newRefreshToken = _tokenCreator.GenerateRefreshToken(out newRefreshTokenExpDate);

            userToken.AccessToken = newAccessToken;
            userToken.RefreshToken = newRefreshToken;
            userToken.RefreshTokenExpireDate = newRefreshTokenExpDate;

            _unitOfWork.TokenRepo.Update(userToken);
            _unitOfWork.Save();

            tokens = new Tuple<string, string>(newAccessToken, newRefreshToken);
            expDates = new Tuple<DateTime, DateTime>(newAccessTokenExpDate, newRefreshTokenExpDate);

            return true;
        }

        public bool IsRefreshTokenExpired(string email)
        {
            var queryableRefreshToken = _unitOfWork.TokenRepo.ReadWhere(t => t.User.Email == email);
            if (!queryableRefreshToken.Any())
            {
                _logger.LogError("User with this email was not found - email: {0}", email);
                return true;
            }

            var userToken = queryableRefreshToken.First();
            var refreshToken = userToken.RefreshToken;
            if (refreshToken == null)
            {
                _logger.LogError("Refresh token for this user email was not found - email: {0}", email);
                return true;
            }

            var refreshTokenExpDate = userToken.RefreshTokenExpireDate;
            if (refreshTokenExpDate == null)
            {
                _logger.LogError("Refresh token exp date is null");
                return true;
            }

            return refreshTokenExpDate < DateTime.Now;
        }
    }
}
