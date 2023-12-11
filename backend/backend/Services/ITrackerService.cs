using backend.ServerResponse;

namespace backend.Services
{
    public interface ITrackerService
    {
        StatusResponse SuccessLogin(string email);
        StatusResponse UnsuccessTryLogin(string email, string reason);
        StatusResponse SuccessLogout(string email);
        StatusResponse UnsuccessTryLogout(string email, string reason);
        StatusResponse GetByEmail(string email);
    }
}
