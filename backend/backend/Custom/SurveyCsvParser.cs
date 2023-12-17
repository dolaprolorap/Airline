using backend.DataAccess.Repository;
using backend.Models.DB;
using backend.Models.API;

namespace backend.Custom
{
    public class SurveyCsvParser
    {
        private IUnitOfWork _unit;

        private const string linesDelim = "\r\n";
        private const string wordsDelim = ",";

        private const int recordsCount = 9;

        public SurveyCsvParser(IUnitOfWork unit) 
        { 
            _unit = unit;
        }

        public bool ParseAndValidateManySurvey(string csv, out List<Survey>? surveys)
        {
            IList<string> lines = csv.Split(linesDelim);
            surveys = new List<Survey>();

            var surveysDb = _unit.SurveyRepo.ReadAll();
            
            int lastId = 0;
            if (surveysDb.Any()) lastId = surveysDb.Max(x => x.Id);

            foreach (string line in lines)
            {
                if (_tryParseSurveyCsv(line, out SurveyRecord surveyRecord))
                {
                    if (_validateSurvey(surveyRecord))
                    {
                        surveys.Add(new Survey()
                        {
                            Id = ++lastId,
                            FromAirport = surveyRecord.FromAirport,
                            ToAirport = surveyRecord.ToAirport,
                            Age = surveyRecord.Age,
                            Sex = surveyRecord.Sex,
                            CabinType = surveyRecord.CabinType,
                            Q1 = surveyRecord.Q1,
                            Q2 = surveyRecord.Q2,
                            Q3 = surveyRecord.Q3,
                            Q4 = surveyRecord.Q4,
                        });
                    }
                    else
                    {
                        surveys = null;
                        return false;
                    }
                }
                else
                {
                    surveys = null;
                    return false;
                }
            }

            return true;
        }

        private bool _tryParseSurveyCsv (string lineCsv, out SurveyRecord? survey)
        {
            IList<string> words = lineCsv.Split(wordsDelim);
            survey = null;

            if (words.Count != recordsCount) 
                return false;

            if (words[2] == "") words[2] = "0";
            if (words[5] == "") words[5] = "0";
            if (words[6] == "") words[6] = "0";
            if (words[7] == "") words[7] = "0";
            if (words[8] == "") words[8] = "0";

            if (!int.TryParse(words[2], out int age)) 
                return false;
            if (!int.TryParse(words[5], out int q1)) 
                return false;
            if (!int.TryParse(words[6], out int q2)) 
                return false;
            if (!int.TryParse(words[7], out int q3)) 
                return false;
            if (!int.TryParse(words[8], out int q4)) 
                return false;

            survey = new SurveyRecord ()
            {
                FromAirport = words[0],
                ToAirport = words[1],
                Age = age,
                Sex = words[3],
                CabinType = words[4] == "First" ? "First Class" : words[4],
                Q1 = q1,
                Q2 = q2,
                Q3 = q3,
                Q4 = q4,
            };
            return true;
        }

        private bool _validateSurvey(SurveyRecord survey)
        {
            if (survey.FromAirport != "")
            {
                var fromAirport = _unit.AirportRepo.ReadFirst(a => a.Iatacode == survey.FromAirport);
                if (fromAirport == null) return false;
            }
            
            if (survey.ToAirport != "")
            {
                var toAirport = _unit.AirportRepo.ReadFirst(a => a.Iatacode == survey.ToAirport);
                if (toAirport == null) return false;
            }

            if (survey.Sex != "M" && survey.Sex != "F" && survey.Sex != "") return false;

            if (survey.CabinType != "")
            {
                var cabinType = _unit.CabintypeRepo.ReadFirst(c => c.Name == survey.CabinType);
                if (cabinType == null) return false;
            }

            return true;
        }
    }
}
