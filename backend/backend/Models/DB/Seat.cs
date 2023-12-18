namespace backend.Models.DB
{
    public class Seat
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public int SeatNumber { get; set; }
        public virtual Schedule Schedule { get; set; }
    }
}
