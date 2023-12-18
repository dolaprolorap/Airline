using backend.DataAccess.Repository;
using backend.ServerResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : Controller
    {
        private IUnitOfWork _unit;

        public TicketController(IUnitOfWork unit) 
        {
            _unit = unit;
        }

        [HttpGet("Ticket")]
        public IActionResult Ticket([FromQuery] string? ticketId)
        {
            if (ticketId == null)
            {
                return new StatusResponse(
                    StatusResponseType.UserFail,
                    "NoTicketId",
                    "Theres is no ticket id - ticketId is null").
                    ConvertToActionResult();
            }

            if (!int.TryParse(ticketId, out int ticketIdInt))
            {
                return new StatusResponse(
                    StatusResponseType.UserFail,
                    "TicketIdIsNotInt",
                    "Ticket id is not int: " + ticketId).
                    ConvertToActionResult();
            }

            var ticket = _unit.TicketRepo.ReadFirst(t => t.Id == ticketIdInt);

            if (ticket == null)
            {
                return new StatusResponse(
                    StatusResponseType.UserFail,
                    "TicketNotFound",
                    "Ticket not found with id: " + ticketIdInt).
                    ConvertToActionResult();
            }

            _unit.CabintypeRepo.ReadWhere(c => true).Load();
            var country = _unit.CountryRepo.ReadAll();

            var data = new
            {
                ticket.Id,
                ticket.UserId,
                ticket.ScheduleId,
                CabinType = ticket.CabinType.Name,
                ticket.Firstname,
                ticket.Lastname,
                ticket.Phone,
                ticket.PassportNumber,
                PassportCountry = country.Where(c => c.Id == ticket.PassportCountryId).First().Name,
                ticket.BookingReference,
                ticket.Confirmed
            };

            return new StatusResponse(
                StatusResponseType.Success,
                "TicketGotten",
                "Ticket gotten with id: " + ticketId,
                data).
                ConvertToActionResult();
        }
    }
}
