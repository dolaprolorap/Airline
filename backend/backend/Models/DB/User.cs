using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace backend.Models.DB;

public partial class User
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FirstName { get; set; }

    public string LastName { get; set; } = null!;

    public int? OfficeId { get; set; }

    public DateOnly? Birthdate { get; set; }

    public bool? Active { get; set; }

    public virtual Office? Office { get; set; }

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<Trackerrecord> Trackerrecords { get; set; } = new List<Trackerrecord>();

    public virtual Token Token { get; set; } = null!;
}
