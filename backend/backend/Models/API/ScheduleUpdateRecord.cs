namespace backend.Models.API
{
    public class ScheduleUpdateRecord
    {
        public ScheduleUpdateRecordType Type { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public int FlightId { get; set; }
        public string FromAirportCode { get; set; }
        public string ToAirportCode { get; set; }
        public int AircraftId { get; set; }
        public int EconomyPrice { get; set; }
        public bool IsActive { get; set; }
    }
}
