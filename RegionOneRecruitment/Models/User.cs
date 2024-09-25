namespace RegionOneRecruitment.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? JobInterests { get; set; }
        public string? Password { get; set; }
        public byte[]? ResumeBytes { get; set; }
        public bool IsAdmin { get; set; }
    }
}
