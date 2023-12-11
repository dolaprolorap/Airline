using System;
using System.Collections.Generic;

namespace backend.Models.DB;

public partial class Trackerrecordtype
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Trackerrecord> Trackerrecords { get; set; } = new List<Trackerrecord>();
}
