using User = backend.Models.DB.User;
using UserData = backend.Models.API.UserData;

namespace backend.ServerResponse.Services.AuthService
{
    public enum GetMyselfResponseType
    {
        UserNotFound,
        ConvertError,
        Ok
    }

    public class GetMyselfResponse : StatusResponse
    {
        public GetMyselfResponseType ResponseType { get; protected set; }
        public UserData? UserData { get; protected set; }

        public GetMyselfResponse(
            GetMyselfResponseType type,
            string? email = null,
            UserData? userData = null)
        {
            ResponseType = type;
            UserData = userData;
            UserMsg = type.ToString();

            switch (type)
            {
                case GetMyselfResponseType.UserNotFound:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = "User with that email was not found: " + email ?? "null";
                    break;
                case GetMyselfResponseType.ConvertError:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = "Convert error to UserData, see logs";
                    break;
                case GetMyselfResponseType.Ok:
                    Status = StatusResponseType.Success;
                    LoggerMsg = "User found: " + email ?? "null";
                    break;
            }

            Data = new
            {
                User = userData
            };
        }
    }
}
