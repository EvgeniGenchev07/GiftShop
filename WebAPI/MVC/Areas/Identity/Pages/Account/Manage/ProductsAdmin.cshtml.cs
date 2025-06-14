using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize(Roles = "Administrator")]
public class ProductsAdminModel : PageModel
{
    private readonly IDb<Product, int> _productContext;

    public ProductsAdminModel(ProductContext productContext)
    {
        _productContext = productContext;
    }

    public List<Product> Products { get; set; }

    public async Task OnGetAsync()
    {
        Products = (await _productContext.ReadAll(true, true)).Where(p => p.Status != ProductStatus.Deleted).ToList();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var product = await _productContext.Read(id, false, true);
        if (product != null)
        {
            product.Status = ProductStatus.Deleted;
            await _productContext.Update(product);
            TempData["StatusMessage"] = "Product deleted successfully";
        }

        return RedirectToPage();
    }
}
