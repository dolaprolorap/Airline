using backend.Models.DB;
using Route = backend.Models.DB.Route;

namespace backend.DataAccess.Repository
{
    public interface IUnitOfWork
    {
        IRepository<Aircraft> AircraftRepo { get; }
        IRepository<Airport> AirportRepo { get; }
        IRepository<Amenitiesticket> AmenitiesticketRepo { get; }
        IRepository<Amenity> AmenityRepo { get; }
        IRepository<Cabintype> CabintypeRepo { get; }
        IRepository<Country> CountryRepo { get; }
        IRepository<Office> OfficeRepo { get; }
        IRepository<Role> RoleRepo { get; }
        IRepository<Route> RouteRepo { get; }
        IRepository<Schedule> ScheduleRepo { get; }
        IRepository<Ticket> TicketRepo { get; }
        IRepository<Token> TokenRepo { get; }
        IRepository<Trackerrecord> TrackerrecordRepo { get; }
        IRepository<Trackerrecordtype> TrackerrecordtypeRepo { get; }
        IRepository<User> UserRepo { get; }
        IRepository<Warntype> WarntypeRepo { get; }

        void Save();
    }
}
