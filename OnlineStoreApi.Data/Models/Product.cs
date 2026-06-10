using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStoreApi.Data.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        [Range(0, 50)]
        public decimal Discount { get; set; } // Max 50%

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "active"; // active, inactive, tükənib

        [ForeignKey("ProductCategory")]
        public int CategoryId { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
