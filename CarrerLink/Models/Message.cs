namespace CarrerLink.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public int RecruiterId { get; set; }
        public int ApplicantId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentDate { get; set; }

        public virtual Recruiter? Recruiter { get; set; }
        public virtual Applicant? Applicant { get; set; }
    }
}