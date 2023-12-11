using System;
using System.Collections.Generic;

namespace backend.Models.DB;

public partial class Trackerrecord
{
    public int Id { get; set; }

    public int? Recordtype { get; set; }

    public int? Warntype { get; set; }

    public DateTime? Datetime { get; set; }

    public string? Description { get; set; }

    public int? Userid { get; set; }

    public virtual Trackerrecordtype? RecordtypeNavigation { get; set; }

    public virtual User? User { get; set; }

    public virtual Warntype? WarntypeNavigation { get; set; }
}
