using backend.DataAccess.Repository;
using backend.Models.DB;
using backend.ServerRequest.Controllers.AmenitiesController;
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
            _unit.CabintypeRepo.ReadWhere(c => true).Load();

            return new StatusResponse(
                StatusResponseType.Success,
                "TicketsGotten",
                "Tickets with book ref have been returned",
                tickets.Select(t => new
                {
                    t.Id,
                    t.Schedule.FlightNumber,
                    FromAirportName = t.Schedule.Route.DepartureAirport.Name,
                    ToAirportName = t.Schedule.Route.ArrivalAirport.Name,
                    Date = t.Schedule.Date.ToString("yyyy-MM-dd"),
                    Time = t.Schedule.Time.ToString("HH:mm"),
                    t.Firstname,
                    t.Lastname,
                    t.Phone,
                    CabinTypeName = t.CabinType.Name
                })).
                ConvertToActionResult();
        }

        [HttpGet("GetAmenities")]
        public IActionResult GetAmenities()
        {
            var amenities = _unit.AmenityRepo.ReadAll();
            return new StatusResponse(
                StatusResponseType.Success,
                "AmenitiesGotten",
                "Amenities returned",
                amenities.Select(a => new
                {
                    a.Id,
                    a.Service,
                    a.Price
                })).
                ConvertToActionResult();
        }

        [HttpPost("ApplyAmenities")]
        public IActionResult ApplyAmenities([FromBody] ApplyAmenitiesRequest applyAmenities)
        {
            var ticket = _unit.TicketRepo.ReadFirst(t => t.Id == applyAmenities.TicketId);
            if (ticket == null)
            {
                return new StatusResponse(
                    StatusResponseType.UserFail,
                    "TicketNotFound",
                    "There is no ticket with id: " + applyAmenities.TicketId).
                    ConvertToActionResult();
            }

            var amenities = _unit.AmenityRepo.
                ReadWhere(a => applyAmenities.AmenityNamesToApply.Contains(a.Service)).ToList();
            if (!amenities.Any())
            {
                return new StatusResponse(
                    StatusResponseType.UserFail,
                    "AmenityNotFound",
                    "There is no amenities with specified name").
                    ConvertToActionResult();
            }

            foreach(var amenityName in applyAmenities.AmenityNamesToApply)
            {
                var queryableAmenity = amenities.Where(a => a.Service == amenityName);
                if (!queryableAmenity.Any())
                {
                     return new StatusResponse(
                         StatusResponseType.UserFail,
                         "AmenityNotFound",
                         "There is no amenity with name: " + amenityName).
                         ConvertToActionResult();
                }

                Amenity amentity = queryableAmenity.First();

                _unit.AmenitiesticketRepo.Add(new Amenitiesticket()
                {
                    AmenityId = amentity.Id,
                    TicketId = applyAmenities.TicketId,
                    Price = amentity.Price
                });
            }

            _unit.Save();

            return new StatusResponse(
                StatusResponseType.Success,
                "AmenitiesAdded",
                "AmenitiesAdded to ticket: " + applyAmenities.TicketId).
                ConvertToActionResult();
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
