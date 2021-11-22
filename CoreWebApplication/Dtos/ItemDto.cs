using System;
using System.ComponentModel.DataAnnotations;

namespace CoreWebApplication.Dtos
{
    public class ItemDto
    {
        public Guid Id { get; init; }
        
        [Required]
        public string Name { get; set; }

        [Required]
        [Range(0, 10000)]
        public int Price { get; set; }
        
        public DateTimeOffset CreatedDate { get; init; }
    }
}
