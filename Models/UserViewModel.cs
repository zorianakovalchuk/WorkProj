using System.ComponentModel.DataAnnotations;
using WorkProj.Entity;
using WorkProj.Entity.Entity;

namespace WorkProj.Models
{
    public class UserViewModel
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public static List<Book>? Books { get; set; }

        public static User? User { get; set; }

        public User ToUser()
        {
            return new User
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                Email = this.Email,
                Password = this.Password,
                ConfirmPassword = this.ConfirmPassword,
                CreatedAt = this.CreatedAt
            };
        }
        public User ValidateUser(DBContext context)
        {
            List<User> users = context.Users.ToList();

            foreach (var user in users)
            {
                if (user.Email == this.Email && user.Password == this.Password)
                {
                    return user;
                }
            }

            return null;
        }

        public static decimal TotalPrice()
        {

            decimal price = 0;

            if (User?.CartBooks is not null)
            {
                foreach (var book in User.CartBooks)
                {
                    price += book.Price;
                }
            }

            return price;
        }
    }
}
