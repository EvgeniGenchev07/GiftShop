using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class Feedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public User User { get; set; }

        [Required]
        [Range(0, 5, ErrorMessage = "Rating must be in (0;5]")]
        public int Rating { get; set; }

       
        [MaxLength(1_000, ErrorMessage = "Review must not be more than 1000 symbols!")]
        [MinLength(2, ErrorMessage = "Review must be at least 2 symbols!")]
        public string Review { get; set; }

        public Feedback()
        {

        }

        public Feedback(User user, int rating, string review)
        {
            User = user;
            Rating = rating;
            Review = review;

        }
        public Feedback(int rating, string review)
        {
            Rating = rating;
            Review = review;
        }
    }
}
