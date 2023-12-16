using backend.DataAccess.Repository;
using backend.ServerResponse;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeController : Controller
    {
        private IUnitOfWork _unit;

        public OfficeController(IUnitOfWork unit) 
        { 
            _unit = unit;
        }

        [HttpGet("Get")]
        public IActionResult Get()
        {
            return new StatusResponse(
                StatusResponseType.Success,
                "OfficesGotten",
                "Returned offices",
                _unit.OfficeRepo.ReadAll().Select(o => new
                {
                    Country = _unit.CountryRepo.ReadFirst(c => c.Id == o.CountryId)!.Name,
                    Title = o.Title,
                    Phone = o.Phone,
                    Contact = o.Contact
                })).
                ConvertToActionResult();
        }
    }
}
