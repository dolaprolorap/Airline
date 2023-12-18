namespace backend.ServerResponse.Services.AuthService
{
    public enum LoginResponseType
    {
        UserWasNotFound,
        UnactiveUser,
        InvalidPassword,
        TokensNotUpdated,
        Ok
    }

    public class LoginResponse : StatusResponse
    {
        public LoginResponse(LoginResponseType type, object? data = null, string? userEmail = null, string? password = null)
        {
            switch (type)
            {
                case LoginResponseType.UserWasNotFound:
                    Status = StatusResponseType.Success;
                    LoggerMsg = string.Format("User with email '{0}' not found", userEmail);
                    UserMsg = "UserNotFound";
                    break;
                case LoginResponseType.UnactiveUser:
                    Status = StatusResponseType.Success;
                    LoggerMsg = string.Format("User with email '{0}' is unactive", userEmail);
                    UserMsg = "UnactiveUser";
                    break;
                case LoginResponseType.InvalidPassword:
                    Status = StatusResponseType.Success;
                    LoggerMsg = string.Format("Invalid password '{0}' for user '{1}'", password, userEmail);
                    UserMsg = "InvalidPwd";
                    break;
                case LoginResponseType.TokensNotUpdated:
                    Status = StatusResponseType.ServerFail;
                    LoggerMsg = string.Format("Fail on update token after login with email '{0}'", userEmail);
                    UserMsg = "InternalServerError";
                    break;
                case LoginResponseType.Ok:
                    Status = StatusResponseType.Success;
                    LoggerMsg = string.Format("User '{0}' loged in", userEmail);
                    UserMsg = "LoggedIn";
                    break;
            }

            Data = data;
        }
    }
}
