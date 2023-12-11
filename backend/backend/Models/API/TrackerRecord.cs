using TrackerRecordDb = backend.Models.DB.Trackerrecord;

namespace backend.Models.API
{
    public class TrackerRecord
    {
        public DateTime? DateTime { get; set; }
        public int? RecordType { get; set; }
        public int? WarnType { get; set; }
        public string? Description { get; set; }

        public TrackerRecord(TrackerRecordDb recordDb)
        {
            DateTime = recordDb.Datetime;
            RecordType = recordDb.Recordtype;
            WarnType = recordDb.Warntype;
            Description = recordDb.Description;
        }
    }
}
