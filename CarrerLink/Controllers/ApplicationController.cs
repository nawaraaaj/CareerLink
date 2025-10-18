using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarrerLink.Data;
using CarrerLink.Models;

namespace CarrerLink.Controllers
{
    [Authorize] 
    public class ApplicationController : Controller
    {
        private readonly CarrerLinkContext _context;

        public ApplicationController(CarrerLinkContext context)
        {
            _context = context;
        }

        // applicant apply to job

        [Authorize(Roles = "Applicant")]
        [HttpPost]
        public async Task<IActionResult> Apply(int jobId)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var applicant = await _context.Applicant
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (applicant == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // check for duplicate application
            var existing = await _context.Application
                .FirstOrDefaultAsync(a => a.JobId == jobId && a.ApplicantId == applicant.ApplicantId);

            if (existing != null)
            {
                TempData["Error"] = "You have already applied for this job.";
                return RedirectToAction("Details", "Job", new { id = jobId });
            }

            var application = new Application
            {
                JobId = jobId,
                ApplicantId = applicant.ApplicantId,
                Status = "Pending",
                AppliedAt = DateTime.Now
            };

            _context.Application.Add(application);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Application submitted successfully!";
            return RedirectToAction("Details", "Job", new { id = jobId });
        }


        // applicant viewing their applications
        [Authorize(Roles = "Applicant")]
        public async Task<IActionResult> MyApplications()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var applicant = await _context.Applicant
                .FirstOrDefaultAsync(a => a.UserId == userId);

            if (applicant == null)
                return RedirectToAction("Login", "Account");

            var applications = await _context.Application
                .Include(a => a.Job)
                .Where(a => a.ApplicantId == applicant.ApplicantId)
                .ToListAsync();

            return View(applications);
        }

        // recruiter viewing applicant applying for job
        [Authorize(Roles = "Recruiter")]
        public async Task<IActionResult> ApplicantsList(int jobId)
        {
            var job = await _context.Job.FirstOrDefaultAsync(j => j.JobId == jobId);
            if (job == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (job.RecruiterId != userId)
                return Forbid();

            var applications = await _context.Application
                .Include(a => a.Applicant)
                    .ThenInclude(ap => ap.User)
                .Where(a => a.JobId == jobId)
                .ToListAsync();

            return View(applications);
        }

        // application status
        [Authorize(Roles = "Recruiter")]
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int applicationId, string status)
        {
            var application = await _context.Application
                .Include(a => a.Job)
                .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

            if (application == null)
                return NotFound();

            var userId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            if (application.Job.RecruiterId != userId)
                return Forbid();

            application.Status = status;
            await _context.SaveChangesAsync();

            return RedirectToAction("ApplicantsList", new { jobId = application.JobId });
        }
    }
}
