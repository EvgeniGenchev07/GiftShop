using System.ComponentModel.DataAnnotations;
using BusinessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MVC.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ForgotPasswordModel : PageModel
{
    private readonly UserManager<User> _userManager;

    public ForgotPasswordModel(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    [BindProperty] public InputModel Input { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToPage("./ForgotPasswordConfirmation");

            return RedirectToPage("./ForgotPasswordConfirmation");
        }

        return Page();
    }

    public class InputModel
    {
        [Required] [EmailAddress] public string Email { get; set; }
    }
}
