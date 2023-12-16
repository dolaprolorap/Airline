namespace backend.ServerResponse.Services.AuthService
{
    public enum ExitResponseType
    {
        UserNotFound,
        TokenNotFound,
        Exited
    }

    public class ExitResponse : StatusResponse
    {
        public ExitResponseType ResponseType { get; protected set; }

        public ExitResponse(
            ExitResponseType type,
            string? email = null)
        {
            ResponseType = type;
            UserMsg = type.ToString();

            switch (type)
            {
                case ExitResponseType.UserNotFound:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = "User with email not found: " + email ?? "null";
                    break;
                case ExitResponseType.TokenNotFound:
                    Status = StatusResponseType.ServerFail;
                    LoggerMsg = "There is no token record for user with email: " + email ?? "null";
                    break;
                case ExitResponseType.Exited:
                    Status = StatusResponseType.Success;
                    LoggerMsg = "User with email exited: " + email ?? "null";
                    break;
            }
        }
    }
}
