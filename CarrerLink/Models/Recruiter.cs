using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarrerLink.Models
{
    public class Recruiter
    {
        [Key]
        public int RecruiterId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public string CompanyName { get; set; }
        public string CompanyWebsite { get; set; }
        public string CompanyDescription { get; set; }
        public string Location { get; set; }
        public string Industry { get; set; }

        // Navigation property (optional)
        public virtual User User { get; set; } = null!;
    }
}
