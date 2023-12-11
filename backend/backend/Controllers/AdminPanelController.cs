using Microsoft.AspNetCore.Mvc;
using backend.Models.API;
using backend.DataAccess.Repository;
using Microsoft.AspNetCore.Authorization;
using backend.Services;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // BIG KOSTIL
    [Authorize]
    public class AdminPanelController : ControllerBase
    {
        private IUnitOfWork _unit;
        private ITokenCreatorService _tokenCreatorService;
        private IAuthService _authService;

        public AdminPanelController(IUnitOfWork unit, ITokenCreatorService tokenCreator, IAuthService authService) 
        {
            _unit = unit;
            _tokenCreatorService = tokenCreator;
            _authService = authService;
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody] RegisterUser registerUser)
        {
            return _authService.Register(registerUser).ConvertToActionResult();
        }

        [HttpGet("Users")]
        public IActionResult GetUsersByOfficeId([FromQuery] int? officeId) 
        {
            if (officeId == null)
            {
                return Ok(_unit.UserRepo.ReadAll().ToList());
            }
            return Ok(_unit.UserRepo.ReadWhere(user => user.OfficeId == officeId).ToList());
        }

        [HttpPost("SetActiveUser")]
        public IActionResult SetAcitveUser([FromBody] SetActiveUserModel setActiveUser)
        {
            var queryableUser = _unit.UserRepo.ReadWhere(user => user.Id == setActiveUser.Id);
            if (!queryableUser.Any())
            {
                return BadRequest("User with that id was not found");
            }

            var user = queryableUser.First();
            user.Active = setActiveUser.ActiveState;

            _unit.UserRepo.Update(user);
            _unit.Save();

            return Ok();
        }

        [HttpPost("ChangeRole")]
        public IActionResult ChangeRole([FromBody] ChangeRoleModel changeRole)
        {
            var queryableUser = _unit.UserRepo.ReadWhere(user => user.Id == changeRole.Id);
            if (!queryableUser.Any())
            {
                return BadRequest("User with that id was not found");
            }

            var user = queryableUser.First();

            var queryableRole = _unit.RoleRepo.ReadWhere(role => role.Id == changeRole.RoleId);
            if (!queryableRole.Any())
            {
                return BadRequest("Role with that id was not found");
            }

            user.RoleId = changeRole.RoleId;

            _unit.UserRepo.Update(user);
            _unit.Save();

            return Ok();
        }
    }
}
