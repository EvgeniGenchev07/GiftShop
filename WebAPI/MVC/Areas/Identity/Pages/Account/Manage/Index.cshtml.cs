using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize(Roles = "User")]
public class IndexModel : PageModel
{
    private readonly IdentityContext _identityContext;
    private readonly UserManager<User> _userManager;

    public IndexModel(
        UserManager<User> userManager,
        IdentityContext identityContext)
    {
        _userManager = userManager;
        _identityContext = identityContext;
    }


    [TempData] public string StatusMessage { get; set; }

    [BindProperty] public DataModel Data { get; set; }

    private async Task LoadAsync(User user)
    {
        Data = new DataModel
        {
            Name = user.Name,
            Orders = user.Orders is null ? new List<Order>() : user.Orders.Take(3).ToList(),
            Feedbacks = user.Feedbacks is null ? new List<Feedback>() : user.Feedbacks.Take(3).ToList()
        };
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        user = await _identityContext.ReadUserAsync(user.Id);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        await LoadAsync(user);
        return Page();
    }

    public class DataModel
    {
        public string Name { get; set; }
        public List<Order> Orders { get; set; }
        public List<Feedback> Feedbacks { get; set; }
    }
}
