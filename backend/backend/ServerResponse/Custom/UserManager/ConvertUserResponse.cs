using backend.Models.API;

namespace backend.ServerResponse.Custom.UserManager
{
    public enum ConvertUserResponseType
    {
        RoleNotFound,
        OfficeNotFound,
        Ok
    }

    public class ConvertUserResponse : StatusResponse
    {
        public ConvertUserResponseType ResponseType { get; protected set; }
        public UserData? User { get; protected set; }

        public ConvertUserResponse(
            ConvertUserResponseType type,
            int? roleId = null,
            int? officeId = null,
            UserData? user = null)
        {
            ResponseType = type;
            User = user;

            switch (type)
            {
                case ConvertUserResponseType.RoleNotFound:
                    Status = StatusResponseType.UserFail;
                    UserMsg = "RoleNotFound";
                    LoggerMsg = "Role with that id was not found: " + roleId ?? "null";
                    break;
                case ConvertUserResponseType.OfficeNotFound:
                    Status = StatusResponseType.UserFail;
                    UserMsg = "OfficeNotFound";
                    LoggerMsg = "Office with that id was not found: " + officeId ?? "null";
                    break;
                case ConvertUserResponseType.Ok:
                    Status = StatusResponseType.Success;
                    UserMsg = "UserConverted";
                    LoggerMsg = "User converted";
                    break;
            }

            Data = new 
            {
                User = user
            };
        }
    }
}
