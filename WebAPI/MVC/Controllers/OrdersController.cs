using BusinessLayer;
using DataLayer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using Newtonsoft.Json;

namespace MVC.Controllers;

public class OrdersController : Controller
{
    private readonly IdentityContext _authentication;
    private readonly IDb<Order, int> _context;

    public OrdersController(OrderContext context, IdentityContext identity)
    {
        _context = context;
        _authentication = identity;
    }

    [HttpPost]
    public async Task<IActionResult> ProcessOrder([FromBody] Dictionary<string, object> data)
    {
        if (data is null || !data.ContainsKey("id") || !data.ContainsKey("status")) return BadRequest();
        var order = await _context.Read(int.Parse(data["id"].ToString()), false, true);
        if (order == null) return NotFound();
        order.Status = Enum.Parse<Status>(data["status"].ToString());
        await _context.Update(order);
        return Ok();
    }

    // GET: Orders/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Orders/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(IFormCollection keyValuePairs)
    {
        try
        {
            var json = keyValuePairs["productsString"];
            TempData["orderedProducts"] = json;
            return RedirectToAction(nameof(Checkout));
        }
        catch (Exception ex)
        {
            return View();
        }
    }

    [Authorize(Roles = "User")]
    public IActionResult Checkout()
    {
        try
        {
            var products =
                JsonConvert.DeserializeObject<List<OrderedProduct>>((TempData["orderedProducts"] as string[])[0]);
            return View(nameof(Checkout), products);
        }
        catch (Exception ex)
        {
            return RedirectToAction(nameof(Create));
        }
    }

    [HttpPost]
    [Authorize(Roles = "User")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(IFormCollection keyValuePairs)
    {
        try
        {
            var name = keyValuePairs["firstName"] + " " + keyValuePairs["lastName"];
            var address = keyValuePairs["apartment"] + ", " + keyValuePairs["address"] + ", " + keyValuePairs["city"] +
                          ", " + keyValuePairs["state"] + ", " + keyValuePairs["country"] + ", " + keyValuePairs["zip"];
            Console.WriteLine(address);
            var user = await _authentication.ReadUserAsync(User.Identity.GetUserId<string>());
            var products = JsonConvert.DeserializeObject<List<OrderedProduct>>(keyValuePairs["productsString"]);
            var paymentMethod = keyValuePairs["payment"] == "cashOnDelivery"
                ? PaymentMethod.CashOnDelivery
                : PaymentMethod.CreditCard;
            var order = new Order(name, keyValuePairs["email"], address, keyValuePairs["phoneNumber"], user, products,
                paymentMethod, products.Sum(p => p.Product.Price * p.Quantity) <= 100 ? 10 : 0);
            await _context.Create(order);
            var newOrder = await _authentication.GetUserLastOrder(user.Id);
            TempData["OrderId"] = newOrder.Id;
            TempData["Email"] = newOrder.ReceiverEmail;
            TempData["PaymentMethod"] = newOrder.PaymentMethod.ToString();
            var total = (double)newOrder.OrderedProducts.Sum(p => p.Product.Price * p.Quantity);
            TempData["TotalAmount"] = (total + (total <= 100 ? 10 : 0)).ToString();
            return RedirectToAction(nameof(Confirmed));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return RedirectToAction(nameof(Create));
        }
    }

    [Authorize(Roles = "User")]
    public IActionResult Confirmed()
    {
        var model = new OrderSuccess
        {
            OrderId = Convert.ToInt32(TempData["OrderId"]),
            Email = TempData["Email"]?.ToString(),
            PaymentMethod = TempData["PaymentMethod"]?.ToString(),
            TotalAmount = TempData["TotalAmount"].ToString()
        };

        return View(model);
    }
}
