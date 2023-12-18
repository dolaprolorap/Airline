using System;
using System.Collections.Generic;

namespace backend.Models.DB;

public partial class Amenitiesticket
{
    public int Id { get; set; }

    public int AmenityId { get; set; }

    public int TicketId { get; set; }

    public decimal Price { get; set; }

    public virtual Amenity Amenity { get; set; } = null!;

    public virtual Ticket Ticket { get; set; } = null!;
}
