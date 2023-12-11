using backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("GetActivity")]
        public IActionResult GetActivity([FromQuery] string email)
        {
            return _trackerService.GetByEmail(email).ConvertToActionResult();
        }
    }
}
