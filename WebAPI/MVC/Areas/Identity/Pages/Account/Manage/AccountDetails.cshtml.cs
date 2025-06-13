using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using BusinessLayer;

namespace MVC.Areas.Identity.Pages.Account.Manage
{
    public class AccountDetailsModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountDetailsModel> _logger;

        public AccountDetailsModel(
            UserManager<User> userManager,
            ILogger<AccountDetailsModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [TempData] 
        public string StatusMessage { get; set; }
        
        public bool IsEditing { get; set; }
        
        [BindProperty] 
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Phone]
            [Display(Name = "Phone Number")]
            public string? PhoneNumber { get; set; }

            [Display(Name = "Address")]
            public string? Address { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string handler = null)
        {
            IsEditing = handler == "edit";
            var user = await _userManager.GetUserAsync(User);
            
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadData(user);
            return Page();
        }
        
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadData(user);
                return Page();
            }

            // Update user properties
            user.Name = Input.FullName;
            user.Email = Input.Email;
            user.PhoneNumber = Input.PhoneNumber;
            user.Address = Input.Address;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                await LoadData(user);
                return Page();
            }

            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        private async Task LoadData(User user)
        {
            Input = new InputModel
            {
                FullName = user.Name,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address
            };
        }
    }
}
