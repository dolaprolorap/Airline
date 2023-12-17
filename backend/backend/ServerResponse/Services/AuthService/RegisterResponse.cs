using backend.Models.DB;

namespace backend.ServerResponse.Services.AuthService
{
    public enum RegisterResponseType
    {
        UserAlreadyExists,
        RoleNotFound,
        OfficeNotFound,
        InvalidDateTimeFormat,
        UserCreated
    }

    public class RegisterResponse : StatusResponse
    {
        public RegisterResponse(
            RegisterResponseType type, 
            string? userEmail = null, 
            string? roleName = null, 
            string? officeName = null,
            string? dateTimeStr = null)
        {
            UserMsg = type.ToString();

            switch (type)
            {
                case RegisterResponseType.UserAlreadyExists:
                    Status = StatusResponseType.Success;
                    LoggerMsg = string.Format("User with email '{0}' already exists", userEmail);
                    break;
                case RegisterResponseType.RoleNotFound:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = string.Format("Role name '{0}' was not found", roleName);
                    break;
                case RegisterResponseType.OfficeNotFound:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = string.Format("Office name '{0}' was not found", officeName);
                    break;
                case RegisterResponseType.InvalidDateTimeFormat:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = string.Format("Invalid DateTime format string '{0}'", dateTimeStr);
                    break;
                case RegisterResponseType.UserCreated:
                    Status = StatusResponseType.Success;
                    LoggerMsg = "User has been registrated";
                    break;
            }
        }
    }
}
