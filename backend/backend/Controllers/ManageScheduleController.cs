using Microsoft.AspNetCore.Mvc;
using backend.Models.API;
using backend.DataAccess.Repository;
using backend.ServerResponse;
using backend.ServerResponse.AuthService;
using Microsoft.EntityFrameworkCore;
using backend.Models.DB;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageScheduleController : ControllerBase
    {
        private IUnitOfWork _unit;

        public ManageScheduleController(IUnitOfWork unit) 
        {
            _unit = unit;
        }

        [HttpGet("GetSchedule")]
        public IActionResult GetSchedule([FromQuery] ScheduleFilter scheduleFilter)
        {
            Airport? toAirport = null;
            Airport? fromAirport = null;

            if (scheduleFilter.FromAirportName != null)
            {
                fromAirport = _unit.AirportRepo.ReadFirst(airport => airport.Name == scheduleFilter.FromAirportName);

                if (fromAirport == null)
                {
                    return new StatusResponse(StatusResponseType.UserFail,
                        "There is no From Airport with that name: " + scheduleFilter.FromAirportName,
                        "There is no From Airport with that name: " + scheduleFilter.FromAirportName).
                        ConvertToActionResult();
                }
            }

            if (scheduleFilter.ToAirportName != null)
            {
                toAirport = _unit.AirportRepo.ReadFirst(airport => airport.Name == scheduleFilter.ToAirportName);

                if (toAirport == null)
                {
                    return new StatusResponse(StatusResponseType.UserFail,
                        "There is no To Airport with that name: " + scheduleFilter.ToAirportName,
                        "There is no To Airport with that name: " + scheduleFilter.ToAirportName).
                        ConvertToActionResult();
                }
            }     

            var routes = _unit.RouteRepo.ReadWhere(
                route => 
                (fromAirport == null || route.DepartureAirportId == fromAirport.Id) && 
                (toAirport == null || route.ArrivalAirportId == toAirport.Id)
            );
            if (!routes.Any())
            {
                return new StatusResponse(StatusResponseType.Success, "", "", new List<ScheduleRecord>()).
                    ConvertToActionResult();
            }
            routes.Load();
            var routesIds = routes.Select(route => route.Id).ToList();

            DateOnly? outbound = null;
            if (scheduleFilter.Outbound != null)
            {
                try
                {
                    outbound = DateOnly.ParseExact(scheduleFilter.Outbound, "dd-MM-yyyy",
                        System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    return new StatusResponse(StatusResponseType.UserFail, 
                        "Invalid DateOnly: " + scheduleFilter.Outbound,
                        "Invalid DateOnly: " + scheduleFilter.Outbound).
                        ConvertToActionResult();
                }
            }

            var flights = _unit.ScheduleRepo.ReadWhere(
                flight => 
                (outbound == null || flight.Date == outbound) && 
                routesIds.Contains(flight.Id)
            );
            
            List<ScheduleRecord> scheduleRecords = new List<ScheduleRecord>();
            foreach (var flight in flights)
            {
                scheduleRecords.Add(new ScheduleRecord()
                {
                    Date = flight.Date,
                    Time = flight.Time,
                    FromAirportName = flight.Route.DepartureAirport.Name!,
                    ToAirportName = flight.Route.ArrivalAirport.Name!,
                    FlightNumber = flight.FlightNumber!,
                    Aircraft = flight.Aircraft.Name,
                    EconomyPrice = (int)flight.EconomyPrice,
                    IsActive = flight.Confirmed
                });
            }

            return new StatusResponse(StatusResponseType.Success, "", "", scheduleFilter).ConvertToActionResult();
        }

        [HttpPost("SetActiveFlight")]
        public IActionResult SetActiveFlightFlight([FromBody] SetActiveFlight setActiveFlight)
        {
            var flight = _unit.ScheduleRepo.ReadFirst(s => s.Id == setActiveFlight.FlightId);
            if (flight == null)
                return new StatusResponse(StatusResponseType.UserFail,
                    "There is no schedule with that id: " + setActiveFlight.FlightId,
                    "There is no schedule with that id: " + setActiveFlight.FlightId).
                    ConvertToActionResult();

            flight.Confirmed = setActiveFlight.ActiveState;

            _unit.ScheduleRepo.Update(flight);
            _unit.Save();

            return new StatusResponse(StatusResponseType.Success).ConvertToActionResult();
        }

        [HttpPost("EditFlight")]
        public IActionResult EditFlight([FromBody] EditFlight editFlight)
        {
            var flight = _unit.ScheduleRepo.ReadFirst(s => s.Id == editFlight.Id);
            if (flight == null)
            {
                return new StatusResponse(StatusResponseType.UserFail,
                    "There is no flight with id: " + editFlight,
                    "There is no flight with id: " + editFlight).ConvertToActionResult();
            }

            flight.Date = editFlight.Date;
            flight.Time = editFlight.Time;
            flight.EconomyPrice = editFlight.EconomyPrice;

            _unit.ScheduleRepo.Update(flight);
            _unit.Save();

            return new StatusResponse(StatusResponseType.Success).ConvertToActionResult();
        }

        [HttpPost("EditFlights")]
        public IActionResult EditFlights([FromBody] EditFlight[] flights)
        {
            return Ok();
        }
    }
}
