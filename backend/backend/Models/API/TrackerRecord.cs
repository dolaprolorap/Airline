using TrackerRecordDb = backend.Models.DB.Trackerrecord;

namespace backend.Models.API
{
    public class TrackerRecord
    {
        public DateTime? DateTime { get; set; }
        public string? RecordType { get; set; }
        public string? WarnType { get; set; }
        public string? Description { get; set; }

        public TrackerRecord(TrackerRecordDb recordDb)
        {
            DateTime = recordDb.Datetime;
            RecordType = recordDb.RecordtypeNavigation!.Name;
            WarnType = recordDb.WarntypeNavigation!.Name;
            Description = recordDb.Description;
        }
    }
}
