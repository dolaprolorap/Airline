namespace backend.Models.API
{
    public class TicketData
    {
        public int ScheduleId { get; set; }
        public string CabinType { get; set; }
        public Passenger Passenger { get; set; }
        public string BookingRef { get; set; }
    }
}
