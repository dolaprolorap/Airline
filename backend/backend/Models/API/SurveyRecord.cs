namespace backend.Models.API
{
    public class SurveyRecord
    {
        public string? FromAirport { get; set; }
        public string? ToAirport { get; set; }
        public int? Age { get; set; }
        public string? Sex { get; set; }
        public string? CabinType { get; set; }
        public int Q1 { get; set; }
        public int Q2 { get; set; }
        public int Q3 { get; set; }
        public int Q4 { get; set; }
    }
}
