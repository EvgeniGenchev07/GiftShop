using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize(Roles = "Administrator")]
public class FeedbacksAdminModel : PageModel
{
    private readonly IDb<Feedback, int> _feedbackContext;

    public FeedbacksAdminModel(FeedbackContext feedbackContext)
    {
        _feedbackContext = feedbackContext;
    }

    public List<Feedback> Feedbacks { get; set; }

    public async Task OnGetAsync()
    {
        Feedbacks = await _feedbackContext.ReadAll(true, true);
    }

    public async Task<IActionResult> OnPostDeleteReviewAsync(int id)
    {
        await _feedbackContext.Delete(id);
        TempData["StatusMessage"] = "Review deleted successfully";
        return RedirectToPage();
    }
}
