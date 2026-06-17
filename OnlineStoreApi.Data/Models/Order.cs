using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStoreApi.Data.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "pending"; // pending, confirmed, cancelled, delivered

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        // Backward compatibility with existing Customer model
        [ForeignKey("Customer")]
        public int? CustomerId { get; set; }

        public Customer Customer { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
