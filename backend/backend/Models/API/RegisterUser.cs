namespace backend.Models.API
{
    public class RegisterUser
    {
        public string? RoleName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string? OfficeName { get; set; }
        public string Birthdate { get; set; }
    }
}
