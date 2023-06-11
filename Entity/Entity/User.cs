using System.ComponentModel.DataAnnotations;

namespace WorkProj.Entity.Entity
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }

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

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Book>? FavoriteBooks { get; set; }

        public virtual ICollection<Book>? CartBooks { get; set; }
    }
}
