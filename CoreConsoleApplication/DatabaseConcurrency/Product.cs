using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CoreConsoleApplication.DatabaseConcurrency
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [ConcurrencyCheck]
        [Required]
        public string Name { get; set; }
        public string Color { get; set; }
        public int UnitPrice { get; set; }
        public int AvailableQuantity { get; set; }
    }
}
