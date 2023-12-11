using backend.Models.API;
using Microsoft.AspNetCore.Mvc;
using backend.ServerResponse;

namespace backend.Services
{
    public interface IAuthService
    {
        public StatusResponse Login(LoginModel loginData);
        public StatusResponse Refresh(TokenPair tokenPair);
        public StatusResponse Register(RegisterUser registerUser);
    }
}
