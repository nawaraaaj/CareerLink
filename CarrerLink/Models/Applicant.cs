using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarrerLink.Models
{
    public class Applicant
    {
        [Key]
        public int ApplicantId { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public string ResumePath { get; set; }
        public string Skills { get; set; }
        public string? ProfilePicturePath { get; set; }
        public string Experience { get; set; }
        public string Education { get; set; }
        public string PortfolioUrl { get; set; }


        public virtual User User { get; set; } = null!;
    }
}
