namespace RegionOneRecruitment.Models
{
    public class Opening
    {
        public int OpeningId { get; set; }
        public string? JobTitle { get; set; }
        public string? JobDescription { get; set; }
        public string? PayRate { get; set; }
        public string? SkillsRequired { get; set; }
        public string? CertificationsRequired { get; set; }
        public bool PositionFilled { get; set; }
        public string? PostedDate { get; set; }
    }
}
