using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers;

public class FeedbacksController : Controller
{
    private readonly IDb<Feedback, int> _context;
    private readonly UserManager<User> _userManager;

    public FeedbacksController(FeedbackContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Feedbacks
    public async Task<IActionResult> Index()
    {
        return View(await _context.ReadAll(true, true));
    }


    // GET: Feedbacks/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Feedbacks/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Rating,Review")] Feedback feedback)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null) feedback.User = user;
            feedback.Created = DateTime.Now;
            await _context.Create(feedback);
            return RedirectToAction(nameof(Index));
        }

        return View(feedback);
    }
}
