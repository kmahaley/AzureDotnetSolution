using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CoreConsoleApplication.DatabaseConcurrency
{
    public class Store
    {
        [Key]
        public int StoreId { get; set; }

        [Required]
        public string StoreName { get; set; }
        [ConcurrencyCheck]
        public string City { get; set; }
    }
}
