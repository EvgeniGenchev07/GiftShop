using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLayer;

public class Order
{
    public Order()
    {
    }

    public Order(string name, string email, string address, string phoneNumber, User user,
        List<OrderedProduct> products, PaymentMethod paymentMethod, decimal shippingCost)
    {
        ReceiverName = name;
        ReceiverEmail = email;
        ReceiverAddress = address;
        ReceiverPhoneNumber = phoneNumber;
        User = user;
        OrderedProducts = products;
        Status = Status.Confirmed;
        Created = DateTime.Now;
        PaymentMethod = paymentMethod;
        ShippingCost = shippingCost;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(100, ErrorMessage = "Addres must not be more than 100 symbols!")]
    [MinLength(2, ErrorMessage = "Addres must be at least 20 symbols!")]
    public string ReceiverAddress { get; set; }

    [Required]
    [RegularExpression(@"(?:\+359\s?|0)\d\d\s?\d{3}\s?\d{3,4}", ErrorMessage = "Please, enter valid number!")]
    public string ReceiverPhoneNumber { get; set; }

    public User User { get; set; }
    public List<OrderedProduct> OrderedProducts { get; set; }
    public Status Status { get; set; }
    public DateTime Created { get; set; }
    public string ReceiverName { get; set; }

    [EmailAddress] public string ReceiverEmail { get; set; }

    public PaymentMethod PaymentMethod { get; set; }
    public decimal ShippingCost { get; set; }
}
