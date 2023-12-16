using backend.Custom;
using backend.DataAccess.Repository;
using backend.Models.API;
using Microsoft.AspNetCore.Mvc;
using Route = backend.Models.DB.Route;
using Microsoft.EntityFrameworkCore;
using backend.Models.DB;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using BookRequest = backend.ServerRequest.Custom.BookingManager.BookRequest;
using backend.ServerResponse.Controllers.BookController;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private IUnitOfWork _unit;
        private BookingManager _bookingManager;

        public BookController(IUnitOfWork unit) 
        { 
            _unit = unit;
            _bookingManager = new BookingManager(_unit);
        }

        [HttpPost("SearchFlights")]
        public IActionResult SearchFlights([FromBody] SearchFlightModel searchFlight)
        {
            var forwardFlightsStatus = _bookingManager.CreateManyFlights(searchFlight);

            if (forwardFlightsStatus.ResponseType !=
                ServerResponse.Custom.BookManager.CreateManyFlightsResponseType.Ok)
                return new SearchFlightsResponse(
                    SearchFlightsResponseType.FailedCreateManyFlights).
                    ConvertToActionResult();

            if (searchFlight.WithReturn)
            {
                var returnSearchFlight = new SearchFlightModel()
                {
                    FromCode = searchFlight.ToCode,
                    ToCode = searchFlight.FromCode,
                    CabinType = searchFlight.CabinType,
                    WithReturn = false,
                    OutboundDate = searchFlight.ReturnDate,
                    ReturnDate = searchFlight.ReturnDate
                };

                var returnFlightsStatus = _bookingManager.CreateManyFlights(returnSearchFlight);

                if (returnFlightsStatus.ResponseType !=
                    ServerResponse.Custom.BookManager.CreateManyFlightsResponseType.Ok)
                    return new SearchFlightsResponse(
                        SearchFlightsResponseType.FailedCreateManyFlights).
                        ConvertToActionResult();

                return new SearchFlightsResponse(
                    SearchFlightsResponseType.Ok,
                    forwardManyFlights: forwardFlightsStatus.ManyFlights,
                    returnManyFlights: returnFlightsStatus.ManyFlights).
                    ConvertToActionResult();
            }

            return new SearchFlightsResponse(
                SearchFlightsResponseType.Ok,
                forwardManyFlights: forwardFlightsStatus.ManyFlights).
                ConvertToActionResult();
        }

        [HttpPost("Book")]
        public IActionResult Book([FromBody] BookData book)
        {
            var claimsIdentity = HttpContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity == null) return Unauthorized();
            var email = claimsIdentity.Name;
            if (email == null) return Unauthorized();

            var user = _unit.UserRepo.ReadFirst(u => u.Email == email);
            if (user == null) return Unauthorized();

            return _bookingManager.Book(new BookRequest
            {
                UserId = user.Id,
                BookData = book
            }).ConvertToActionResult();
        }
    }
}
