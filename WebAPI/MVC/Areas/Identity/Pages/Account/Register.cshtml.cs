using System.ComponentModel.DataAnnotations;
using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MVC.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class RegisterModel : PageModel
{
    private readonly IdentityContext _identityContext;
    private readonly ILogger<RegisterModel> _logger;
    private readonly SignInManager<User> _signInManager;

    public RegisterModel(
        SignInManager<User> signInManager,
        ILogger<RegisterModel> logger,
        IdentityContext identityContext)
    {
        _signInManager = signInManager;
        _logger = logger;
        _identityContext = identityContext;
    }

    [BindProperty] public InputModel Input { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
            try
            {
                await _identityContext.CreateUserAsync(Input.Username, Input.Password, Input.Email, Role.User);
                _logger.LogInformation("User created a new account with password.");

                var createdUser = await _signInManager.UserManager.FindByEmailAsync(Input.Email);

                await _signInManager.SignInAsync(createdUser, false);

                return LocalRedirect("/");
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, "Error creating new account.");
                _logger.LogError(e, "Error during registration");
                return Page();
            }

        return Page();
    }

    public class InputModel
    {
        [Required] [Display(Name = "Name")] public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 5)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
