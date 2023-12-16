namespace backend.ServerResponse.Controllers.AdminPanelController
{
    public enum GetUsersByOfficeIdResponseType
    {
        UsersGotten
    }

    public class GetUsersByOfficeIdResponse : StatusResponse
    {
        public GetUsersByOfficeIdResponseType ResponseType { get; protected set; }

        public GetUsersByOfficeIdResponse(
            GetUsersByOfficeIdResponseType responseType,
            bool officeIdSpecified,
            int? officeId = null,
            object? foundUsers = null)
        {
            ResponseType = responseType;
            UserMsg = responseType.ToString();

            switch (ResponseType)
            {
                case GetUsersByOfficeIdResponseType.UsersGotten:
                    Status = StatusResponseType.Success;
                    LoggerMsg = officeIdSpecified ?
                        "Users got with office id: " + officeId ?? "null" :
                        "All users got";
                    Data = foundUsers;
                    break;
            }
        }
    }
}
