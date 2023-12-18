namespace backend.ServerRequest.Controllers.AmenitiesController
{
    public class ApplyAmenitiesRequest
    {
        public int TicketId { get; set; }
        public List<string> AmenityNamesToApply { get; set; }
    }
}
