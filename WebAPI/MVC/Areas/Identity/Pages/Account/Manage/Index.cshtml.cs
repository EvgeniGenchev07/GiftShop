using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BusinessLayer;
using DataLayer;
using System.Data.Entity;

namespace MVC.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IdentityContext _identityContext;

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IdentityContext identityContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityContext = identityContext;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public DataModel Data { get; set; }

        public class DataModel
        {
           
            public string Name { get; set; }
            public List<Order> Orders { get; set; }
        }

        private async Task LoadAsync(User user)
        {

            Data = new DataModel
            {
                Name = user.Name,
                Orders = user.Orders?? new List<Order>(),
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            user =  await _identityContext.ReadUserAsync(user.Id);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

       
    }
}
