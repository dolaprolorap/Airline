namespace backend.ServerResponse.Controllers.AdminPanelController
{
    public enum SetActiveUserResponseType
    {
        UserNotFound,
        ActiveStateChanged
    }

    public class SetActiveUserResponse : StatusResponse
    {
        public SetActiveUserResponseType ResponseType { get; protected set; }

        public SetActiveUserResponse(
            SetActiveUserResponseType responseType,
            string? email = null) 
        {
            ResponseType = responseType;
            UserMsg = responseType.ToString();

            switch (responseType)
            {
                case SetActiveUserResponseType.UserNotFound:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = "User with that email not found: " + email;
                    break;
                case SetActiveUserResponseType.ActiveStateChanged:
                    Status = StatusResponseType.Success;
                    LoggerMsg = "User active state changed: " + email;
                    break;
            }
        }
    }
}
