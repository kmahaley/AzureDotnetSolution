using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SqlDbApplication.Models.Sql
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
    }
}
