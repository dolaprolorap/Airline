namespace backend.Models.API
{
    public class BookData
    {
        public List<int> ScheduleIds { get; set; }
        public string CabinType { get; set; }
        public List<Passenger> Passengers { get; set; }
    }
}
