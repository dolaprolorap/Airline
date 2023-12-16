using backend.DataAccess.Repository;
using backend.Models.API;
using backend.Models.DB;
using backend.ServerRequest.Custom.BookingManager;
using backend.ServerResponse.Controllers.BookController;
using backend.ServerResponse.Custom.BookManager;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Sockets;
using Route = backend.Models.DB.Route;

namespace backend.Custom
{
    public class BookingManager
    {
        private IUnitOfWork _unit;

        public BookingManager(IUnitOfWork unit) 
        { 
            _unit = unit;
        }

        public CreateManyFlightsResponse CreateManyFlights(SearchFlightModel searchFlight)
        {
            var routeConstructor = new RouteConstructor(_unit);

            var fromAirport = _unit.AirportRepo.ReadFirst(airport => airport.Iatacode == searchFlight.FromCode);
            var toAirport = _unit.AirportRepo.ReadFirst(airport => airport.Iatacode == searchFlight.ToCode);

            if (fromAirport == null)
            {
                return new CreateManyFlightsResponse(
                    CreateManyFlightsResponseType.FromAirportCodeInvalid,
                    fromCode: searchFlight.FromCode);
            }

            if (toAirport == null)
            {
                return new CreateManyFlightsResponse(
                    CreateManyFlightsResponseType.ToAirportCodeInvalid,
                    toCode: searchFlight.ToCode);
            }

            List<List<Route>>? paths = new List<List<Route>>();
            var constructorResponse = routeConstructor.ConstructPath(
                fromAirport.Id,
                toAirport.Id,
                out paths);

            List<ManyFlightsData> manyFlights = new List<ManyFlightsData>();

            _unit.AirportRepo.ReadWhere(r => true).Load();

            foreach (var path in paths)
            {
                ManyFlightsData manyFlightsData = new ManyFlightsData();

                if (!DateOnly.TryParseExact(searchFlight.OutboundDate, "yyyy-MM-dd", out DateOnly lastDate))
                {
                    return new CreateManyFlightsResponse(
                        CreateManyFlightsResponseType.FromDateInvalid,
                        searchFlight.OutboundDate);
                }

                foreach (var (route, i) in path.Select((value, i) => (value, i)))
                {
                    Schedule? schedule = null;

                    if (i == 0)
                    {
                        schedule = _unit.ScheduleRepo.ReadFirst(
                            s => s.Date == lastDate &&
                            s.RouteId == route.Id);
                    }
                    else
                    {
                        schedule = _unit.ScheduleRepo.ReadFirst(
                            s => s.Date >= lastDate &&
                            s.RouteId == route.Id);
                    }

                    if (schedule == null) break;

                    FlightData flightData = new FlightData()
                    {
                        Id = schedule.Id,
                        FromCode = schedule.Route.DepartureAirport.Iatacode,
                        ToCode = schedule.Route.ArrivalAirport.Iatacode,
                        Date = schedule.Date.ToString(),
                        Time = schedule.Time.ToString(),
                        FlightNumber = schedule.FlightNumber!,
                        EconomyPrice = (int)schedule.EconomyPrice,
                    };
                    manyFlightsData.Flights.Add(flightData);
                }

                manyFlights.Add(manyFlightsData);
            }

            return new CreateManyFlightsResponse(
                CreateManyFlightsResponseType.Ok,
                manyFlights: manyFlights);
        }

        public BookResponse Book(BookRequest bookRequest)
        {
            string bookingRef = _createBookingRef();
            var ticketDatas = new List<TicketData>();

            foreach (var scheduleId in bookRequest.BookData.ScheduleIds)
            {
                foreach (var passenger in bookRequest.BookData.Passengers)
                {

                    ticketDatas.Add(new TicketData()
                    {
                        ScheduleId = scheduleId,
                        Passenger = passenger,
                        BookingRef = bookingRef,
                        CabinType = bookRequest.BookData.CabinType
                    });
                }
            }

            var user = _unit.UserRepo.ReadFirst(u => u.Id == bookRequest.UserId);
            if (user == null)
            {
                return new BookResponse(
                    BookResponseType.UserNotFound,
                    userId: bookRequest.UserId.ToString()
                    );
            }

            var cabinType = _unit.CabintypeRepo.ReadFirst(c => c.Name == bookRequest.BookData.CabinType);
            if (cabinType == null)
            {
                return new BookResponse(
                    BookResponseType.CabinNameNotFound,
                    cabinName: bookRequest.BookData.CabinType);
            }

            int lastId = _unit.TicketRepo.Max(ticket => ticket.Id);

            foreach (var ticket in ticketDatas)
            {          
                var schedule = _unit.ScheduleRepo.ReadFirst(s => s.Id == ticket.ScheduleId); 
                if (schedule == null)
                {
                    return new BookResponse(
                        BookResponseType.ScheduleNotFound,
                        scheduleId: ticket.ScheduleId.ToString());
                }

                var passportCountry = _unit.CountryRepo.ReadFirst(c => c.Name == ticket.Passenger.CountryName);
                if (passportCountry == null)
                {
                    return new BookResponse(
                        BookResponseType.PassportCountryNotFound,
                        countryName: ticket.Passenger.CountryName);
                }

                _unit.TicketRepo.Add(new Ticket()
                {
                    Id = ++lastId,
                    UserId = user.Id,
                    ScheduleId = schedule.Id,
                    CabinTypeId = cabinType.Id,
                    Firstname = ticket.Passenger.Firstname,
                    Lastname = ticket.Passenger.Lastname,
                    Phone = ticket.Passenger.Phone,
                    PassportNumber = ticket.Passenger.PassportNumber,
                    PassportCountryId = passportCountry.Id,
                    BookingReference = bookingRef,
                    Confirmed = true
                });
            }

            _unit.Save();

            return new BookResponse(BookResponseType.Ok);
        }

        private string _createBookingRef()
        {
            string bookingRef = "";
            do
            {
                bookingRef = _randomString(6);
            }
            while (_unit.TicketRepo.ReadWhere(t => t.BookingReference == bookingRef).Any());
            return bookingRef;
        }

        private string _randomString(int length)
        {
            var rand = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[rand.Next(s.Length)]).ToArray());
        }
    }
}
