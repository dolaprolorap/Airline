namespace backend.Models.DB
{
    public class Token
    {
        public int Id { get; set; }

        public string? AccessToken { get; set; }

        public string? RefreshToken { get; set; }

        public int UserId { get; set; }

        public DateTime? RefreshTokenExpireDate { get; set; }

        public virtual User User { get; set; } = null!;

    }
}
