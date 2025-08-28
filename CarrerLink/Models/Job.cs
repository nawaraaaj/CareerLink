using System.ComponentModel.DataAnnotations;

namespace CarrerLink.Models
{
    public class Job
    {
        public int JobId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        public string Location { get; set; }
        public string JobType { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Salary must be a positive number")]
        public int Salary { get; set; }

        public DateTime PostedDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Application deadline is required")]
        public DateTime ApplicationDeadline { get; set; }

        public int RecruiterId { get; set; }
        public virtual Recruiter? Recruiter { get; set; }
    }
}