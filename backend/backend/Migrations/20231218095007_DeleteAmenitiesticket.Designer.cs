﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using backend.DataAccess;

#nullable disable

namespace backend.Migrations
{
    [DbContext(typeof(AirlinesdbContext))]
    [Migration("20231218095007_DeleteAmenitiesticket")]
    partial class DeleteAmenitiesticket
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8mb4_0900_ai_ci")
                .HasAnnotation("ProductVersion", "7.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8mb4");

            modelBuilder.Entity("Amenitiescabintype", b =>
                {
                    b.Property<int>("CabinTypeId")
                        .HasColumnType("int")
                        .HasColumnName("CabinTypeID");

                    b.Property<int>("AmenityId")
                        .HasColumnType("int")
                        .HasColumnName("AmenityID");

                    b.HasKey("CabinTypeId", "AmenityId")
                        .HasName("PRIMARY")
                        .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                    b.HasIndex(new[] { "AmenityId" }, "FK_TravelClassAdditionalService_AdditionalService");

                    b.ToTable("amenitiescabintype", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("backend.Models.DB.Aircraft", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<int>("BusinessSeats")
                        .HasColumnType("int");

                    b.Property<int>("EconomySeats")
                        .HasColumnType("int");

                    b.Property<string>("MakeModel")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("MakeModel"), "utf8mb4");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Name"), "utf8mb4");

                    b.Property<int>("TotalSeats")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("aircrafts", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("backend.Models.DB.Airport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<int>("CountryId")
                        .HasColumnType("int")
                        .HasColumnName("CountryID");

                    b.Property<string>("Iatacode")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)")
                        .HasColumnName("IATACode");

                    b.Property<string>("Name")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Name"), "utf8mb4");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "CountryId" }, "FK_AirPort_Country");

                    b.ToTable("airports", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("backend.Models.DB.Amenitiesticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AmenityId")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("TicketId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AmenityId");

                    b.HasIndex("TicketId");

                    b.ToTable("Amenitiesticket");
                });

            modelBuilder.Entity("backend.Models.DB.Amenity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<decimal>("Price")
                        .HasPrecision(19, 4)
                        .HasColumnType("decimal(19,4)");

                    b.Property<string>("Service")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Service"), "utf8mb4");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("amenities", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("backend.Models.DB.Cabintype", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Name"), "utf8mb4");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("cabintypes", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("backend.Models.DB.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Name"), "utf8mb4");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("countries", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("backend.Models.DB.Office", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("varchar(250)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Contact"), "utf8mb4");

                    b.Property<int>("CountryId")
                        .HasColumnType("int")
                        .HasColumnName("CountryID");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Phone"), "utf8mb4");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Title"), "utf8mb4");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "CountryId" }, "FK_Office_Country");

                    b.ToTable("offices", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("backend.Models.DB.Role", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Title"), "utf8mb4");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("roles", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("backend.Models.DB.Route", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<int>("ArrivalAirportId")
                        .HasColumnType("int")
                        .HasColumnName("ArrivalAirportID");

                    b.Property<int>("DepartureAirportId")
                        .HasColumnType("int")
                        .HasColumnName("DepartureAirportID");

                    b.Property<int>("Distance")
                        .HasColumnType("int");

                    b.Property<int>("FlightTime")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "DepartureAirportId" }, "FK_Routes_Airports2");

                    b.HasIndex(new[] { "ArrivalAirportId" }, "FK_Routes_Airports3");

                    b.ToTable("routes", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("backend.Models.DB.Schedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<int>("AircraftId")
                        .HasColumnType("int")
                        .HasColumnName("AircraftID");

                    b.Property<bool>("Confirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<decimal>("EconomyPrice")
                        .HasPrecision(19, 4)
                        .HasColumnType("decimal(19,4)");

                    b.Property<string>("FlightNumber")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("FlightNumber"), "utf8mb4");

                    b.Property<int>("RouteId")
                        .HasColumnType("int")
                        .HasColumnName("RouteID");

                    b.Property<TimeOnly>("Time")
                        .HasMaxLength(6)
                        .HasColumnType("time(6)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "AircraftId" }, "FK_Schedule_AirCraft");

                    b.HasIndex(new[] { "RouteId" }, "FK_Schedule_Routes");

                    b.ToTable("schedules", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("backend.Models.DB.Survey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<int?>("Age")
                        .HasColumnType("int");

                    b.Property<string>("CabinType")
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("CabinType"), "utf8mb4");

                    b.Property<string>("FromAirport")
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("FromAirport"), "utf8mb4");

                    b.Property<int>("Q1")
                        .HasColumnType("int");

                    b.Property<int>("Q2")
                        .HasColumnType("int");

                    b.Property<int>("Q3")
                        .HasColumnType("int");

                    b.Property<int>("Q4")
                        .HasColumnType("int");

                    b.Property<string>("Sex")
                        .HasMaxLength(1)
                        .HasColumnType("varchar(1)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Sex"), "utf8mb4");

                    b.Property<string>("ToAirport")
                        .HasMaxLength(3)
                        .HasColumnType("varchar(3)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("ToAirport"), "utf8mb4");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("surveys", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("backend.Models.DB.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<string>("BookingReference")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("varchar(6)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("BookingReference"), "utf8mb4");

                    b.Property<int>("CabinTypeId")
                        .HasColumnType("int")
                        .HasColumnName("CabinTypeID");

                    b.Property<bool>("Confirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Firstname"), "utf8mb4");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Lastname"), "utf8mb4");

                    b.Property<int>("PassportCountryId")
                        .HasColumnType("int")
                        .HasColumnName("PassportCountryID");

                    b.Property<string>("PassportNumber")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("varchar(9)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("PassportNumber"), "utf8mb4");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(14)
                        .HasColumnType("varchar(14)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Phone"), "utf8mb4");

                    b.Property<int>("ScheduleId")
                        .HasColumnType("int")
                        .HasColumnName("ScheduleID");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("UserID");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "ScheduleId" }, "FK_Ticket_Schedule");

                    b.HasIndex(new[] { "CabinTypeId" }, "FK_Ticket_TravelClass");

                    b.HasIndex(new[] { "UserId" }, "FK_Ticket_User");

                    b.ToTable("tickets", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("backend.Models.DB.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("AccessToken")
                        .HasMaxLength(2048)
                        .HasColumnType("varchar(2048)")
                        .HasColumnName("accesstoken");

                    b.Property<string>("RefreshToken")
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)")
                        .HasColumnName("refreshtoken");

                    b.Property<DateTime?>("RefreshTokenExpireDate")
                        .HasColumnType("timestamp")
                        .HasColumnName("refreshtokenexpiredate");

                    b.Property<int>("UserId")
                        .HasColumnType("int")
                        .HasColumnName("userid");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("tokens", (string)null);
                });

            modelBuilder.Entity("backend.Models.DB.Trackerrecord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<DateTime?>("Datetime")
                        .HasColumnType("timestamp")
                        .HasColumnName("datetime");

                    b.Property<string>("Description")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)")
                        .HasColumnName("description");

                    b.Property<int?>("Recordtype")
                        .HasColumnType("int")
                        .HasColumnName("recordtype");

                    b.Property<int?>("Userid")
                        .HasColumnType("int")
                        .HasColumnName("userid");

                    b.Property<int?>("Warntype")
                        .HasColumnType("int")
                        .HasColumnName("warntype");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Recordtype" }, "recordtype");

                    b.HasIndex(new[] { "Userid" }, "userid");

                    b.HasIndex(new[] { "Warntype" }, "warntype");

                    b.ToTable("trackerrecords", (string)null);
                });

            modelBuilder.Entity("backend.Models.DB.Trackerrecordtype", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("trackerrecordtypes", (string)null);
                });

            modelBuilder.Entity("backend.Models.DB.User", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int")
                        .HasColumnName("ID");

                    b.Property<bool?>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateOnly?>("Birthdate")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("varchar(150)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Email"), "utf8mb4");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("FirstName"), "utf8mb4");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("LastName"), "utf8mb4");

                    b.Property<int?>("OfficeId")
                        .HasColumnType("int")
                        .HasColumnName("OfficeID");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("varchar(512)")
                        .UseCollation("utf8mb4_0900_ai_ci");

                    MySqlPropertyBuilderExtensions.HasCharSet(b.Property<string>("Password"), "utf8mb4");

                    b.Property<int>("RoleId")
                        .HasColumnType("int")
                        .HasColumnName("RoleID");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "OfficeId" }, "FK_Users_Offices");

                    b.HasIndex(new[] { "RoleId" }, "FK_Users_Roles");

                    b.ToTable("users", (string)null);

                    MySqlEntityTypeBuilderExtensions.HasCharSet(b, "utf8mb3");
                    MySqlEntityTypeBuilderExtensions.UseCollation(b, "utf8mb3_general_ci");
                });

            modelBuilder.Entity("backend.Models.DB.Warntype", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("id");

                    b.Property<string>("Name")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.ToTable("warntypes", (string)null);
                });

            modelBuilder.Entity("Amenitiescabintype", b =>
                {
                    b.HasOne("backend.Models.DB.Amenity", null)
                        .WithMany()
                        .HasForeignKey("AmenityId")
                        .IsRequired()
                        .HasConstraintName("FK_TravelClassAdditionalService_AdditionalService");

                    b.HasOne("backend.Models.DB.Cabintype", null)
                        .WithMany()
                        .HasForeignKey("CabinTypeId")
                        .IsRequired()
                        .HasConstraintName("FK_TravelClassAdditionalService_TravelClass");
                });

            modelBuilder.Entity("backend.Models.DB.Airport", b =>
                {
                    b.HasOne("backend.Models.DB.Country", "Country")
                        .WithMany("Airports")
                        .HasForeignKey("CountryId")
                        .IsRequired()
                        .HasConstraintName("FK_AirPort_Country");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("backend.Models.DB.Amenitiesticket", b =>
                {
                    b.HasOne("backend.Models.DB.Amenity", "Amenity")
                        .WithMany("Amenitiestickets")
                        .HasForeignKey("AmenityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend.Models.DB.Ticket", "Ticket")
                        .WithMany("Amenitiestickets")
                        .HasForeignKey("TicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Amenity");

                    b.Navigation("Ticket");
                });

            modelBuilder.Entity("backend.Models.DB.Office", b =>
                {
                    b.HasOne("backend.Models.DB.Country", "Country")
                        .WithMany("Offices")
                        .HasForeignKey("CountryId")
                        .IsRequired()
                        .HasConstraintName("FK_Office_Country");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("backend.Models.DB.Route", b =>
                {
                    b.HasOne("backend.Models.DB.Airport", "ArrivalAirport")
                        .WithMany("RouteArrivalAirports")
                        .HasForeignKey("ArrivalAirportId")
                        .IsRequired()
                        .HasConstraintName("FK_Routes_Airports3");

                    b.HasOne("backend.Models.DB.Airport", "DepartureAirport")
                        .WithMany("RouteDepartureAirports")
                        .HasForeignKey("DepartureAirportId")
                        .IsRequired()
                        .HasConstraintName("FK_Routes_Airports2");

                    b.Navigation("ArrivalAirport");

                    b.Navigation("DepartureAirport");
                });

            modelBuilder.Entity("backend.Models.DB.Schedule", b =>
                {
                    b.HasOne("backend.Models.DB.Aircraft", "Aircraft")
                        .WithMany("Schedules")
                        .HasForeignKey("AircraftId")
                        .IsRequired()
                        .HasConstraintName("FK_Schedule_AirCraft");

                    b.HasOne("backend.Models.DB.Route", "Route")
                        .WithMany("Schedules")
                        .HasForeignKey("RouteId")
                        .IsRequired()
                        .HasConstraintName("FK_Schedule_Routes");

                    b.Navigation("Aircraft");

                    b.Navigation("Route");
                });

            modelBuilder.Entity("backend.Models.DB.Ticket", b =>
                {
                    b.HasOne("backend.Models.DB.Cabintype", "CabinType")
                        .WithMany("Tickets")
                        .HasForeignKey("CabinTypeId")
                        .IsRequired()
                        .HasConstraintName("FK_Ticket_TravelClass");

                    b.HasOne("backend.Models.DB.Schedule", "Schedule")
                        .WithMany("Tickets")
                        .HasForeignKey("ScheduleId")
                        .IsRequired()
                        .HasConstraintName("FK_Ticket_Schedule");

                    b.HasOne("backend.Models.DB.User", "User")
                        .WithMany("Tickets")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK_Ticket_User");

                    b.Navigation("CabinType");

                    b.Navigation("Schedule");

                    b.Navigation("User");
                });

            modelBuilder.Entity("backend.Models.DB.Token", b =>
                {
                    b.HasOne("backend.Models.DB.User", "User")
                        .WithOne("Token")
                        .HasForeignKey("backend.Models.DB.Token", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Tokens_Users");

                    b.Navigation("User");
                });

            modelBuilder.Entity("backend.Models.DB.Trackerrecord", b =>
                {
                    b.HasOne("backend.Models.DB.Trackerrecordtype", "RecordtypeNavigation")
                        .WithMany("Trackerrecords")
                        .HasForeignKey("Recordtype")
                        .HasConstraintName("trackerrecords_ibfk_1");

                    b.HasOne("backend.Models.DB.User", "User")
                        .WithMany("Trackerrecords")
                        .HasForeignKey("Userid")
                        .HasConstraintName("trackerrecords_ibfk_3");

                    b.HasOne("backend.Models.DB.Warntype", "WarntypeNavigation")
                        .WithMany("Trackerrecords")
                        .HasForeignKey("Warntype")
                        .HasConstraintName("trackerrecords_ibfk_2");

                    b.Navigation("RecordtypeNavigation");

                    b.Navigation("User");

                    b.Navigation("WarntypeNavigation");
                });

            modelBuilder.Entity("backend.Models.DB.User", b =>
                {
                    b.HasOne("backend.Models.DB.Office", "Office")
                        .WithMany("Users")
                        .HasForeignKey("OfficeId")
                        .HasConstraintName("FK_Users_Offices");

                    b.HasOne("backend.Models.DB.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .IsRequired()
                        .HasConstraintName("FK_Users_Roles");

                    b.Navigation("Office");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("backend.Models.DB.Aircraft", b =>
                {
                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("backend.Models.DB.Airport", b =>
                {
                    b.Navigation("RouteArrivalAirports");

                    b.Navigation("RouteDepartureAirports");
                });

            modelBuilder.Entity("backend.Models.DB.Amenity", b =>
                {
                    b.Navigation("Amenitiestickets");
                });

            modelBuilder.Entity("backend.Models.DB.Cabintype", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("backend.Models.DB.Country", b =>
                {
                    b.Navigation("Airports");

                    b.Navigation("Offices");
                });

            modelBuilder.Entity("backend.Models.DB.Office", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("backend.Models.DB.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("backend.Models.DB.Route", b =>
                {
                    b.Navigation("Schedules");
                });

            modelBuilder.Entity("backend.Models.DB.Schedule", b =>
                {
                    b.Navigation("Tickets");
                });

            modelBuilder.Entity("backend.Models.DB.Ticket", b =>
                {
                    b.Navigation("Amenitiestickets");
                });

            modelBuilder.Entity("backend.Models.DB.Trackerrecordtype", b =>
                {
                    b.Navigation("Trackerrecords");
                });

            modelBuilder.Entity("backend.Models.DB.User", b =>
                {
                    b.Navigation("Tickets");

                    b.Navigation("Token")
                        .IsRequired();

                    b.Navigation("Trackerrecords");
                });

            modelBuilder.Entity("backend.Models.DB.Warntype", b =>
                {
                    b.Navigation("Trackerrecords");
                });
#pragma warning restore 612, 618
        }
    }
}
