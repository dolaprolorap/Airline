using Microsoft.AspNetCore.Mvc;
using backend.Models.API;
using backend.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginModel loginData)
        {
            var status = _authService.Login(loginData);
            return status.ConvertToActionResult();
        }

        [HttpPost("Refresh")]    
        public IActionResult Refresh([FromBody] TokenPair tokenPair)
        {
            var status = _authService.Refresh(tokenPair);
            return status.ConvertToActionResult();
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] RegisterUser registerUser) 
        {
            var status = _authService.Register(registerUser);
            return status.ConvertToActionResult();
        }

        [HttpGet("GetMyself")]
        [Authorize]
        public IActionResult GetMyself()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return Unauthorized();
            var email = claimsIdentity.Name;
            if (email == null) return Unauthorized();
            return _authService.GetMyself(email).ConvertToActionResult();
        }

        [HttpPost("Exit")]
        [Authorize] 
        public IActionResult Exit()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return Unauthorized();
            var email = claimsIdentity.Name;
            if (email == null) return Unauthorized();

            return _authService.Exit(email).ConvertToActionResult();
        }
    }
}
