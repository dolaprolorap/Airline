using Microsoft.AspNetCore.Mvc;
using backend.Models.API;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageSchedule : ControllerBase
    {
        [HttpPost("GetSchedule")]
        public IActionResult GetSchedule([FromBody] FlightsScheduleModel flightsSchedule)
        {
            return Ok();
        }

        [HttpPost("CancelFlight")]
        public IActionResult CancelFlight([FromBody] string flightId)
        {
            return Ok();
        }

        [HttpPost("EditFlight")]
        public IActionResult EditFlight([FromBody] FlightModel flight)
        {
            return Ok();
        }

        [HttpPost("EditFlights")]
        public IActionResult EditFlights([FromBody] FlightModel[] flights)
        {
            return Ok();
        }
    }
}
