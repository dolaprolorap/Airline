using backend.Models.API;

namespace backend.Services
{
    public interface IConvertScheduleRecordService
    {
        public bool ScheduleUpdateRecordFromCsv(string csv, out ScheduleUpdateRecord? record);
    }
}
