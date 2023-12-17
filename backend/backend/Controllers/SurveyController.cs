using Microsoft.AspNetCore.Mvc;
using backend.DataAccess.Repository;
using backend.ServerResponse;
using backend.ServerResponse.Controllers.SurveyController;
using backend.Models.DB;
using backend.Custom;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveyController : Controller
    {
        private IUnitOfWork _unit;
        private SurveyCsvParser _surveyCsvParser;

        public SurveyController(IUnitOfWork unit)
        {
            _unit = unit;
            _surveyCsvParser = new SurveyCsvParser(unit);
        }

        [HttpGet("GetSummary")]
        public IActionResult GetSummary()
        {
            var surveys = _unit.SurveyRepo.ReadAll();
            return new GetSummaryResponse(
                GetSummaryResponseType.SummaryGotten,
                new
                {
                    SampleSize = surveys.Count(),
                    Male = surveys.Where(s => s.Sex == "M").Count(),
                    Female = surveys.Where(s => s.Sex == "F").Count(),
                    Age18_24 = surveys.Where(s => s.Age >= 18 && s.Age <= 24).Count(),
                    Age25_39 = surveys.Where(s => s.Age >= 25 && s.Age <= 39).Count(),
                    Age40_59 = surveys.Where(s => s.Age >= 40 && s.Age <= 59).Count(),
                    Age60 = surveys.Where(s => s.Age >= 60).Count(),
                    Economy = surveys.Where(s => s.CabinType == "Economy").Count(),
                    Business = surveys.Where(s => s.CabinType == "Business").Count(),
                    First = surveys.Where(s => s.CabinType == "First").Count(),
                    AUH = surveys.Where(s => s.ToAirport == "AUH").Count(),
                    ADE = surveys.Where(s => s.ToAirport == "ADE").Count(),
                    BAH = surveys.Where(s => s.ToAirport == "BAH").Count(),
                    DOH = surveys.Where(s => s.ToAirport == "DOH").Count(),
                    RUH = surveys.Where(s => s.ToAirport == "RUH").Count(),
                    CAI = surveys.Where(s => s.ToAirport == "CAI").Count(),
                }).
                ConvertToActionResult();
        }

        [HttpGet("GetDetailed")]
        public IActionResult GetDetailed()
        {
            var surveys = _unit.SurveyRepo.ReadAll();
            
            Dictionary<string, Func<Survey, int, bool>> categories = new Dictionary<string, Func<Survey, int, bool>>()
            {
                { "q1", (s, i) => s.Q1 == i },
                { "q2", (s, i) => s.Q2 == i },
                { "q3", (s, i) => s.Q3 == i },
                { "q4", (s, i) => s.Q4 == i }
            };
            List<int> ranks = new List<int>()
            {
                0,
                1,
                2,
                3,
                4,
                5,
                6
            };

            Dictionary<string, object> categoriesData = new Dictionary<string, object>();
            foreach(var category in categories)
            {
                Dictionary<string, object> rankData = new Dictionary<string, object>();
                foreach(var rank in ranks)
                {
                    var categorySurvey = surveys.Where(s => category.Value(s, rank));
                    rankData.Add(rank.ToString(), new
                    {
                        Total = categorySurvey.Count(),
                        Male = categorySurvey.Where(s => s.Sex == "M").Count(),
                        Female = categorySurvey.Where(s => s.Sex == "F").Count(),
                        Age18_24 = categorySurvey.Where(s => s.Age >= 18 && s.Age <= 24).Count(),
                        Age25_39 = categorySurvey.Where(s => s.Age >= 25 && s.Age <= 39).Count(),
                        Age40_59 = categorySurvey.Where(s => s.Age >= 40 && s.Age <= 59).Count(),
                        Age60 = categorySurvey.Where(s => s.Age >= 60).Count(),
                        Economy = categorySurvey.Where(s => s.CabinType == "Economy").Count(),
                        Business = categorySurvey.Where(s => s.CabinType == "Business").Count(),
                        First = categorySurvey.Where(s => s.CabinType == "First").Count(),
                        AUH = categorySurvey.Where(s => s.ToAirport == "AUH").Count(),
                        ADE = categorySurvey.Where(s => s.ToAirport == "ADE").Count(),
                        BAH = categorySurvey.Where(s => s.ToAirport == "BAH").Count(),
                        DOH = categorySurvey.Where(s => s.ToAirport == "DOH").Count(),
                        RUH = categorySurvey.Where(s => s.ToAirport == "RUH").Count(),
                        CAI = categorySurvey.Where(s => s.ToAirport == "CAI").Count(),
                    });
                }
                categoriesData.Add(category.Key, rankData);
            }

            return new GetDetailedResponse(
                GetDetailedResponseType.DetailedGotten,
                data: categoriesData).
                ConvertToActionResult();
        }

        [HttpPost("Load")]
        public IActionResult Load() 
        {
            IFormFile? csvFile;

            try
            {
                csvFile = HttpContext.Request.Form.Files.First();
            }
            catch (Exception ex)
            {
                return new LoadResponse(
                    LoadResponseType.FailedGettingFile, 
                    excMsg: ex.Message).ConvertToActionResult();
            }

            if (csvFile == null)
                return new LoadResponse(LoadResponseType.NoFile)
                    .ConvertToActionResult();

            TextReader textReader = new StreamReader(csvFile.OpenReadStream());
            string csvStr = textReader.ReadToEnd();

            List<Survey> surveys;
            if(_surveyCsvParser.ParseAndValidateManySurvey(csvStr, out surveys)) 
            {
                foreach(var survey in surveys)
                {
                    _unit.SurveyRepo.Add(survey);
                }
            }
            else
            {
                return new LoadResponse(LoadResponseType.InvalidFormat).
                    ConvertToActionResult();
            }

            _unit.Save();
            return new LoadResponse(LoadResponseType.Loaded).
                ConvertToActionResult();
        }
    }
}
