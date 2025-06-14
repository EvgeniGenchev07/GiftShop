using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer;

public class User : IdentityUser
{
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
        Feedbacks = new List<Feedback>();
    }

    public User(string id, string email, string name) : this(email, name)
    {
        Id = id;
    }

    [Required]
    [PersonalDataAttribute]
    [MinLength(5, ErrorMessage = "Name must be at least 5 symbols!")]
    [MaxLength(50, ErrorMessage = "Name must not be more than 50 symbols!")]
    public string Name { get; set; }

    [PersonalDataAttribute]
    [MinLength(20, ErrorMessage = "Address must be at least 20 symbols!")]
    [MaxLength(50, ErrorMessage = "Address must not be more than 50 symbols!")]
    public string Address { get; set; }

    public List<Feedback> Feedbacks { get; set; }
    public List<Order> Orders { get; set; }

    public static explicit operator User(ValueTask<IdentityUser> v)
    {
        throw new NotImplementedException();
    }
}
