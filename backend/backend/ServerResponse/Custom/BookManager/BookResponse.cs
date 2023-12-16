namespace backend.ServerResponse.Custom.BookManager
{
    public enum BookResponseType
    {
        UserNotFound,
        CabinNameNotFound,
        ScheduleNotFound,
        PassportCountryNotFound,
        Ok
    }

    public class BookResponse : StatusResponse
    {
        public BookResponseType ResponseType { get; set; } 

        public BookResponse(
            BookResponseType type,
            string? userId = null,
            string? cabinName = null,
            string? scheduleId = null,
            string? countryName = null) 
        {
            ResponseType = type;

            switch(type)
            {
                case BookResponseType.UserNotFound:
                    Status = StatusResponseType.UserFail;
                    UserMsg = BookResponseType.UserNotFound.ToString();
                    LoggerMsg = "User with that id was not found: " + userId ?? "null";
                    break;
                case BookResponseType.CabinNameNotFound:
                    Status = StatusResponseType.UserFail;
                    UserMsg = BookResponseType.CabinNameNotFound.ToString();
                    LoggerMsg = "Cabin with that name was not found: " + cabinName ?? "null";
                    break;
                case BookResponseType.ScheduleNotFound:
                    Status = StatusResponseType.UserFail;
                    UserMsg = BookResponseType.ScheduleNotFound.ToString();
                    LoggerMsg = "Schedule with that id was not found: " + scheduleId ?? "null";
                    break;
                case BookResponseType.PassportCountryNotFound:
                    Status = StatusResponseType.UserFail;
                    UserMsg = BookResponseType.PassportCountryNotFound.ToString();
                    LoggerMsg = "Passport country with that name was not found: " + countryName ?? "null";
                    break;
                case BookResponseType.Ok:
                    Status = StatusResponseType.Success;
                    UserMsg = BookResponseType.Ok.ToString();
                    LoggerMsg = "Ticket has been booked";
                    break;
            }
        }
    }
}
