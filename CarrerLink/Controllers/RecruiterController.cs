using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CarrerLink.Data;
using CarrerLink.Models;

namespace CarrerLink.Controllers
{
    public class RecruiterController : Controller
    {
        private readonly CarrerLinkContext _context;

        public RecruiterController(CarrerLinkContext context)
        {
            _context = context;
        }

        // GET: Recruiter
        public async Task<IActionResult> Index()
        {
            var carrerLinkContext = _context.Recruiter.Include(r => r.User);
            return View(await carrerLinkContext.ToListAsync());
        }

        // GET: Recruiter/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recruiter = await _context.Recruiter
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RecruiterId == id);
            if (recruiter == null)
            {
                return NotFound();
            }

            return View(recruiter);
        }

        // GET: Recruiter/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id");
            return View();
        }

        // POST: Recruiter/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecruiterId,UserId,CompanyName,CompanyWebsite,CompanyDescription,Location,Industry")] Recruiter recruiter)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recruiter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", recruiter.UserId);
            return View(recruiter);
        }

        // GET: Recruiter/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recruiter = await _context.Recruiter.FindAsync(id);
            if (recruiter == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", recruiter.UserId);
            return View(recruiter);
        }

        // POST: Recruiter/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecruiterId,UserId,CompanyName,CompanyWebsite,CompanyDescription,Location,Industry")] Recruiter recruiter)
        {
            if (id != recruiter.RecruiterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recruiter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecruiterExists(recruiter.RecruiterId))
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
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", recruiter.UserId);
            return View(recruiter);
        }

        // GET: Recruiter/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recruiter = await _context.Recruiter
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RecruiterId == id);
            if (recruiter == null)
            {
                return NotFound();
            }

            return View(recruiter);
        }

        // POST: Recruiter/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recruiter = await _context.Recruiter.FindAsync(id);
            if (recruiter != null)
            {
                _context.Recruiter.Remove(recruiter);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecruiterExists(int id)
        {
            return _context.Recruiter.Any(e => e.RecruiterId == id);
        }
    }
}
