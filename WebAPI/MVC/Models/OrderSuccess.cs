namespace MVC.Models;

public class OrderSuccess
{
    public int OrderId { get; set; }

    public string TotalAmount { get; set; }

    public string PaymentMethod { get; set; }

    public string Email { get; set; }
}
