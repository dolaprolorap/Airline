namespace backend.ServerResponse.Services.TrackerService
{
    public enum TrackerResponseType
    {
        UserNotFound,
        TrackerTypeNotFound,
        WarnTypeNotFound,
        Ok
    }

    public class TrackerResponse : StatusResponse
    {
        public TrackerResponse(TrackerResponseType type)
        {
            UserMsg = type.ToString();

            switch (type)
            {
                case TrackerResponseType.UserNotFound:
                    Status = StatusResponseType.ServerFail;
                    LoggerMsg = "There is no user with email";
                    break;
                case TrackerResponseType.TrackerTypeNotFound:
                    Status = StatusResponseType.ServerFail;
                    LoggerMsg = "Tracker type not found";
                    break;
                case TrackerResponseType.WarnTypeNotFound:
                    Status = StatusResponseType.ServerFail;
                    LoggerMsg = "Warn type not found";
                    break;
                case TrackerResponseType.Ok:
                    Status = StatusResponseType.Success;
                    LoggerMsg = "Tracker record is created";
                    break;
            }
        }
    }
}
