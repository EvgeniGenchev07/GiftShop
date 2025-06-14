using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessLayer;

[JsonSerializable(typeof(Product))]
public class Product
{
    public Product()
    {
    }

    public Product(string name, decimal price, int quantity)
    {
        Name = name;
        Price = price;
        Quantity = quantity;
        Status = ProductStatus.InStock;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }


    [Required]
    [MaxLength(50, ErrorMessage = "Name must not be more than 50 symbols!")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 symbols!")]
    public string Name { get; set; }

    public string ImagePath { get; set; }

    [Required]
    [Range(0, 10_000, ErrorMessage = "Price must be in (0;10_000]")]
    public decimal Price { get; set; }

    [Required]
    [Range(0, 100, ErrorMessage = "Quantity must be in (0;100]")]
    public int Quantity { get; set; }

    public ProductStatus Status { get; set; }
}
