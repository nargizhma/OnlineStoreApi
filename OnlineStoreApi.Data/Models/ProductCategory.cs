using System.ComponentModel.DataAnnotations;

namespace OnlineStoreApi.Data.Models
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
