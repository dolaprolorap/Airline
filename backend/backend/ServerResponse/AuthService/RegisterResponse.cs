using backend.Models.DB;

namespace backend.ServerResponse.AuthService
{
    public enum RegisterResponseType
    {
        UserAlreadyExists,
        RoleIdWasNotFound,
        OfficeIdWasNotFound,
        InvalidDateTimeFormat,
        Ok
    }

    public class RegisterResponse : StatusResponse
    {
        public RegisterResponse(RegisterResponseType type, string? userEmail = null, int? roleId = null, int? officeId = null, 
            string? dateTimeStr = null)
        {
            switch (type)
            {
                case RegisterResponseType.UserAlreadyExists:
                    Status = StatusResponseType.Success;
                    LoggerMsg = string.Format("User with email '{0}' already exists", userEmail);
                    UserMsg = "User already exists";
                    break;
                case RegisterResponseType.RoleIdWasNotFound:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = string.Format("Role id '{0}' was not found", roleId);
                    UserMsg = "Error occured";
                    break;
                case RegisterResponseType.OfficeIdWasNotFound:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = string.Format("Office id '{0}' was not found", officeId);
                    UserMsg = "Error occured";
                    break;
                case RegisterResponseType.InvalidDateTimeFormat:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = string.Format("Invalid DateTime format string '{0}'", dateTimeStr);
                    UserMsg = "Error occured";
                    break;
                case RegisterResponseType.Ok:
                    Status = StatusResponseType.Success;
                    LoggerMsg = "User has been registrated";
                    break;
            }
        }
    }
}
