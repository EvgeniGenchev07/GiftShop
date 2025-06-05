namespace MVC.Models;

public class OrderSuccess
{
    public int OrderId { get; set; }
    
    public decimal TotalAmount { get; set; }
    
    public string PaymentMethod { get; set; }
    
    public string Email { get; set; }
    
}
