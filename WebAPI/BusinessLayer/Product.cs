using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Product
    {

        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(50, ErrorMessage = "Name must not be more than 50 symbols!")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 symbols!")]
        public string Name{ get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Brand must not be more than 50 symbols!")]
        [MinLength(2, ErrorMessage = "Brand must be at least 2 symbols!")]
        public string Brand { get; set; }

        [Required]
        [Range(0, 10_000, ErrorMessage = "Price must be in (0;10_000]")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Quantity must be in (0;100]")]
        public int Quantity { get; set; }

        public Product()
        {

        }

        public Product(string name,string brand, decimal price, int quantity)
        {
            Name = name;
            Brand = brand;
            Price = price;
            Quantity = quantity;
            
        }

    }
}
