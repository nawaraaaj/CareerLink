namespace CarrerLink.Models
{
    public class User
    {
        public int Id { get; set; }

        // Common fields
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } 
        public string PhoneNumber { get; set; }
        public string UserType { get; set; } // "Applicant" or "Recruiter"

        // Applicant-specific fields
        public DateTime? DateOfBirth { get; set; }
        public string Address { get; set; }
        public string ResumeUrl { get; set; }
        public string Skills { get; set; } 
        public string ProfilePictureUrl { get; set; }
        public string Experience { get; set; }
        public string Education { get; set; }
        public string Certifications { get; set; }
        public string PortfolioUrl { get; set; }

        // Recruiter-specific fields
        public string CompanyName { get; set; }
        public string CompanyWebsite { get; set; }
        public string CompanyDescription { get; set; }
        public string CompanyLogoUrl { get; set; }
        public string Location { get; set; }
        public string Industry { get; set; }
    }
}
