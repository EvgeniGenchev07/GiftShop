using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

[Authorize(Roles = "Administrator")]
public class OrdersAdminModel : PageModel
{
    private readonly IDb<Order, int> _context;

    public OrdersAdminModel(OrderContext context)
    {
        _context = context;
    }

    public List<Order> Orders { get; set; }
    public List<Status> AvailableStatuses { get; } = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

    public async Task OnGetAsync()
    {
        Orders = await _context.ReadAll(true, true);
    }

    public async Task<IActionResult> OnPostUpdateStatusAsync(int orderId, Status status)
    {
        var order = await _context.Read(orderId, false, true);
        if (order != null)
        {
            order.Status = status;
            await _context.Update(order);
            TempData["StatusMessage"] = $"Order #{orderId} status updated to {status}";
        }

        return RedirectToPage();
    }
}
