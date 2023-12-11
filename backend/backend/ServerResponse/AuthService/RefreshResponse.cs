namespace backend.ServerResponse.AuthService
{
    public enum RefreshResponseType
    {
        NoNameClaim,
        NameClaimIsNull,
        RefreshTokenExpired,
        FailUpdateToken,
        Ok
    }

    public class RefreshResponse : StatusResponse
    {
        public RefreshResponse(RefreshResponseType type, object? data = null, string? oldAccessToken = null, 
            string? newAccessToken = null)
        {
            switch(type)
            {
                case RefreshResponseType.NoNameClaim:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = string.Format("No name claim in token '{0}'", oldAccessToken ?? "null");
                    UserMsg = "Token error";
                    break;
                case RefreshResponseType.NameClaimIsNull:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = string.Format("Name claim is null in token '{0}'", oldAccessToken ?? "null");
                    UserMsg = "Token error";
                    break;
                case RefreshResponseType.RefreshTokenExpired:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = string.Format("Refresh token expired");
                    UserMsg = "Token error";
                    break;
                case RefreshResponseType.FailUpdateToken:
                    Status = StatusResponseType.UserFail;
                    LoggerMsg = string.Format("Failed to update token");
                    UserMsg = "Token error";
                    break;
                case RefreshResponseType.Ok:
                    Status = StatusResponseType.Success;
                    LoggerMsg = string.Format("Token '{0}' refreshed to '{1}'", 
                        oldAccessToken ?? "null",
                        newAccessToken ?? "null");
                    break;
            }

            Data = data;
        }
    }
}
