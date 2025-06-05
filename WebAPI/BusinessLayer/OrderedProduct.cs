using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLayer;

public class OrderedProduct
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int Quantity { get; set; }
    public Product Product { get; set; }

    public OrderedProduct()
    {
    }

    public OrderedProduct(int quantity, Product product)
    {
        Quantity = quantity;
        Product = product;
    }
}
