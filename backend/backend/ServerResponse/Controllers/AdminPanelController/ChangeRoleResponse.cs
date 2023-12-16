namespace backend.ServerResponse.Controllers.AdminPanelController
{
    public enum ChangeRoleResponseType
    {
        UserNotFound,
        RoleNotFound,
        RoleChanged
    }

    public class ChangeRoleResponse : StatusResponse
    {
        public ChangeRoleResponseType ResponseType { get; protected set; }

        public ChangeRoleResponse(
            ChangeRoleResponseType responseType,
            string? email = null,
            string? roleName = null)
        {
            ResponseType = responseType;
            UserMsg = responseType.ToString();

            switch (responseType)
            {
                case ChangeRoleResponseType.UserNotFound:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = "User with that email has not been found: " + email ?? "null";
                    break;
                case ChangeRoleResponseType.RoleNotFound:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = "Role with that name has not been found: " + roleName ?? "null";
                    break;
                case ChangeRoleResponseType.RoleChanged:
                    Status = StatusResponseType.Success;
                    LoggerMsg = "Role changed for email: " + email ?? "null";
                    break;
            }
        }
    }
}
