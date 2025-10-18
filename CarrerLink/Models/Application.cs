using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarrerLink.Models
{
    public class Application
    {
        [Key]
        public int ApplicationId { get; set; }

        [Required]
        [ForeignKey("Job")]
        public int JobId { get; set; }

        [Required]
        [ForeignKey("Applicant")]
        public int ApplicantId { get; set; }

        public DateTime AppliedAt { get; set; } = DateTime.Now;

        [MaxLength(50)]
        public string Status { get; set; } = "Pending";

        public virtual Job Job { get; set; } = null!;
        public virtual Applicant Applicant { get; set; } = null!;
    }
}
