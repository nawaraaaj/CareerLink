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
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace CarrerLink.Controllers
{
    public class UserController : Controller
    {
        private readonly CarrerLinkContext _context;

        public UserController(CarrerLinkContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Name,Email,Mobile,Password,ConfirmPassword")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                var userExist = (from u in _context.User where u.UserName == userViewModel.Email select u).ToList();
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
                   
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));// redirect to login 
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
                var userExist = (from u in _context.User where u.UserName == LoginViewModel.Email && u.Password == LoginViewModel.Password select u).ToList();
                if (userExist.Count() > 0)
                {
                    List<Claim> claims = new List<Claim>();
                    Claim claim = new Claim(ClaimTypes.Email, LoginViewModel.Email);
                    Claim claim1 = new Claim(ClaimTypes.Role, userExist[0].UserType);
                    claims.Add(claim);
                    claims.Add(claim1);
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


        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.UserViewModel.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userViewModel = await _context.UserViewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userViewModel == null)
            {
                return NotFound();
            }

            return View(userViewModel);
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
        public async Task<IActionResult> Create([Bind("Id,Name,Email,Mobile,Password,ConfirmPassword")] UserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userViewModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userViewModel);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userViewModel = await _context.UserViewModel.FindAsync(id);
            if (userViewModel == null)
            {
                return NotFound();
            }
            return View(userViewModel);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email,Mobile,Password,ConfirmPassword")] UserViewModel userViewModel)
        {
            if (id != userViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userViewModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserViewModelExists(userViewModel.Id))
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
            return View(userViewModel);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userViewModel = await _context.UserViewModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userViewModel == null)
            {
                return NotFound();
            }

            return View(userViewModel);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userViewModel = await _context.UserViewModel.FindAsync(id);
            if (userViewModel != null)
            {
                _context.UserViewModel.Remove(userViewModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserViewModelExists(int id)
        {
            return _context.UserViewModel.Any(e => e.Id == id);
        }
    }
}
