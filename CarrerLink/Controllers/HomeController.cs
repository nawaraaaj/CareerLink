using System.Diagnostics;
using CarrerLink.Models;
using CarrerLink.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarrerLink.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CarrerLinkContext _context;

        public HomeController(ILogger<HomeController> logger, CarrerLinkContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: query string
        public async Task<IActionResult> Index(string keyword, string jobType)
        {
            
            var jobs = _context.Job
                               .Include(j => j.Recruiter)
                               .ThenInclude(r => r.User)
                               .AsQueryable();

            
            if (!string.IsNullOrEmpty(keyword))
            {
                jobs = jobs.Where(j =>
                    j.Title.Contains(keyword) ||
                    j.Recruiter.CompanyName.Contains(keyword) ||
                    j.Location.Contains(keyword));
            }

            
            if (!string.IsNullOrEmpty(jobType))
            {
                jobs = jobs.Where(j => j.JobType == jobType);
            }

            // shows job types from db
            var jobTypes = await _context.Job
                                         .Select(j => j.JobType)
                                         .Where(jt => !string.IsNullOrEmpty(jt))
                                         .Distinct()
                                         .OrderBy(jt => jt)
                                         .ToListAsync();

            ViewBag.JobTypes = jobTypes;

            // sorting descending
            var jobList = await jobs
                                .OrderByDescending(j => j.PostedDate)
                                .ToListAsync();

            return View(jobList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
