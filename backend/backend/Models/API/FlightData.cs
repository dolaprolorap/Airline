namespace backend.Models.API
{
    public class FlightData
    {
        public string FromCode { get; set; } = "";      
        public string ToCode { get; set; } = "";
        public string Date { get; set; } = "";
        public string Time { get; set; } = "";
        public string FlightNumber { get; set; } = "";
        public int EconomyPrice { get; set; } = 0;
    }
}
