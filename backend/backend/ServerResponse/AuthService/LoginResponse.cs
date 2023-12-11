namespace backend.ServerResponse.AuthService
{
    public enum LoginResponseType
    {
        UserWasNotFound,
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
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = string.Format("User with email '{0}' not found", userEmail);
                    UserMsg = "User was not found";
                    break;
                case LoginResponseType.InvalidPassword:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = string.Format("Invalid password '{0}' for user '{1}'", password, userEmail);
                    UserMsg = "Invalid password";
                    break;
                case LoginResponseType.TokensNotUpdated:
                    Status = StatusResponseType.ServerFail;
                    LoggerMsg = string.Format("Fail on update token after login with email '{0}'", userEmail);
                    UserMsg = "Interal server error";
                    break;
                case LoginResponseType.Ok:
                    Status = StatusResponseType.Success;
                    LoggerMsg = string.Format("User '{0}' loged in", userEmail);
                    break;
            }

            Data = data;
        }
    }
}
