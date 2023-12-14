using backend.Custom;
using backend.DataAccess.Repository;
using backend.Models.API;
using Microsoft.AspNetCore.Mvc;
using backend.ServerResponse.BookController;
using Route = backend.Models.DB.Route;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IUnitOfWork _unit;

        public BookController(IUnitOfWork unit) 
        { 
            _unit = unit;
        }

        [HttpPost("SearchFlights")]
        public IActionResult SearchFlights([FromBody] SearchFlightModel searchFlight)
        {
            var routeConstructor = new RouteConstructor(_unit);

            var fromAirport = _unit.AirportRepo.ReadFirst(airport => airport.Iatacode == searchFlight.FromCode);
            var toAirport = _unit.AirportRepo.ReadFirst(airport => airport.Iatacode == searchFlight.ToCode);

            if (fromAirport == null)
            {
                return new SearchFlightsResponse(
                    SearchFlightsResponseType.FromAirportCodeInvalid,
                    fromCode: searchFlight.FromCode).
                    ConvertToActionResult();
            }

            if (toAirport == null)
            {
                return new SearchFlightsResponse(
                    SearchFlightsResponseType.ToAirportCodeInvalid,
                    toCode: searchFlight.ToCode).
                    ConvertToActionResult();
            }

            IEnumerable<IEnumerable<Route>>? paths = new List<List<Route>>();
            var constructorResponse = routeConstructor.ConstructPath(
                fromAirport.Id,
                toAirport.Id,
                out paths);

            IEnumerable<ManyFlightsData> manyFlights = new List<ManyFlightsData>();

            //_unit.RouteRepo.ReadWhere(r => true).Load();

            foreach (var path in paths)
            {
                DateOnly lastDate;
                ManyFlightsData manyFlightsData = new ManyFlightsData();

                foreach (var (route, i) in path.Select((value, i) => (value, i)))
                {
                    if (i == 0)
                    {
                        if (!DateOnly.TryParseExact(searchFlight.OutboundDate, "dd-MM-yyyy", out lastDate))
                        {
                            return new SearchFlightsResponse(
                                SearchFlightsResponseType.FromDateInvalid,
                                searchFlight.OutboundDate).
                                ConvertToActionResult();
                        }

                        var schedule = _unit.ScheduleRepo.ReadFirst(
                            s => s.Date == lastDate &&
                            s.RouteId == route.Id);
                        
                        if (schedule == null)
                        {
                            break;
                        }

                        FlightData flightData = new FlightData()
                        {
                            FromCode = schedule.Route.DepartureAirport.Iatacode,
                            ToCode = schedule.Route.ArrivalAirport.Iatacode,
                            Date = schedule.Date.ToString(),
                            Time = schedule.Time.ToString(),
                            FlightNumber = schedule.FlightNumber,
                            EconomyPrice = (int)schedule.EconomyPrice,
                        };
                    }
                }
            }
        }

        [HttpPost("Book")]
        public IActionResult Book([FromBody] BookModel book)
        {
            return Ok();
        }
    }
}
