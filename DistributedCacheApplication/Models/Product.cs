using System.ComponentModel.DataAnnotations;

namespace DistributedCacheApplication.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Color { get; set; }
        
        public int UnitPrice { get; set; }
        
        public int AvailableQuantity { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}
