using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserPanelController : ControllerBase
    {
        private ITrackerService _trackerService;

        public UserPanelController(ITrackerService trackerService) 
        {
            _trackerService = trackerService;
        }

        [HttpGet("GetCurrentUserActivity")]
        public IActionResult GetActivity()
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return Unauthorized();
            var email = claimsIdentity.Name;
            if (email == null) return Unauthorized();
            return _trackerService.GetByEmail(email).ConvertToActionResult();
        }
    }
}
