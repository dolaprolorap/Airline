using backend.Models.API;

namespace backend.ServerRequest.Custom.BookingManager
{
    public class BookRequest
    {
        public int UserId { get; set; }
        public BookData BookData { get; set; }
    }
}
