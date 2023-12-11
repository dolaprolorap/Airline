namespace backend.Models.API
{
    public class EditFlight
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public int EconomyPrice { get; set; }
    }
}
