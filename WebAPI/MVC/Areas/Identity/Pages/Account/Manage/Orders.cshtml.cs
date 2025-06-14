using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize(Roles = "User")]
public class OrdersModel : PageModel
{
    private const int PageSize = 5;
    private readonly IdentityContext _identityContext;
    private readonly UserManager<User> _userManager;

    public OrdersModel(
        UserManager<User> userManager,
        IdentityContext identityContext)
    {
        _userManager = userManager;
        _identityContext = identityContext;
    }

    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public DataModel Data { get; set; }

    public async Task<IActionResult> OnGetAsync(string status = null, int index = 1)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound("Unable to load user with ID 'user.Id'.");
        CurrentPage = index;

        var statusEnum = status is null ? Status.None : Enum.Parse<Status>(status);
        user = await _identityContext.ReadUserAsync(user.Id);
        var totalItems = user.Orders.Count();
        TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
        Data = new DataModel
        {
            TotalOrders = user.Orders.Count(),
            Orders = statusEnum == Status.None
                ? user.Orders
                : user.Orders.Where(x => x.Status == statusEnum)
                    .Skip((index - 1) * PageSize)
                    .Take(PageSize).ToList(),
            SelectedStatus = statusEnum
        };
        return Page();
    }

    public class DataModel
    {
        public int TotalOrders { get; set; }
        public Status SelectedStatus { get; set; }
        public List<Order> Orders { get; set; }
    }
}
