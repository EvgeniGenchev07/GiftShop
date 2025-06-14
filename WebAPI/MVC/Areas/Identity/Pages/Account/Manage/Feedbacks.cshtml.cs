using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize(Roles = "User")]
public class FeedbacksModel : PageModel
{
    private readonly IdentityContext _identityContext;
    private readonly UserManager<User> _userManager;

    public FeedbacksModel(IdentityContext identityContext, UserManager<User> userManager)
    {
        _identityContext = identityContext;
        _userManager = userManager;
    }

    public List<Feedback> Feedbacks { get; set; } = new();
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; }
    public int PageSize { get; set; } = 5;

    public async Task OnGetAsync(int page = 1)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            throw new ApplicationException($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        user = await _identityContext.ReadUserAsync(user.Id);
        Feedbacks = user
            .Feedbacks is null
            ? new List<Feedback>()
            : user.Feedbacks
                .OrderByDescending(f => f.Created)
                .Skip((page - 1) * PageSize)
                .Take(PageSize)
                .ToList();
    }
}
