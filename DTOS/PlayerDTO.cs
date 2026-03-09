namespace hateekub.DTOS
{
    public class PlayerDTO
    {
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string? RoleName { get; set; }
        public string? RankName { get; set; }
        public string? UserProfile { get; set; }

        public string? Avatar { get; set; }

        public bool Status { get; set; }
    }
}   