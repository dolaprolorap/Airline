using backend.Models.API;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookFlightCotroller : ControllerBase
    {
        [HttpPost("SearchFlights")]
        public IActionResult SearchFlights([FromBody] SearchFlightModel searchFlight)
        {
            return Ok();
        }

        [HttpPost("Book")]
        public IActionResult Book([FromBody] BookModel book)
        {
            return Ok();
        }
    }
}
