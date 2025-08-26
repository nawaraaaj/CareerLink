namespace CarrerLink.Models
{
    public class Job
    {
        public int JobId { get; set; }  // Primary Key
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string JobType { get; set; }  
        public int Salary { get; set; }  
        public DateTime PostedDate { get; set; }
        public DateTime ApplicationDeadline { get; set; }

        public int RecruiterId { get; set; }  // Linked to Recruiter
        public Recruiter Recruiter { get; set; }  // Navigation Property
    }
}
