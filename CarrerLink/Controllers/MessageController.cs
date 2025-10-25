using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarrerLink.Data;
using CarrerLink.Models;
using System;
using System.Threading.Tasks;
using System.Security.Claims;

namespace CarrerLink.Controllers
{
    [Authorize]
    public class MessageController : Controller
    {
        private readonly CarrerLinkContext _context;

        public MessageController(CarrerLinkContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Create(int applicantId)
        {
            var applicant = await _context.Applicant
                .Include(a => a.User)
                .FirstOrDefaultAsync(a => a.ApplicantId == applicantId);

            if (applicant == null)
            {
                ViewBag.ApplicantName = "Unknown Applicant";
                ViewBag.ApplicantId = 0;
            }
            else
            {
                ViewBag.ApplicantName = applicant.User?.Name ?? "Unknown Applicant";
                ViewBag.ApplicantId = applicant.ApplicantId;
            }

            return View(new MessageViewModel { ApplicantId = applicantId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MessageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await LoadApplicantInfo(model.ApplicantId);
                return View(model);
            }

            try
            {
                var recruiterIdClaim = User.FindFirst("RecruiterId");
                if (recruiterIdClaim == null || !int.TryParse(recruiterIdClaim.Value, out int recruiterId))
                {
                    ModelState.AddModelError("", "Recruiter not found.");
                    await LoadApplicantInfo(model.ApplicantId);
                    return View(model);
                }

                var recruiter = await _context.Recruiter
                    .FirstOrDefaultAsync(r => r.RecruiterId == recruiterId);

                if (recruiter == null)
                {
                    ModelState.AddModelError("", "Recruiter not found.");
                    await LoadApplicantInfo(model.ApplicantId);
                    return View(model);
                }
                var applicantExists = await _context.Applicant
                    .AnyAsync(a => a.ApplicantId == model.ApplicantId);

                if (!applicantExists)
                {
                    ModelState.AddModelError("", "Applicant not found.");
                    await LoadApplicantInfo(model.ApplicantId);
                    return View(model);
                }
                var message = new Message
                {
                    RecruiterId = recruiter.RecruiterId,
                    ApplicantId = model.ApplicantId,
                    Content = model.Content?.Trim() ?? string.Empty,
                    SentDate = DateTime.Now
                };

                _context.Message.Add(message);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Message sent successfully!";
                return RedirectToAction(nameof(Create), new { applicantId = model.ApplicantId });
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "An error occurred while sending the message.");
                await LoadApplicantInfo(model.ApplicantId);
                return View(model);
            }
        }

        private async Task LoadApplicantInfo(int applicantId)
        {
            if (applicantId > 0)
            {
                var applicant = await _context.Applicant
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.ApplicantId == applicantId);

                ViewBag.ApplicantName = applicant?.User?.Name ?? "Unknown Applicant";
                ViewBag.ApplicantId = applicant?.ApplicantId ?? 0;
            }
            else
            {
                ViewBag.ApplicantName = "Unknown Applicant";
                ViewBag.ApplicantId = 0;
            }
        }
    }
}