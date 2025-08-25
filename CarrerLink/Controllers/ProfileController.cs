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
       //Get userid 
        var userIdClaim = User.FindFirst("UserId")?.Value;

        int userId = int.Parse(userIdClaim);

        //Fetch user details
        var user = _context.User.FirstOrDefault(u => u.Id == userId);
        if (user == null)
            return NotFound();

        //Fetch user specific details either applicant or recruiter
        var applicant = _context.Applicant.FirstOrDefault(a => a.UserId == userId);
        var recruiter = _context.Recruiter.FirstOrDefault(r => r.UserId == userId);

        var model = new ProfileViewModel
        {
            User = user,
            Applicant = applicant,
            Recruiter = recruiter
        };

        return View("~/Views/Home/Profile.cshtml", model);
    }
}
