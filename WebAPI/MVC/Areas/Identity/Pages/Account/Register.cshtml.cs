using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using BusinessLayer;
using DataLayer;
using NuGet.Protocol;

namespace MVC.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IdentityContext _identityContext;

        public RegisterModel(
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IdentityContext identityContext)
        {
            _signInManager = signInManager;
            _logger = logger;
            _identityContext = identityContext;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Name")]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 5)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _identityContext.CreateUserAsync(Input.Username, Input.Password, Input.Email, Role.User);
                    _logger.LogInformation("User created a new account with password.");

                    var createdUser = await _signInManager.UserManager.FindByEmailAsync(Input.Email);
            
                    await _signInManager.SignInAsync(createdUser, isPersistent: false);
            
                    return LocalRedirect("/");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError(string.Empty, "Error creating new account.");
                    _logger.LogError(e, "Error during registration");
                    return Page();
                }
            }
            return Page();
        }
    }
}
