namespace backend.ServerResponse.Services.TrackerService
{
    public enum GetByEmailResponseType
    {
        UserNotFound,
        RecordsGotten
    }

    public class GetByEmailResponse : StatusResponse
    {
        public GetByEmailResponseType ResponseType { get; protected set; }

        public GetByEmailResponse(
            GetByEmailResponseType type,
            string? userEmail = null,
            object? records = null) 
        {
            ResponseType = type;
            UserMsg = type.ToString();

            switch (type) 
            { 
                case GetByEmailResponseType.UserNotFound:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = "User with this email was not found: " + userEmail ?? "null";
                    break;
                case GetByEmailResponseType.RecordsGotten:
                    Status = StatusResponseType.Success;
                    LoggerMsg = "Records have been gotten for user with email: " + userEmail ?? "null";
                    Data = records;
                    break;
            }
        }
    }
}
