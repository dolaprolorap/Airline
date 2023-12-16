namespace backend.ServerResponse.Controllers.AdminPanelController
{
    public enum AddUserResponseType
    {
        UserAdded
    }

    public class AddUserResponse : StatusResponse
    {
        public AddUserResponseType ResponseType { get; set; }

        public AddUserResponse (
            AddUserResponseType responseType,
            string? addedUserEmail = null)
        {
            ResponseType = responseType;
            UserMsg = responseType.ToString();

            switch (responseType)
            {
                case AddUserResponseType.UserAdded:
                    Status = StatusResponseType.Success;
                    LoggerMsg = "User with email added: " + addedUserEmail ?? "null";
                    break;
            }
        }
    }
}
