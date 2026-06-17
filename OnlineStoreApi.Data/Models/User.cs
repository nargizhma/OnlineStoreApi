using System.ComponentModel.DataAnnotations;

namespace OnlineStoreApi.Data.Models
{
    public class User
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

        [Required]
        [MaxLength(255)]
        public string PasswordHash { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
