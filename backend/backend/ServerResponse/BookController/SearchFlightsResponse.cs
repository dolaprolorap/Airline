namespace backend.ServerResponse.BookController
{
    public enum SearchFlightsResponseType
    {
        FromAirportCodeInvalid,
        ToAirportCodeInvalid,
        FromDateInvalid,
        Ok
    }

    public class SearchFlightsResponse : StatusResponse
    {
        public SearchFlightsResponseType ResponseType { get; protected set; }

        public SearchFlightsResponse(
            SearchFlightsResponseType type,
            string? fromCode = null,
            string? toCode = null,
            string? fromDate = null) 
        {
            ResponseType = type;

            switch (type)
            {
                case SearchFlightsResponseType.FromAirportCodeInvalid:
                    Status = StatusResponseType.UserFail;
                    UserMsg = SearchFlightsResponseType.FromAirportCodeInvalid.ToString();
                    LoggerMsg = "There is not airpot with that code (FromAirport): " + fromCode ?? "null";
                    break;
                case SearchFlightsResponseType.ToAirportCodeInvalid:
                    Status = StatusResponseType.UserFail;
                    UserMsg = SearchFlightsResponseType.ToAirportCodeInvalid.ToString();
                    LoggerMsg = "There is not airpot with that code (ToAirport): " + toCode ?? "null";
                    break;
                case SearchFlightsResponseType.FromDateInvalid:
                    Status = StatusResponseType.UserFail;
                    UserMsg = SearchFlightsResponseType.FromDateInvalid.ToString();
                    LoggerMsg = "Invalid departure date string: " + fromDate ?? "null";
                    break;
                case SearchFlightsResponseType.Ok:
                    Status = StatusResponseType.Success;
                    UserMsg = SearchFlightsResponseType.Ok.ToString();
                    LoggerMsg = "Flights successfuly being found";
                    break;
            }
        }
    }
}
