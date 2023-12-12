using backend.Models.API;
using backend.Models.DB;
using backend.ServerResponse;

namespace backend.Services
{
    public interface IScheduleService
    {
        public StatusResponse GetSchedule(ScheduleFilter scheduleFilter);
        public StatusResponse SetActiveFlight(SetActiveFlight setActiveFlight);
        public StatusResponse EditFlight(EditFlight editFlight);
        public StatusResponse EditFlightsByCsv(string csv);
    }
}
