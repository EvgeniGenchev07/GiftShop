using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessLayer;
using DataLayer;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;

namespace MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IDb<Product,int> _context;

        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllById([FromBody][AllowNull] int[][] data)
        {
            try
            {
                if (data != null && data.Length > 0 && data[0].Length > 0)
                {
                    
                List<Product> products = await _context.ReadAll(false, true);
                List<OrderedProduct> result = new List<OrderedProduct>();
                for (int i = 0; i < data[0].Length; ++i)
                {
                    Product product = products.FirstOrDefault(p => p.Id == data[0][i]);
                    if (product is not null) result.Add(new OrderedProduct(data[1][i],product));
                }
                return Ok(result);
                }
                return Ok(new List<OrderedProduct>());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.ReadAll());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Read((int)id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
    [Bind("Id,Name,Brand,Price,Quantity")] Product product,
    IFormFile photo)  // Parameter name matches the input name
        {
            if (ModelState.IsValid)
            {
                // Handle image upload
                if (photo != null && photo.Length > 0)
                {
                    // Create unique filename
                    var fileName = $"{product.Name}{Path.GetExtension(photo.FileName)}";
                    var filePath = Path.Combine("wwwroot/images", fileName);

                    // Save file
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await photo.CopyToAsync(stream);
                    }

                    // Store path in database
                    product.ImagePath = $"/images/{fileName}";
                }

                await _context.Create(product);
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Read((int)id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Brand,Price,Quantity")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _context.Update(product);
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                catch (ArgumentException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Read((int)id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Read(id);
            if (product != null)
            {
                 await _context.Delete(product.Id);
            }

           
            return RedirectToAction(nameof(Index));
        }

       
    }
}
