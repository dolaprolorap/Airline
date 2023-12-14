using backend.DataAccess.Repository;
using UserDb = backend.Models.DB.User;

namespace backend.Models.API
{
    public class UserData
    {
        public string RoleName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Firstname { get; set; } = "";
        public string Lastname { get; set; } = "";
        public string OfficeName { get; set; } = "";
        public string Birthdate { get; set; } = "";
        public string IsActive { get; set; } = "";
    }
}
