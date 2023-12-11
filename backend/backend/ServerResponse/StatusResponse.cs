using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace backend.ServerResponse
{
    public enum StatusResponseType
    {
        Success,
        SuccessWarn,
        UserFail,
        ServerFail
    }

    public class StatusResponse
    {
        public static bool ShowAllLogs { get; set; } = true;

        public StatusResponseType Status { get; protected set; }
        public string UserMsg { get; protected set; } = "";
        private string _loggerMsg = "";
        public string LoggerMsg 
        { 
            get
            {
                return _loggerMsg;
            } 
            protected set
            {
                _loggerMsg = value;

                if (LoggerMsg.Length != 0)
                {
                    ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                    ILogger logger = loggerFactory.CreateLogger<StatusResponse>();

                    Dictionary<StatusResponseType, LogLevel> conformityDict = new Dictionary<StatusResponseType, LogLevel>
                    {
                        { StatusResponseType.Success, LogLevel.Information },
                        { StatusResponseType.SuccessWarn, LogLevel.Warning },
                        { StatusResponseType.UserFail, LogLevel.Error },
                        { StatusResponseType.ServerFail, LogLevel.Critical }
                    };

                    if (Status == StatusResponseType.ServerFail || ShowAllLogs)
                    {
                        logger.Log(conformityDict[Status], LoggerMsg);
                    }
                }
            }
        }
        public object? Data { get; protected set; } = null;

        public StatusResponse() { }

        public StatusResponse(StatusResponseType status) 
        {
            Status = status;
        }

        public StatusResponse(StatusResponseType status, string userMsg) : this(status)
        {
            UserMsg = userMsg;
        }

        public StatusResponse(StatusResponseType status, string userMsg, string loggerMsg) : this(status, userMsg)
        {
            LoggerMsg = loggerMsg;
        }

        public StatusResponse(StatusResponseType status, string userMsg, string loggerMsg, object? data) : this(status, userMsg, loggerMsg)
        {
            Data = data;
        }

        public IActionResult ConvertToActionResult()
        {
            switch (Status)
            {
                case StatusResponseType.Success:
                case StatusResponseType.SuccessWarn:
                    return new OkObjectResult(new
                    {
                        Msg = UserMsg,
                        Data
                    });
                case StatusResponseType.UserFail:
                    return new BadRequestObjectResult(new
                    {
                        Msg = UserMsg,
                        Data
                    });
                case StatusResponseType.ServerFail:
                    var value = new
                    {
                        Msg = UserMsg,
                        Data
                    };
                    return new ObjectResult(value)
                    {
                        StatusCode = 500
                    };
                default:
                    // impossible situation
                    return new OkObjectResult(null);
            }
        }
    }
}
