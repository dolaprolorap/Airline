namespace backend.Models.API
{
    public class ScheduleRecord
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string FromAirportName { get; set; }
        public string ToAirportName { get; set; }
        public string FlightNumber { get; set; }
        public string Aircraft { get; set; }
        public int EconomyPrice { get; set; }
        public bool IsActive { get; set; }
    }
}
