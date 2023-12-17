namespace backend.ServerResponse.Controllers.SurveyController
{
    public enum GetSummaryResponseType
    {
        SummaryGotten
    }

    public class GetSummaryResponse : StatusResponse
    {
        public GetSummaryResponseType ResponseType { get; set; }

        public GetSummaryResponse(
            GetSummaryResponseType type,
            object? data = null)
        {
            ResponseType = type;
            UserMsg = type.ToString();
            Data = data;

            switch (type)
            {
                case GetSummaryResponseType.SummaryGotten:
                    Status = StatusResponseType.Success;
                    LoggerMsg = "Summary gotten";
                    break;
            }
        }
    }
}
