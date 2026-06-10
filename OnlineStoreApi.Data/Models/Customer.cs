using System.ComponentModel.DataAnnotations;

namespace OnlineStoreApi.Data.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Phone]
        [MaxLength(20)]
        public string Phone { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
