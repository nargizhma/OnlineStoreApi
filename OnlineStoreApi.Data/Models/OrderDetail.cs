using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStoreApi.Data.Models
{
    public class OrderDetail
    {
        [Key]
        public int Id { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        public Order Order { get; set; }

        public Product Product { get; set; }
    }
}
