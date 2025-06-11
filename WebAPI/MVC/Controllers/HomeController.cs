using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDb<Product,int> _context;

        public HomeController(ILogger<HomeController> logger,ProductContext productContext)
        {
            _logger = logger;
            _context = productContext;
        }

        public IActionResult Index()
        {
            return View(_context.ReadAll(false,true).Result.Take(8).ToList());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
