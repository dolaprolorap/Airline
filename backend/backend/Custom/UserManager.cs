using backend.DataAccess.Repository;
using backend.ServerResponse.Custom.UserManager;
using UserData = backend.Models.API.UserData;
using UserDb = backend.Models.DB.User;

namespace backend.Custom
{
    public class UserManager
    {
        private IUnitOfWork _unit;

        public UserManager(IUnitOfWork unit) 
        {
            _unit = unit;
        }

        public ConvertUserResponse ConvertUser(UserDb userDb)
        {
            var role = _unit.RoleRepo.ReadFirst(role => role.Id == userDb.RoleId);
            var office = _unit.OfficeRepo.ReadFirst(office => office.Id == userDb.OfficeId);

            if (role == null)
            {
                return new ConvertUserResponse(
                    ConvertUserResponseType.RoleNotFound,
                    roleId: userDb.RoleId);
            }

            if (office == null)
            {
                return new ConvertUserResponse(
                    ConvertUserResponseType.OfficeNotFound,
                    officeId: userDb.OfficeId);
            }

            var userData = new UserData()
            {
                RoleName = role.Title,
                Email = userDb.Email,
                Firstname = userDb.FirstName,
                Lastname = userDb.LastName,
                OfficeName = office.Title,
                Birthdate = userDb.Birthdate.ToString(),
                IsActive = userDb.Active.ToString()
            };

            return new ConvertUserResponse(
                ConvertUserResponseType.Ok, 
                user: userData);
        }
    }
}
