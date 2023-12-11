namespace backend.Models.API
{
    public class ScheduleFilter
    {
        public string? FromAirportName { get; set; }
        public string? ToAirportName { get; set; }
        public string? Outbound { get; set; }
        public string? FlightNumber { get; set; }
    }
}
