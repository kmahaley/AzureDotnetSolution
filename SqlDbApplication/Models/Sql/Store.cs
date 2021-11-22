using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SqlDbApplication.Models.Sql
{
    public class Store
    {
        [Key]
        public int StoreId { get; set; }
        [Required]
        public string StoreName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
