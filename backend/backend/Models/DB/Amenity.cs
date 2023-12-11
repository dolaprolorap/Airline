using System;
using System.Collections.Generic;

namespace backend.Models.DB;

public partial class Amenity
{
    public int Id { get; set; }

    public string Service { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual ICollection<Amenitiesticket> Amenitiestickets { get; set; } = new List<Amenitiesticket>();

    public virtual ICollection<Cabintype> CabinTypes { get; set; } = new List<Cabintype>();
}
