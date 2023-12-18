using System;
using System.Collections.Generic;
using backend.Models.DB;
using Microsoft.EntityFrameworkCore;
using Route = backend.Models.DB.Route;

namespace backend.DataAccess;

public partial class AirlinesdbContext : DbContext
{
    public AirlinesdbContext()
    {
    }

    public AirlinesdbContext(DbContextOptions<AirlinesdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aircraft> Aircrafts { get; set; }

    public virtual DbSet<Airport> Airports { get; set; }

    public virtual DbSet<Amenitiesticket> Amenitiestickets { get; set; }

    public virtual DbSet<Amenity> Amenities { get; set; }

    public virtual DbSet<Cabintype> Cabintypes { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Office> Offices { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Route> Routes { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Trackerrecord> Trackerrecords { get; set; }

    public virtual DbSet<Trackerrecordtype> Trackerrecordtypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Warntype> Warntypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Aircraft>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("aircrafts")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.MakeModel)
                .HasMaxLength(10)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
        });

        modelBuilder.Entity<Airport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("airports")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.CountryId, "FK_AirPort_Country");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.Iatacode)
                .HasMaxLength(3)
                .HasColumnName("IATACode");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            entity.HasOne(d => d.Country).WithMany(p => p.Airports)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AirPort_Country");
        });

        modelBuilder.Entity<Amenitiesticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("amenitiestickets")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AmenityId).HasColumnName("AmenityID");
            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.Price).HasPrecision(19, 4);

            entity.HasOne(d => d.Amenity).WithMany(p => p.Amenitiestickets)
                .HasForeignKey(d => d.AmenityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AmenitiesTickets_Amenities");

            entity.HasOne(d => d.Ticket).WithMany(p => p.Amenitiestickets)
                .HasForeignKey(d => d.TicketId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AmenitiesTickets_Tickets");
        });

        modelBuilder.Entity<Amenity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("amenities")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Price).HasPrecision(19, 4);
            entity.Property(e => e.Service)
                .HasMaxLength(50)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
        });

        modelBuilder.Entity<Cabintype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("cabintypes")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            entity.HasMany(d => d.Amenities).WithMany(p => p.CabinTypes)
                .UsingEntity<Dictionary<string, object>>(
                    "Amenitiescabintype",
                    r => r.HasOne<Amenity>().WithMany()
                        .HasForeignKey("AmenityId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TravelClassAdditionalService_AdditionalService"),
                    l => l.HasOne<Cabintype>().WithMany()
                        .HasForeignKey("CabinTypeId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_TravelClassAdditionalService_TravelClass"),
                    j =>
                    {
                        j.HasKey("CabinTypeId", "AmenityId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j
                            .ToTable("amenitiescabintype")
                            .HasCharSet("utf8mb3")
                            .UseCollation("utf8mb3_general_ci");
                        j.HasIndex(new[] { "AmenityId" }, "FK_TravelClassAdditionalService_AdditionalService");
                        j.IndexerProperty<int>("CabinTypeId").HasColumnName("CabinTypeID");
                        j.IndexerProperty<int>("AmenityId").HasColumnName("AmenityID");
                    });
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("countries")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
        });

        modelBuilder.Entity<Office>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("offices")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.CountryId, "FK_Office_Country");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Contact)
                .HasMaxLength(250)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CountryId).HasColumnName("CountryID");
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            entity.HasOne(d => d.Country).WithMany(p => p.Offices)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Office_Country");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("roles")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
        });

        modelBuilder.Entity<Route>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("routes")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.DepartureAirportId, "FK_Routes_Airports2");

            entity.HasIndex(e => e.ArrivalAirportId, "FK_Routes_Airports3");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ArrivalAirportId).HasColumnName("ArrivalAirportID");
            entity.Property(e => e.DepartureAirportId).HasColumnName("DepartureAirportID");

            entity.HasOne(d => d.ArrivalAirport).WithMany(p => p.RouteArrivalAirports)
                .HasForeignKey(d => d.ArrivalAirportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Routes_Airports3");

            entity.HasOne(d => d.DepartureAirport).WithMany(p => p.RouteDepartureAirports)
                .HasForeignKey(d => d.DepartureAirportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Routes_Airports2");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("schedules")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.AircraftId, "FK_Schedule_AirCraft");

            entity.HasIndex(e => e.RouteId, "FK_Schedule_Routes");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AircraftId).HasColumnName("AircraftID");
            entity.Property(e => e.EconomyPrice).HasPrecision(19, 4);
            entity.Property(e => e.FlightNumber)
                .HasMaxLength(10)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.RouteId).HasColumnName("RouteID");
            entity.Property(e => e.Time).HasMaxLength(6);

            entity.HasOne(d => d.Aircraft).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.AircraftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_AirCraft");

            entity.HasOne(d => d.Route).WithMany(p => p.Schedules)
                .HasForeignKey(d => d.RouteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedule_Routes");
        });

        modelBuilder.Entity<Survey>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("surveys")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FromAirport)
                .HasMaxLength(3)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.ToAirport)
                .HasMaxLength(3)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Sex)
                .HasMaxLength(1)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CabinType)
                .HasMaxLength(20)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("tickets")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.ScheduleId, "FK_Ticket_Schedule");

            entity.HasIndex(e => e.CabinTypeId, "FK_Ticket_TravelClass");

            entity.HasIndex(e => e.UserId, "FK_Ticket_User");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BookingReference)
                .HasMaxLength(6)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.CabinTypeId).HasColumnName("CabinTypeID");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.PassportCountryId).HasColumnName("PassportCountryID");
            entity.Property(e => e.PassportNumber)
                .HasMaxLength(9)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.Phone)
                .HasMaxLength(14)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.CabinType).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.CabinTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_TravelClass");

            entity.HasOne(d => d.Schedule).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_Schedule");

            entity.HasOne(d => d.User).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ticket_User");
        });

        modelBuilder.Entity<Token>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tokens");

            entity.Property(e => e.Id)
                .HasColumnName("id");
            entity.Property(e => e.AccessToken)
                .HasMaxLength(2048)
                .HasColumnName("accesstoken");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(512)
                .HasColumnName("refreshtoken");
            entity.Property(e => e.UserId)
                .HasColumnName("userid");
            entity.Property(e => e.RefreshTokenExpireDate)
                .HasColumnType("timestamp")
                .HasColumnName("refreshtokenexpiredate");

            entity.HasOne(d => d.User)
                .WithOne(p => p.Token)
                .HasForeignKey<Token>(t => t.UserId)
                .HasConstraintName("FK_Tokens_Users");
        });

        modelBuilder.Entity<Trackerrecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("trackerrecords");

            entity.HasIndex(e => e.Recordtype, "recordtype");

            entity.HasIndex(e => e.Userid, "userid");

            entity.HasIndex(e => e.Warntype, "warntype");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Datetime)
                .HasColumnType("timestamp")
                .HasColumnName("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("description");
            entity.Property(e => e.Recordtype).HasColumnName("recordtype");
            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Warntype).HasColumnName("warntype");

            entity.HasOne(d => d.RecordtypeNavigation).WithMany(p => p.Trackerrecords)
                .HasForeignKey(d => d.Recordtype)
                .HasConstraintName("trackerrecords_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.Trackerrecords)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("trackerrecords_ibfk_3");

            entity.HasOne(d => d.WarntypeNavigation).WithMany(p => p.Trackerrecords)
                .HasForeignKey(d => d.Warntype)
                .HasConstraintName("trackerrecords_ibfk_2");
        });

        modelBuilder.Entity<Trackerrecordtype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("trackerrecordtypes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("users")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_general_ci");

            entity.HasIndex(e => e.OfficeId, "FK_Users_Offices");

            entity.HasIndex(e => e.RoleId, "FK_Users_Roles");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.OfficeId).HasColumnName("OfficeID");
            entity.Property(e => e.Password)
                .HasMaxLength(512)
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Office).WithMany(p => p.Users)
                .HasForeignKey(d => d.OfficeId)
                .HasConstraintName("FK_Users_Offices");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });

        modelBuilder.Entity<Warntype>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("warntypes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
