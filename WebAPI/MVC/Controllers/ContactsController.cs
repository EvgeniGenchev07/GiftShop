using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;

namespace MVC.Controllers;

public class ContactsController : Controller
{
    private readonly ILogger<ContactsController> _logger;

    public ContactsController(ILogger<ContactsController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Submit()
    {
        string name = Request.Form["name"];
        string email = Request.Form["email"];
        string phone = Request.Form["phone"];
        string message = Request.Form["message"];
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone) ||
            string.IsNullOrWhiteSpace(message))
        {
            ViewBag.Error = "All fields are required.";
            return RedirectToAction(nameof(Index), ControllerContext.ActionDescriptor.ControllerName);
        }

        if (!email.Contains("@") || !email.Contains("."))
        {
            ViewBag.Error = "Please enter a valid email address.";
            return RedirectToAction("Index");
        }

        Console.WriteLine($"Name: {name}, Email: {email}, Phone: {phone}, Message: {message}");
        ViewBag.Success = "Thank you for contacting us!";
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
