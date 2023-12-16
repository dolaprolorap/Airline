using backend.Models.API;
using Microsoft.AspNetCore.Mvc;
using backend.ServerResponse;
using backend.ServerResponse.Services.AuthService;

namespace backend.Services
{
    public interface IAuthService
    {
        public StatusResponse Login(LoginModel loginData);
        public StatusResponse Refresh(TokenPair tokenPair);
        public StatusResponse Register(RegisterUser registerUser);
        public GetMyselfResponse GetMyself(string email);
    }
}
