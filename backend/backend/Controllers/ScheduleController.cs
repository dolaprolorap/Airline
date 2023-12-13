using Microsoft.AspNetCore.Mvc;
using backend.Models.API;
using backend.DataAccess.Repository;
using backend.ServerResponse;
using backend.ServerResponse.AuthService;
using Microsoft.EntityFrameworkCore;
using backend.Models.DB;
using backend.Services;
using Microsoft.AspNetCore.Authorization;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ScheduleController : ControllerBase
    {
        private IUnitOfWork _unit;
        private IScheduleService _schedule;

        public ScheduleController(IUnitOfWork unit, IScheduleService scheduleService) 
        {
            _unit = unit;
            _schedule = scheduleService;
        }

        [HttpGet("GetSchedule")]
        public IActionResult GetSchedule([FromQuery] ScheduleFilter scheduleFilter)
        {
            return _schedule.GetSchedule(scheduleFilter).ConvertToActionResult();
        }

        [HttpPost("SetActiveFlight")]
        public IActionResult SetActiveFlightFlight([FromBody] SetActiveFlight setActiveFlight)
        {
            return _schedule.SetActiveFlight(setActiveFlight).ConvertToActionResult();  
        }

        [HttpPost("EditFlight")]
        public IActionResult EditFlight([FromBody] EditFlight editFlight)
        {
            return _schedule.EditFlight(editFlight).ConvertToActionResult();    
        }

        [HttpPost("EditFlightsByCsv")]
        public IActionResult EditFlightsByCsv()
        {
            var csvFile = HttpContext.Request.Form.Files.First();

            if (csvFile == null)
                return new StatusResponse(StatusResponseType.UserFail, 
                    "There is no file", 
                    "There is no file").ConvertToActionResult();

            TextReader textReader = new StreamReader(csvFile.OpenReadStream());
            string csvStr = textReader.ReadToEnd();

            return _schedule.EditFlightsByCsv(csvStr).ConvertToActionResult();
        }
    }
}
