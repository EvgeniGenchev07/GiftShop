using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;


namespace BusinessLayer
{
    public class User:IdentityUser
    {
        [Required]
        [MinLength(5, ErrorMessage = "Name must be at least 5 symbols!")]
        [MaxLength(50, ErrorMessage = "Name must not be more than 50 symbols!")]
        public string Name { get; set; }
        
        [MinLength(20, ErrorMessage = "Address must be at least 20 symbols!")]
        [MaxLength(50, ErrorMessage = "Address must not be more than 50 symbols!")]
        public string Address { get; set; }

        public List<Order> Orders { get; set; }
       // public List<Feedback> Feedbacks { get; set; }

        public User()
        {

        }

        public User(string email, string name)
        {
            Email = email;
            NormalizedEmail = email.ToUpper();
            UserName = email;
            NormalizedUserName = email.ToUpper();
            Name = name;
            Orders = new List<Order>();
        }
        public User(string id, string email, string name) : this(email,name)
        {
            this.Id = id;
        }
        public static explicit operator User(ValueTask<IdentityUser> v)
        {
            throw new NotImplementedException();
        }
    }
}
