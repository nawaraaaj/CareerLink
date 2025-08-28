using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarrerLink.Data;
using CarrerLink.Models;
using System.Security.Claims;


namespace CarrerLink.Controllers
{
    public class JobController : Controller
    {
        private readonly CarrerLinkContext _context;

        public JobController(CarrerLinkContext context)
        {
            _context = context;
        }

        // GET: Job
        public async Task<IActionResult> Index()
        {
            var carrerLinkContext = _context.Job.Include(j => j.Recruiter);
            return View(await carrerLinkContext.ToListAsync());
        }

        // GET: Job/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Job
                .Include(j => j.Recruiter)
                .FirstOrDefaultAsync(m => m.JobId == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // GET: Job/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Job/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Job job)
        {
            // Remove validation errors for navigation properties
            ModelState.Remove("Recruiter");

            if (!ModelState.IsValid)
            {
                return View(job);
            }

            var recruiterIdClaim = User.FindFirst("RecruiterId")?.Value;
            if (recruiterIdClaim == null)
                return Forbid();

            job.RecruiterId = int.Parse(recruiterIdClaim);
            job.PostedDate = DateTime.Now;

            job.ApplicationDeadline = new DateTime(job.ApplicationDeadline.Year, job.ApplicationDeadline.Month, job.ApplicationDeadline.Day, 23, 59, 59);

            _context.Job.Add(job);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Job/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Job.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }
            ViewData["RecruiterId"] = new SelectList(_context.Recruiter, "RecruiterId", "RecruiterId", job.RecruiterId);
            return View(job);
        }

        // POST: Job/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JobId,Title,Description,Location,JobType,Salary,PostedDate,ApplicationDeadline,RecruiterId")] Job job)
        {
            if (id != job.JobId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(job);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobExists(job.JobId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RecruiterId"] = new SelectList(_context.Recruiter, "RecruiterId", "RecruiterId", job.RecruiterId);
            return View(job);
        }

        // GET: Job/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Job
                .Include(j => j.Recruiter)
                .FirstOrDefaultAsync(m => m.JobId == id);
            if (job == null)
            {
                return NotFound();
            }

            return View(job);
        }

        // POST: Job/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var job = await _context.Job.FindAsync(id);
            if (job != null)
            {
                _context.Job.Remove(job);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobExists(int id)
        {
            return _context.Job.Any(e => e.JobId == id);
        }
    }
}
