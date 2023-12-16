using backend.DataAccess.Repository;
using backend.ServerResponse;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportController : Controller
    {
        private IUnitOfWork _unit;

        public AirportController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpGet("Get")]
        public IActionResult Get()
        {
            return new StatusResponse(
                StatusResponseType.Success,
                "AirportsGotten",
                "Returned airports",
                _unit.AirportRepo.ReadAll().Select(a => new
                {
                    Country = _unit.CountryRepo.ReadFirst(c => c.Id == a.CountryId)!.Name,
                    IATACode = a.Iatacode,
                    Name = a.Name
                })).
                ConvertToActionResult();
        }
    }
}
