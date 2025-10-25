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

        public virtual User User { get; set; } = null!; 
        public virtual ICollection<Job> Jobs { get; set; } = new List<Job>();
        public ICollection<Message> MessagesSent { get; set; }

    }
}
