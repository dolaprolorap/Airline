using backend.DataAccess.Repository;
using backend.ServerResponse;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : Controller
    {
        private IUnitOfWork _unit;

        public RoleController(IUnitOfWork unit) 
        {
            _unit = unit;
        }

        [HttpGet("Get")]
        public IActionResult Get()
        {
            return new StatusResponse(
                StatusResponseType.Success,
                "RolesGotten",
                "Returned roles",
                _unit.RoleRepo.ReadAll().Select(r => r.Title)).
                ConvertToActionResult();
        }
    }
}
