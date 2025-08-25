using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CarrerLink.Data;
using CarrerLink.Models;

[Authorize]
public class ProfileController : Controller
{
    private readonly CarrerLinkContext _context;

    public ProfileController(CarrerLinkContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        // 1️⃣ Get logged-in user ID from the "UserId" claim
        var userIdClaim = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        int userId = int.Parse(userIdClaim);

        // 2️⃣ Fetch User entity
        var user = _context.User.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return NotFound();

        // 3️⃣ Fetch related Applicant and Recruiter
        var applicant = _context.Applicant.FirstOrDefault(a => a.UserId == userId);
        var recruiter = _context.Recruiter.FirstOrDefault(r => r.UserId == userId);

        // 4️⃣ Build ProfileViewModel
        var model = new ProfileViewModel
        {
            User = user,
            Applicant = applicant,
            Recruiter = recruiter
        };

        // 5️⃣ Return view
        return View("~/Views/Home/Profile.cshtml", model);
    }
}
