using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using LibraryWebApplication.Models;
using LibraryWebApplication.AccountViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }



    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterAccountViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }



    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginAccountViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return View(model);
    }



    [HttpGet]
    [Authorize]
    public IActionResult Logout()
    {
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> LogoutPost()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }



    [HttpGet]
    [ResponseCache]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();

        foreach (var user in users)
        {

            ViewData[$"Roles_{user.Id}"] = await _userManager.GetRolesAsync(user);
        }

        ViewBag.Users = users;
        return View();
    }



    [HttpGet]
    [Authorize(Roles = "Admin")]
    public ActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateAccountViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.IsAdmin ? "Admin" : "User");

                return RedirectToAction("Index", "Account");
            }
        }

        return View(model);
    }



    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var editViewModel = new EditAccountViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            IsAdmin = await _userManager.IsInRoleAsync(user, "Admin")
        };

        return View(editViewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Edit(EditAccountViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var existingUser = await _userManager.FindByIdAsync(model.Id);

                existingUser.UserName = model.UserName;
                existingUser.Email = model.Email;

                await _userManager.UpdateAsync(existingUser);

                await _userManager.AddToRoleAsync(existingUser, model.IsAdmin ? "Admin" : "User");

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        return View(model);
    }



    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var viewModel = new DeleteAccountViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };

        return View(viewModel);
    }

    [HttpPost, ActionName("Delete")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.DeleteAsync(user);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(user);
        }

        return RedirectToAction("Index");
    }



    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ResetPassword(string id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var user = await _userManager.FindByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var resetPasswordViewModel = new ResetPasswordViewModel
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };

        return View(resetPasswordViewModel);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var existingUser = await _userManager.FindByIdAsync(model.Id);

                existingUser.UserName = model.UserName;
                existingUser.Email = model.Email;

                await _userManager.UpdateAsync(existingUser);

                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                    await _userManager.ResetPasswordAsync(existingUser, token, model.NewPassword);
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        return View(model);
    }
}
