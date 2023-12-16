using backend.DataAccess.Repository;
using backend.ServerResponse;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : Controller
    {
        private IUnitOfWork _unit;

        public CountryController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpGet("Get")]
        public IActionResult Get()
        {
            return new StatusResponse(
                StatusResponseType.Success,
                "CountriesGotten",
                "Returned countries",
                _unit.CountryRepo.ReadAll().Select(c => c.Name)).
                ConvertToActionResult();
        }
    }
}
