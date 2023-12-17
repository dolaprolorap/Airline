using backend.DataAccess;
using backend.Models.DB;
using Route = backend.Models.DB.Route;

namespace backend.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AirlinesdbContext _db;
        public IRepository<Aircraft> AircraftRepo { get; private set; }
        public IRepository<Airport> AirportRepo { get; private set; }
        public IRepository<Amenitiesticket> AmenitiesticketRepo { get; private set; }
        public IRepository<Amenity> AmenityRepo { get; private set; }
        public IRepository<Cabintype> CabintypeRepo { get; private set; }
        public IRepository<Country> CountryRepo { get; private set; }
        public IRepository<Office> OfficeRepo { get; private set; }
        public IRepository<Role> RoleRepo { get; private set; }
        public IRepository<Route> RouteRepo { get; private set; }
        public IRepository<Schedule> ScheduleRepo { get; private set; }
        public IRepository<Survey> SurveyRepo { get; private set; }
        public IRepository<Ticket> TicketRepo { get; private set; }
        public IRepository<Token> TokenRepo { get; private set; }
        public IRepository<Trackerrecord> TrackerrecordRepo { get; private set; }
        public IRepository<Trackerrecordtype> TrackerrecordtypeRepo { get; private set; }
        public IRepository<User> UserRepo { get; private set; }
        public IRepository<Warntype> WarntypeRepo { get; private set; }

        public UnitOfWork(AirlinesdbContext db)
        {
            _db = db;
            AircraftRepo = new Repository<Aircraft>(db);
            AirportRepo = new Repository<Airport>(db);
            AmenitiesticketRepo = new Repository<Amenitiesticket>(db);
            AmenityRepo = new Repository<Amenity>(db);
            CabintypeRepo = new Repository<Cabintype>(db);
            CountryRepo = new Repository<Country>(db);
            OfficeRepo = new Repository<Office>(db);
            RoleRepo = new Repository<Role>(db);
            RouteRepo = new Repository<Route>(db);
            ScheduleRepo = new Repository<Schedule>(db);
            SurveyRepo = new Repository<Survey>(db);
            TicketRepo = new Repository<Ticket>(db);
            TokenRepo = new Repository<Token>(db);
            TrackerrecordRepo = new Repository<Trackerrecord>(db);
            TrackerrecordtypeRepo = new Repository<Trackerrecordtype>(db);
            UserRepo = new Repository<User>(db);
            WarntypeRepo = new Repository<Warntype>(db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
