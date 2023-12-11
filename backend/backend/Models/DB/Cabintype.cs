using System;
using System.Collections.Generic;

namespace backend.Models.DB;

public partial class Cabintype
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
}
