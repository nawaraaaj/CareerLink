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
        // POST: Applicant/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Skills, string Experience, string Education, string PortfolioUrl, IFormFile Resume, IFormFile Profile)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("UserId").Value);
                var userName = User.FindFirst("Name")?.Value ?? "Applicant";

                string resumeDbPath = null;       
                string profilePicDbPath = null;   

                //Resume Upload
                if (Resume != null && Resume.Length > 0)
                {
                    var resumeFileName = $"{userName}_{userId}.pdf";  
                    var resumeFullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Resume", resumeFileName);

                    using (var stream = new FileStream(resumeFullPath, FileMode.Create))
                    {
                        await Resume.CopyToAsync(stream);
                    }

                    resumeDbPath = "/Resume/" + resumeFileName; 
                }

                //Profile Picture Upload
                if (Profile != null && Profile.Length > 0)
                {
                    var ext = Path.GetExtension(Profile.FileName).ToLower();
                    var profileFileName = $"{userName}_{userId}{ext}";
                    var profileFullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ProfilePicture", profileFileName);

                    using (var stream = new FileStream(profileFullPath, FileMode.Create))
                    {
                        await Profile.CopyToAsync(stream);
                    }

                    profilePicDbPath = "/ProfilePicture/" + profileFileName; 
                }




                var applicant = new Applicant
                {
                    UserId = userId,
                    Skills = Skills,
                    Experience = Experience,
                    Education = Education,
                    PortfolioUrl = PortfolioUrl,
                    ResumePath = resumeDbPath,
                    ProfilePicturePath = profilePicDbPath
                };

                _context.Applicant.Add(applicant);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;

                var inner = ex.InnerException;
                while (inner != null)
                {
                    errorMessage += " | Inner Exception: " + inner.Message;
                    inner = inner.InnerException;
                }
                ViewBag.Error = errorMessage;
                return View();
            }
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
