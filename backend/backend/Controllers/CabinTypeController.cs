using backend.DataAccess.Repository;
using backend.ServerResponse;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CabinTypeController : Controller
    {
        private IUnitOfWork _unit;

        public CabinTypeController(IUnitOfWork unit)
        {
            _unit = unit;
        }

        [HttpGet("Get")]
        public IActionResult Get()
        {
            return new StatusResponse(
                StatusResponseType.Success,
                "CabinTypesGotten",
                "Returned cabin types",
                _unit.CabintypeRepo.ReadAll().Select(c => c.Name)).
                ConvertToActionResult();
        }
    }
}
