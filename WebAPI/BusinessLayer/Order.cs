using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100,ErrorMessage ="Addres must not be more than 100 symbols!")]
        [MinLength(2,ErrorMessage ="Addres must be at least 20 symbols!")]
        public string Address { get; set; }
        [Required]
        [RegularExpression(@"(?:\+359\s?|0)\d\d\s?\d{3}\s?\d{3,4}", ErrorMessage = "Please, enter valid number!")]
        public string PhoneNumber { get; set; }
        [Required]
        public User User{ get; set; }
        [Required]
        public List<Product> Products { get; set; }

        [Range(0, 10_000, ErrorMessage = "Price must be in (0;10_000]")]
        public decimal Price { get; set; }

        private Order()
        {

        }
        public Order(string address, string phoneNumber,User user, decimal price,List<Product> products)
        {
            Address = address;
            PhoneNumber = phoneNumber;
            User = user;
            Price = price;
            Products = products;
        }
    }
}
