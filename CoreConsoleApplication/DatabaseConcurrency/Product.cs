using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public bool IsImported { get; set; } 
        public float floatProp { get; set; } 
        public double doubleProp { get; set; } 

        public IList<string> Companies { get; set; } 
    }
}
