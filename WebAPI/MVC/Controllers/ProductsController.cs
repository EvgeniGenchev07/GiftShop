using System.Diagnostics.CodeAnalysis;
using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers;

public class ProductsController : Controller
{
    private readonly IDb<Product, int> _context;

    public ProductsController(ProductContext context)
    {
        _context = context;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllById([FromBody] [AllowNull] int[][] data)
    {
        try
        {
            if (data != null && data.Length > 0 && data[0].Length > 0)
            {
                var products = (await _context.ReadAll(false, true)).Where(p => p.Status == ProductStatus.InStock)
                    .ToList();
                var result = new List<OrderedProduct>();
                for (var i = 0; i < data[0].Length; ++i)
                {
                    var product = products.FirstOrDefault(p => p.Id == data[0][i]);
                    if (product is not null) result.Add(new OrderedProduct(data[1][i], product));
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
        return View((await _context.ReadAll()).Where(p => p.Status == ProductStatus.InStock).ToList());
    }

    // GET: Products/Create
    [Authorize(Roles = "Administrator")]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Products/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Create(
        [Bind("Id,Name,Brand,Price,Quantity")] Product product,
        IFormFile photo)
    {
        if (ModelState.IsValid)
        {
            if (photo != null && photo.Length > 0)
            {
                var fileName = $"{product.Name}{Path.GetExtension(photo.FileName)}";
                var filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await photo.CopyToAsync(stream);
                }

                product.ImagePath = $"/images/{fileName}";
                product.Status = ProductStatus.InStock;
            }

            await _context.Create(product);
            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    // GET: Products/Edit/5
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var product = await _context.Read((int)id);
        if (product == null) return NotFound();
        return View(product);
    }

    // POST: Products/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Administrator")]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Name,Brand,Price,Quantity,ImagePath")]
        Product product,
        IFormFile photo)
    {
        if (id != product.Id) return NotFound();

        if (ModelState.IsValid)
        {
            if (photo != null && photo.Length > 0)
            {
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    var oldImagePath = Path.Combine("wwwroot", product.ImagePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath)) System.IO.File.Delete(oldImagePath);
                }

                var fileName = $"{product.Name}{Path.GetExtension(photo.FileName)}";
                var filePath = Path.Combine("wwwroot/images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await photo.CopyToAsync(stream);
                }

                product.ImagePath = $"/images/{fileName}";
            }

            await _context.Update(product);
            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }
}
