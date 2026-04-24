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
    public async Task<IActionResult> UpdateProfile(ProfileViewModel model)
    {
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
    public async Task<IActionResult> ChangePassword(ProfileViewModel model)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user != null && model.OldPassword != null && model.NewPassword != null)
        {
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["Success"] = "Hasło zmienione! / Password changed!";
            }
            else
            {
                TempData["Error"] = "Błąd! Sprawdź stare hasło. / Error! Check old password.";
            }
        }
        return RedirectToAction("Index");
    }
}