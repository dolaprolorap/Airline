using backend.Models.DB;

namespace backend.ServerResponse.Services.AuthService
{
    public enum RegisterResponseType
    {
        UserAlreadyExists,
        RoleIdWasNotFound,
        OfficeIdWasNotFound,
        InvalidDateTimeFormat,
        UserCreated
    }

    public class RegisterResponse : StatusResponse
    {
        public RegisterResponse(RegisterResponseType type, string? userEmail = null, int? roleId = null, int? officeId = null,
            string? dateTimeStr = null)
        {
            UserMsg = type.ToString();

            switch (type)
            {
                case RegisterResponseType.UserAlreadyExists:
                    Status = StatusResponseType.Success;
                    LoggerMsg = string.Format("User with email '{0}' already exists", userEmail);
                    break;
                case RegisterResponseType.RoleIdWasNotFound:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = string.Format("Role id '{0}' was not found", roleId);
                    break;
                case RegisterResponseType.OfficeIdWasNotFound:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = string.Format("Office id '{0}' was not found", officeId);
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
