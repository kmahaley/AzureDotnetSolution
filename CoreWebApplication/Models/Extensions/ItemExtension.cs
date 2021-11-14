using CoreWebApplication.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreWebApplication.Models.Extensions
{
    public static class ItemExtension
    {
        public static ItemDto AsItemDto(this Item item)
        {
            return new ItemDto
            {
                Id = item.Id,
                Price = item.Price,
                CreatedDate = item.CreatedDate,
                Name = item.Name
            };
        }
    }
}
