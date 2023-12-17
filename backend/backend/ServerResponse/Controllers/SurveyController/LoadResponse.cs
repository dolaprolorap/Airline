namespace backend.ServerResponse.Controllers.SurveyController
{
    public enum LoadResponseType
    {
        FailedGettingFile,
        NoFile,
        InvalidFormat,
        Loaded
    }

    public class LoadResponse : StatusResponse
    {
        public LoadResponseType ResponseType { get; set; }

        public LoadResponse(
            LoadResponseType type,
            string? excMsg = null)
        {
            ResponseType = type;
            UserMsg = type.ToString();

            switch (type)
            {
                case LoadResponseType.FailedGettingFile:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = "Failed get file, excMsg " + excMsg ?? "null";
                    break;
                case LoadResponseType.NoFile:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = "No file for survey controller";
                    break;
                case LoadResponseType.InvalidFormat:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = "Invalid csv file";
                    break;
                case LoadResponseType.Loaded:
                    Status = StatusResponseType.Success;
                    LoggerMsg = "File loaded to survey controller";
                    break;
            }
        }
    }
}
