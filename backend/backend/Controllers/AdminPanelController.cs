﻿using Microsoft.AspNetCore.Mvc;
using backend.Models.API;
using backend.DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using backend.Services;
using backend.ServerResponse.Controllers.AdminPanelController;
using backend.Models.DB;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // BIG KOSTIL
    [Authorize]
    public class AdminPanelController : ControllerBase
    {
        private IUnitOfWork _unit;
        private IAuthService _authService;

        public AdminPanelController(IUnitOfWork unit, ITokenCreatorService tokenCreator, IAuthService authService) 
        {
            _unit = unit;
            _authService = authService;
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody] RegisterUser registerUser)
        {
            return _authService.Register(registerUser).ConvertToActionResult();
        }

        [HttpGet("Users")]
        public IActionResult GetUsersByOfficeId([FromQuery] string? officeName) 
        {
            bool officeSpecified = officeName != null;
            IEnumerable<User> users;

            if (officeSpecified)
            {
                var office = _unit.OfficeRepo.ReadFirst(o => o.Title == officeName);
                if (office == null)
                {
                    return new GetUsersByOfficeIdResponse(
                        GetUsersByOfficeIdResponseType.OfficeNotFound,
                        officeSpecified,
                        officeName: officeName).
                        ConvertToActionResult();
                }

                users = _unit.UserRepo.ReadWhere(user => user.OfficeId == office.Id);
            }
            else
            {
                users = _unit.UserRepo.ReadAll();
            }

            _unit.OfficeRepo.ReadWhere(o => true).Load();
            _unit.RoleRepo.ReadWhere(r => true).Load();

            return new GetUsersByOfficeIdResponse(
                GetUsersByOfficeIdResponseType.UsersGotten,
                officeSpecified,
                officeName: officeName,
                foundUsers: users.Select(u => new
                {
                    u.Id,
                    OfficeName = u.Office!.Title,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    RoleName = u.Role!.Title,
                    u.Birthdate,
                    u.Active
                })).
                ConvertToActionResult();
        }

        [HttpPost("SetActiveUser")]
        public IActionResult SetActiveUser([FromBody] SetActiveUserModel setActiveUser)
        {
            var user = _unit.UserRepo.ReadFirst(user => user.Email == setActiveUser.Email);
            if (user == null)
            {
                return new SetActiveUserResponse(
                    SetActiveUserResponseType.UserNotFound, 
                    email: setActiveUser.Email).
                    ConvertToActionResult();
            }

            user.Active = setActiveUser.ActiveState;

            _unit.UserRepo.Update(user);
            _unit.Save();

            return new SetActiveUserResponse(
                SetActiveUserResponseType.ActiveStateChanged,
                email: setActiveUser.Email).
                ConvertToActionResult();
        }

        [HttpPost("ChangeRole")]
        public IActionResult ChangeRole([FromBody] ChangeRoleModel changeRole)
        {
            var user = _unit.UserRepo.ReadFirst(user => user.Email == changeRole.UserEmail);
            if (user == null)
            {
                return new ChangeRoleResponse(
                    ChangeRoleResponseType.UserNotFound,
                    email: changeRole.UserEmail 
                    ).ConvertToActionResult();
            }

            var role = _unit.RoleRepo.ReadFirst(role => role.Title == changeRole.RoleName);
            if (role == null)
            {
                return new ChangeRoleResponse(
                    ChangeRoleResponseType.RoleNotFound,
                    roleName: changeRole.RoleName
                    ).ConvertToActionResult();
            }

            user.RoleId = role.Id;

            _unit.UserRepo.Update(user);
            _unit.Save();

            return new ChangeRoleResponse(
                ChangeRoleResponseType.RoleChanged,
                email: changeRole.UserEmail
                ).ConvertToActionResult();
        }
    }
}
