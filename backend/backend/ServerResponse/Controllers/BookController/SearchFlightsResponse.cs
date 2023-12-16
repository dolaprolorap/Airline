using backend.Models.API;

namespace backend.ServerResponse.Controllers.BookController
{
    public enum SearchFlightsResponseType
    {
        FailedCreateManyFlights,
        Ok
    }

    public class SearchFlightsResponse : StatusResponse
    {
        public SearchFlightsResponseType ResponseType { get; protected set; }

        public IEnumerable<ManyFlightsData>? ForwardManyFlights { get; protected set; }
        public IEnumerable<ManyFlightsData>? ReturnManyFlights { get; protected set; }

        public SearchFlightsResponse(
            SearchFlightsResponseType type,
            List<ManyFlightsData>? forwardManyFlights = null,
            List<ManyFlightsData>? returnManyFlights = null)
        {
            ResponseType = type;

            ForwardManyFlights = forwardManyFlights;
            ReturnManyFlights = returnManyFlights;

            switch (type)
            {
                case SearchFlightsResponseType.FailedCreateManyFlights:
                    Status = StatusResponseType.UserFail;
                    UserMsg = SearchFlightsResponseType.FailedCreateManyFlights.ToString();
                    LoggerMsg = "Error occured while many flights were being created";
                    break;
                case SearchFlightsResponseType.Ok:
                    Status = StatusResponseType.Success;
                    UserMsg = SearchFlightsResponseType.Ok.ToString();
                    LoggerMsg = "Flights successfuly being found";
                    break;
            }

            Data = new
            {
                ForwardManyFlights,
                ReturnManyFlights
            };
        }
    }
}
