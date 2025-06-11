using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessLayer;
using DataLayer;
using Newtonsoft.Json;
using NuGet.Protocol;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using MVC.Models;


namespace MVC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IDb<Order,int> _context;
        private readonly IdentityContext _authentication;
        
        public OrdersController(OrderContext context, IdentityContext identity)
        {
            _context = context;
            _authentication = identity;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReadAll());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Read((int)id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
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
                TempData["orderedProducts"] =  json;
                return RedirectToAction(nameof(Checkout));
            }
            catch (Exception ex)
            {
                return View();
            }
        }
        public IActionResult Checkout()
        {
            try
            {
                var products = JsonConvert.DeserializeObject<List<OrderedProduct>>((TempData["orderedProducts"]as string[])[0]);
                return View(nameof(Checkout),products);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Create));
            }
        }
        [HttpPost]
        //[Authorize(Roles = "User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(IFormCollection keyValuePairs)
        {
            try
            {
                string name = keyValuePairs["firstName"] + " " + keyValuePairs["lastName"];
                string address = keyValuePairs["apartment"] +", "+ keyValuePairs["address"]+", "+keyValuePairs["city"]+", "+keyValuePairs["state"]+", "+keyValuePairs["country"]+", "+keyValuePairs["zip"];
                Console.WriteLine(address);
                User user = await _authentication.ReadUserAsync(User.Identity.GetUserId<string>());
                List<OrderedProduct> products = JsonConvert.DeserializeObject<List<OrderedProduct>>(keyValuePairs["productsString"]);
                PaymentMethod paymentMethod = keyValuePairs["payment"] == "cashOnDelivery" ? PaymentMethod.CashOnDelivery : PaymentMethod.CreditCard;
                Order order = new Order(name,keyValuePairs["email"], address,keyValuePairs["phoneNumber"], user,products,paymentMethod,(products.Sum(p => p.Product.Price * p.Quantity)<=100?10:0));
                await _context.Create(order);
                Order newOrder = await _authentication.GetUserLastOrder(user.Id);
                TempData["OrderId"] = newOrder.Id;
                TempData["Email"] = newOrder.ReceiverEmail;
                TempData["PaymentMethod"] = newOrder.PaymentMethod.ToString();
                var total = (double)newOrder.OrderedProducts.Sum(p => p.Product.Price * p.Quantity);
                TempData["TotalAmount"] = (total+(total<=100?10:0)).ToString();
                return RedirectToAction(nameof(Confirmed));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return RedirectToAction(nameof(Create));
            }
            
        }

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

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Read((int)id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Address,PhoneNumber,Price")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Update(order);

                }
                catch (ArgumentException) 
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Read((int)id);
                
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Read(id);
            if (order != null)
            {
                 await _context.Delete(order.Id);
            }

            
            return RedirectToAction(nameof(Index));
        }

       
    }
}
