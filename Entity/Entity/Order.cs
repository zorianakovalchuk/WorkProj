using System.ComponentModel.DataAnnotations;
namespace WorkProj.Entity.Entity
{
    public class Order
    {
        [Key]
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public virtual User User { get; set; }

        public virtual Book Book { get; set; }
    }
}
