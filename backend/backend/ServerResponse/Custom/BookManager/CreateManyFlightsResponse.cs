using backend.Models.API;

namespace backend.ServerResponse.Custom.BookManager
{
    public enum CreateManyFlightsResponseType
    {
        FromAirportCodeInvalid,
        ToAirportCodeInvalid,
        FromDateInvalid,
        Ok
    }

    public class CreateManyFlightsResponse : StatusResponse
    {
        public CreateManyFlightsResponseType ResponseType { get; protected set; }
        public List<ManyFlightsData>? ManyFlights { get; protected set; }

        public CreateManyFlightsResponse(
            CreateManyFlightsResponseType type,
            string? fromCode = null,
            string? toCode = null,
            string? fromDate = null,
            List<ManyFlightsData>? manyFlights = null) 
        {
            ResponseType = type;

            ManyFlights = manyFlights;

            switch (type)
            {
                case CreateManyFlightsResponseType.FromAirportCodeInvalid:
                    Status = StatusResponseType.UserFail;
                    UserMsg = CreateManyFlightsResponseType.FromAirportCodeInvalid.ToString();
                    LoggerMsg = "There is not airpot with that code (FromAirport): " + fromCode ?? "null";
                    break;
                case CreateManyFlightsResponseType.ToAirportCodeInvalid:
                    Status = StatusResponseType.UserFail;
                    UserMsg = CreateManyFlightsResponseType.ToAirportCodeInvalid.ToString();
                    LoggerMsg = "There is not airpot with that code (ToAirport): " + toCode ?? "null";
                    break;
                case CreateManyFlightsResponseType.FromDateInvalid:
                    Status = StatusResponseType.UserFail;
                    UserMsg = CreateManyFlightsResponseType.FromDateInvalid.ToString();
                    LoggerMsg = "Invalid departure date string: " + fromDate ?? "null";
                    break;
                case CreateManyFlightsResponseType.Ok:
                    Status = StatusResponseType.Success;
                    UserMsg = CreateManyFlightsResponseType.Ok.ToString();
                    LoggerMsg = "Flights successfuly being found";
                    break;
            }

            Data = new
            {
                ManyFlights
            };
        }
    }
}
