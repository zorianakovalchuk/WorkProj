using System.ComponentModel.DataAnnotations;

namespace WorkProj.Entity.Entity
{
    public class Book
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Distributor { get; set; }

        [Required]
        public string Isbn { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public int Year { get; set; }
        [Required] 
        public string ImageUrl { get; set; }

        [Required]
        public int NumberOfPages { get; set; }

        [Required]
        public string CoverOfBook { get; set; }

        [Required]
        public string Size { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public int? OpinionsNumber { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
