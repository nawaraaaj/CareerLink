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
    public class ApplicantController : Controller
    {
        private readonly CarrerLinkContext _context;

        public ApplicantController(CarrerLinkContext context)
        {
            _context = context;
        }

        // GET: Applicant
        public async Task<IActionResult> Index()
        {
            var carrerLinkContext = _context.Applicant.Include(a => a.User);
            return View(await carrerLinkContext.ToListAsync());
        }

        // GET: Applicant/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicant = await _context.Applicant
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.ApplicantId == id);
            if (applicant == null)
            {
                return NotFound();
            }

            return View(applicant);
        }

        // GET: Applicant/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Applicant/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ApplicantId,ResumePath,Skills,ProfilePicturePath,Experience,Education,PortfolioUrl")] Applicant applicant)
        {
            var userIdClaim = User.FindFirst("UserId");
            applicant.UserId = int.Parse(userIdClaim.Value);

            if(ModelState.IsValid)
            {
                _context.Applicant.Add(applicant);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login", "User");
                
                
            }
            return View(applicant);
        }
        

        // GET: Applicant/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicant = await _context.Applicant.FindAsync(id);
            if (applicant == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", applicant.UserId);
            return View(applicant);
        }

        // POST: Applicant/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ApplicantId,UserId,ResumePath,Skills,ProfilePicturePath,Experience,Education,PortfolioUrl")] Applicant applicant)
        {
            if (id != applicant.ApplicantId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(applicant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ApplicantExists(applicant.ApplicantId))
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
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Id", applicant.UserId);
            return View(applicant);
        }

        // GET: Applicant/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var applicant = await _context.Applicant
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.ApplicantId == id);
            if (applicant == null)
            {
                return NotFound();
            }

            return View(applicant);
        }

        // POST: Applicant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var applicant = await _context.Applicant.FindAsync(id);
            if (applicant != null)
            {
                _context.Applicant.Remove(applicant);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ApplicantExists(int id)
        {
            return _context.Applicant.Any(e => e.ApplicantId == id);
        }
    }
}
