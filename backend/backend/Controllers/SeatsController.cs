using backend.DataAccess.Repository;
using backend.Models.DB;
using backend.ServerRequest.Controllers.SeatsController;
using backend.ServerResponse;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeatsController : Controller
    {
        private IUnitOfWork _unit;

        public SeatsController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpGet("Seats")]
        public IActionResult Seats([FromQuery] int ticketId)
        {
            var ticket = _unit.TicketRepo.ReadFirst(t => t.Id == ticketId);
            if (ticket == null)
            {
                return new StatusResponse(
                    StatusResponseType.UserFail,
                    "TicketNotFound",
                    "Ticket with id not found: " + ticketId).
                    ConvertToActionResult();
            }

            var seats = _unit.SeatRepo.ReadWhere(s => s.ScheduleId == ticket.ScheduleId).ToList();
            return new StatusResponse(
                StatusResponseType.Success,
                "SeatsGotten",
                "Seats returned",
                seats.Select(s => s.SeatNumber)).
                ConvertToActionResult();
        }

        [HttpPost("BookSeat")]
        public IActionResult BookSeat([FromBody] BookSeatRequest bookSeat)
        {
            var ticket = _unit.TicketRepo.ReadFirst(t => t.Id == bookSeat.TicketId);
            if (ticket == null)
            {
                return new StatusResponse(
                    StatusResponseType.UserFail,
                    "TicketNotFound",
                    "Ticket with id not found: " + bookSeat.TicketId).
                    ConvertToActionResult();
            }


            var schedule = _unit.ScheduleRepo.ReadFirst(s => s.Id == ticket.ScheduleId);

            if (_unit.SeatRepo.ReadFirst(s =>
                s.ScheduleId == schedule.Id &&
                s.SeatNumber == bookSeat.SeatNumber) != null)
            {
                return new StatusResponse(
                    StatusResponseType.Success,
                    "SeatBooked",
                    "Seat with booked").
                    ConvertToActionResult();
            }

            var seats = _unit.SeatRepo.ReadWhere(s => true);

            int lastId = 0;
            if (seats.Any()) lastId = seats.Max(s => s.Id);

            Seat seat = new Seat()
            {
                Id = ++lastId,
                ScheduleId = schedule.Id,
                SeatNumber = bookSeat.SeatNumber
            };

            _unit.SeatRepo.Add(seat);
            _unit.Save();

            return new StatusResponse(
                StatusResponseType.Success,
                "SeatBooked",
                "Seat with booked").
                ConvertToActionResult();
        }
    }
}
