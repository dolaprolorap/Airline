namespace backend.Models.API
{
    public class SearchFlightModel
    {
        public string FromCode { get; set; }
        public string ToCode { get; set; }
        public string CabinType { get; set; }
        public bool WithReturn { get; set; }
        public string OutboundDate { get; set; }
        public string ReturnDate { get; set; }
    }
}
