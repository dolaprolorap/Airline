namespace backend.ServerResponse.Controllers.AdminPanelController
{
    public enum GetUsersByOfficeIdResponseType
    {
        OfficeNotFound,
        UsersGotten
    }

    public class GetUsersByOfficeIdResponse : StatusResponse
    {
        public GetUsersByOfficeIdResponseType ResponseType { get; protected set; }

        public GetUsersByOfficeIdResponse(
            GetUsersByOfficeIdResponseType responseType,
            bool officeIdSpecified,
            string? officeName = null,
            object? foundUsers = null)
        {
            ResponseType = responseType;
            UserMsg = responseType.ToString();

            switch (ResponseType)
            {
                case GetUsersByOfficeIdResponseType.OfficeNotFound:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = "Office not found with name: " + officeName ?? "null";
                    break;
                case GetUsersByOfficeIdResponseType.UsersGotten:
                    Status = StatusResponseType.Success;
                    LoggerMsg = officeIdSpecified ?
                        "Users got with office name: " + officeName ?? "null" :
                        "All users got";
                    Data = foundUsers;
                    break;
            }
        }
    }
}
