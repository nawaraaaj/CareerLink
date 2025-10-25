using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CarrerLink.Models
{
    public class MessageViewModel
    {
        [Required]
        public int ApplicantId { get; set; }

        [Required]
        [Display(Name = "Message Content")]
        public string Content { get; set; } = string.Empty;
    }
}