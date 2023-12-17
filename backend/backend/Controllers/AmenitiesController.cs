using backend.DataAccess.Repository;
using backend.ServerResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenitiesController : Controller
    {
        private IUnitOfWork _unit;

        public AmenitiesController(IUnitOfWork unit) 
        {
            _unit = unit;
        }

        [HttpGet("GetTicketsByBookRef")]
        public IActionResult GetTicketsByBookRef([FromQuery] string? bookRef)
        {
            if (bookRef == null)
            {
                return new StatusResponse(
                    StatusResponseType.UserFail,
                    "NoBookRef",
                    "There is no book ref").
                    ConvertToActionResult();
            }

            var tickets = _unit.TicketRepo.ReadWhere(t => t.BookingReference == bookRef);

            var scheduleIds = tickets.Select(t => t.ScheduleId).ToList();
            _unit.ScheduleRepo.ReadWhere(s => scheduleIds.Contains(s.Id)).Load();

            var schedules = tickets.Select(t => t.Schedule).ToList();
            var routesIds = schedules.Select(s => s.RouteId).ToList();
            _unit.RouteRepo.ReadWhere(r => routesIds.Contains(r.Id)).Load();

            _unit.AirportRepo.ReadWhere(a => true).Load();
            return Ok();
            return new StatusResponse(
                StatusResponseType.Success,
                "TicketsGotten",
                "Tickets with book ref have been returned",
                tickets.Select(t => new
                {
                    t.Id,
                    t.Schedule.FlightNumber,
                    
                })).
                ConvertToActionResult();
        }

        [HttpGet("GetPersonalDataByTicket")]
        public IActionResult GetPersonalDataByTicket() 
        {
            return Ok();
        }

        [HttpGet("GetAmenities")]
        public IActionResult GetAmenities()
        {
            return Ok();
        }

        [HttpPost("ApplyAmenities")]
        public IActionResult ApplyAmenities()
        {
            return Ok();
        }

        [HttpPost("MakeReport")]
        public IActionResult MakeReport()
        {
            return Ok();
        }

        [HttpPost("AmonicSummary")]
        public IActionResult AmonicSummary()
        {
            return Ok();
        }
    }
}
