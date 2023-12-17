namespace backend.ServerResponse.Controllers.SurveyController
{
    public enum GetDetailedResponseType
    {
        DetailedGotten
    }

    public class GetDetailedResponse : StatusResponse
    {
        public GetDetailedResponseType ResponseType { get; set; }

        public GetDetailedResponse(
            GetDetailedResponseType type,
            object? data = null)
        {
            ResponseType = type;
            UserMsg = type.ToString();
            Data = data;

            switch (type)
            {
                case GetDetailedResponseType.DetailedGotten:
                    Status = StatusResponseType.Success;
                    LoggerMsg = "Detailed gotten";
                    break;
            }
        }
    }
}
