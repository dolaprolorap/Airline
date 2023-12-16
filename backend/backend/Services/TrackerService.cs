using backend.DataAccess.Repository;
using backend.Models.API;
using backend.ServerResponse;
using backend.ServerResponse.Services.TrackerService;
using Microsoft.EntityFrameworkCore;
using Tracker = backend.Models.DB.Trackerrecord;

namespace backend.Services
{
    public class TrackerService : ITrackerService
    {
        private IUnitOfWork _unit;

        public TrackerService(IUnitOfWork unitOfWork) 
        {
            _unit = unitOfWork;
        }

        private StatusResponse _createBasicTrackerRecord(string email, string trackerName, string warnTypeName, string desc)
        {
            var qUser = _unit.UserRepo.ReadWhere(x => x.Email == email);
            if (!qUser.Any())
            {
                return new TrackerResponse(TrackerResponseType.UserNotFound);
            }

            var qTrackerrecordType = _unit.TrackerrecordtypeRepo.ReadWhere(x => x.Name == trackerName);
            if (!qTrackerrecordType.Any())
            {
                return new TrackerResponse(TrackerResponseType.TrackerTypeNotFound);
            }

            var qWarntype = _unit.WarntypeRepo.ReadWhere(x => x.Name == warnTypeName);
            if (!qWarntype.Any())
            {
                return new TrackerResponse(TrackerResponseType.WarnTypeNotFound);
            }

            int lastIdTr = 0;
            var qAllRecords = _unit.TrackerrecordRepo.ReadAll();
            if (qAllRecords.Any()) lastIdTr = qAllRecords.Max(x => x.Id);
            
            var user = qUser.First();
            var trackerrecordType = qTrackerrecordType.First();
            var warnType = qWarntype.First();

            Tracker tracker = new Tracker()
            {
                Id = lastIdTr + 1,
                Recordtype = trackerrecordType.Id,
                Warntype = warnType.Id,
                Datetime = DateTime.Now,
                Description = desc,
                Userid = user.Id
            };

            _unit.TrackerrecordRepo.Add(tracker);

            _unit.Save();

            return new TrackerResponse(TrackerResponseType.Ok);
        }

        public StatusResponse SuccessLogin(string email)
        {
            return _createBasicTrackerRecord(email, "login", "inf", "");
        }

        public StatusResponse UnsuccessTryLogin(string email, string reason) 
        {
            return _createBasicTrackerRecord(email, "login", "err", reason);
        }

        public StatusResponse SuccessLogout(string email)
        {
            return _createBasicTrackerRecord(email, "logout", "inf", "");
        }

        public StatusResponse UnsuccessTryLogout(string email, string reason) 
        {
            return _createBasicTrackerRecord(email, "logout", "err", reason);
        }

        public StatusResponse GetByEmail(string email) 
        { 
            var queryableUser = _unit.UserRepo.ReadWhere(u => u.Email == email);
            if (!queryableUser.Any())
            {
                return new GetByEmailResponse(
                    GetByEmailResponseType.UserNotFound, 
                    email);
            }

            queryableUser.Load();

            _unit.WarntypeRepo.ReadWhere(wt => true).Load();
            _unit.TrackerrecordtypeRepo.ReadWhere(wt => true).Load();

            var qRecords = _unit.TrackerrecordRepo.ReadWhere(t => t.User!.Email == email).ToList().Select((t, i) => new TrackerRecord(t));

            return new GetByEmailResponse(
                GetByEmailResponseType.RecordsGotten, 
                email, 
                qRecords);
        }
    }
}
