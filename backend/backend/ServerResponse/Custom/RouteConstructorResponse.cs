namespace backend.ServerResponse.Custom
{
    public enum RouteConstructorResponseType
    {
        FromAirportIdNotExists,
        ToAirportIdNotExists,
        Ok
    }

    public class RouteConstructorResponse : StatusResponse
    {
        public RouteConstructorResponseType ResponseType { get; protected set; }

        public RouteConstructorResponse(
            RouteConstructorResponseType type,
            int? fromAirportId = null,
            int? toAirportId = null) 
        {
            ResponseType = type;

            switch (type)
            {
                case RouteConstructorResponseType.FromAirportIdNotExists:
                    Status = StatusResponseType.UserFail;
                    UserMsg = RouteConstructorResponseType.FromAirportIdNotExists.ToString();
                    LoggerMsg = "There is no airport with this id (FromAirport): " + fromAirportId ?? "null";
                    break;
                case RouteConstructorResponseType.ToAirportIdNotExists:
                    Status = StatusResponseType.UserFail;
                    UserMsg = RouteConstructorResponseType.ToAirportIdNotExists.ToString();
                    LoggerMsg = "There is no airport with this id (ToAirport): " + toAirportId ?? "null";
                    break;
                case RouteConstructorResponseType.Ok:
                    Status = StatusResponseType.Success;
                    UserMsg = RouteConstructorResponseType.Ok.ToString();
                    LoggerMsg = "Paths of routes have been constructed";
                    break;
            }
        }
    }
}
