namespace CarrerLink.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } 
        public string Mobile { get; set; }
        public string UserType { get; set; } // "Applicant" or "Recruiter"

        public virtual Applicant? Applicant { get; set; }
        public virtual Recruiter? Recruiter { get; set; }
    }
}
