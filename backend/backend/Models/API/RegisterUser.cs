namespace backend.Models.API
{
    public class RegisterUser
    {
        public int RoleID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int OfficeID { get; set; }
        public string Birthdate { get; set; }
    }
}
