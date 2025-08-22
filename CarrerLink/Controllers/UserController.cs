using CarrerLink.Data;
using CarrerLink.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CarrerLink.Controllers
{
    public class UserController : Controller
    {
        private readonly CarrerLinkContext _context;

        public UserController(CarrerLinkContext context)
        {
            _context = context;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Password,Mobile,UserType")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Register
        [AllowAnonymous]
        public IActionResult Register(string userType)
        {
            ViewBag.UserType = userType;
            return View(new UserViewModel());
        }

        // POST: User/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Name,Email,Mobile,Password,ConfirmPassword")] UserViewModel userViewModel, string UserType)
        {
            if (ModelState.IsValid)
            {
                var userExist = (from u in _context.User where u.Email == userViewModel.Email select u).ToList();
                if (userExist.Count() > 0)
                {
                 ViewData["ErrorMessage"] = "User already exists.";
                }
                else
                {
                    User user = new User();
                    user.Name = userViewModel.Name;
                    user.Email = userViewModel.Email;
                    user.Password = userViewModel.Password;
                    user.Mobile = userViewModel.Mobile;
                    user.UserType = UserType;

                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    if (UserType == "Applicant")
                        return RedirectToAction("Create", "Applicant", new { userId = user.Id });
                    else
                        return RedirectToAction("Create", "Recruiter", new { userId = user.Id });
                }
            }
            return View(userViewModel);
        }

        // GET: User/Login
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        // POST: User/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("Email,Password")] LoginViewModel LoginViewModel)
        {
            if (ModelState.IsValid)
            {
                var userExist = (from u in _context.User where u.Email == LoginViewModel.Email && u.Password == LoginViewModel.Password select u).ToList();
                if (userExist.Count() > 0)
                {
                    List<Claim> claims = new List<Claim>();
                    Claim claim = new Claim(ClaimTypes.Email, LoginViewModel.Email);
                    //Claim claim1 = new Claim(ClaimTypes.Role, userExist[0].UserType);
                    claims.Add(claim);
                    //claims.Add(claim1);
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal claimsPrincical = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincical);

                    return RedirectToAction("ProductDashboard", "Product");
                }
                else
                {
                    ViewData["ErrorMessage"] = "Username/Password is invalid.";
                }
            }
            return View(LoginViewModel);
        }


        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Password,Mobile,UserType")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
