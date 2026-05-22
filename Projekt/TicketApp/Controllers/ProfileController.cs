using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketApp.Models;

namespace TicketApp.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ProfileController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User) as ApplicationUser;
        if (user == null) return NotFound();

        var model = new ProfileViewModel
        {
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Name = user.FirstName,
            Surname = user.LastName
        };
        
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Błąd: Nieprawidłowe dane formularza.";
            return RedirectToAction("Index");
        }
        var user = await _userManager.GetUserAsync(User) as ApplicationUser;
        if (user != null)
        {
            user.PhoneNumber = model.PhoneNumber;
            user.FirstName = model.Name;
            user.LastName = model.Surname;
            
            await _userManager.UpdateAsync(user);
            TempData["Success"] = "Profil zaktualizowany!";
        }
        
        return RedirectToAction("Index");
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ProfileViewModel model)
    {
        if (model.NewPassword != model.ConfirmPassword)
        {
            TempData["Error"] = "Hasła nie są takie same! / Passwords do not match!";
            return RedirectToAction("Index");
        }

        if (model.OldPassword == model.NewPassword)
        {
            TempData["Error"] = "Nowe hasło musi się różnić od starego! / The new password must be different from the old one!";
            return RedirectToAction("Index");
        }
        var user = await _userManager.GetUserAsync(User) as ApplicationUser;
        
        if (user != null && !string.IsNullOrEmpty(model.OldPassword) && !string.IsNullOrEmpty(model.NewPassword))
        {
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            
            if (result.Succeeded)
            {
                TempData["Success"] = "Hasło zmienione! / Password changed!";
            }
            else
            {
                var errors = string.Join(" ", result.Errors.Select(e => e.Description));
                TempData["Error"] = "Błąd / Error: " + errors;
            }
        }
        return RedirectToAction("Index");
    }
}