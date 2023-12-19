namespace backend.Models.DB
{
    public class Seat
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int SeatNumber { get; set; }
        public virtual Ticket Ticket { get; set; } = null!;
    }
}
