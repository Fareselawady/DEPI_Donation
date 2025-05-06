using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DEPI_Donation.Models;
using DEPI_Donation.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using DEPI_Donation.Data;

public class AccountController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly AppDbcontext _context; 
    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager , AppDbcontext appDbcontext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = appDbcontext;
    }

    //public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User { UserName = model.UserName, Email = model.Email, PhoneNumber = model.PhoneNumber };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        return View(nameof(Login));
    }

    public IActionResult Login()
    {
        if(User.Identity!.IsAuthenticated)
            return RedirectToAction("Index", "Home");

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Invalid login attempt.");
        }

        return View(model);
    }
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("LogIn", "Account");
    }

    [HttpGet]
    public async Task<IActionResult> Dashboard()
    {
        var user = await _userManager.GetUserAsync(User); 
        if (user == null)
        {
            return RedirectToAction("Login", "Account"); 
        }

        var donations = await _context.Donations
                                      .Where(d => d.DonorId == user.Id) 
                                      .Include(d => d.Activity)
                                      .ToListAsync();

        return View("Dashboard",donations);
    }

    [HttpGet]

    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User); 
        if (user == null)
        {
            return RedirectToAction("Login", "Account"); 
        }

        var donations = await _context.Donations
                                      .Where(d => d.DonorId == user.Id) 
                                      .Include(d => d.Activity)
                                      .ToListAsync();

        return View("Profile",donations);
    }




}
