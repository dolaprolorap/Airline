﻿using Microsoft.AspNetCore.Mvc;
using backend.Models.API;
using backend.DataAccess.Repository;
using Microsoft.AspNetCore.Identity;
using User = backend.Models.DB.User;
using Token = backend.Models.DB.Token;
using backend.Services;
using System.Security.Claims;

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
    }
}