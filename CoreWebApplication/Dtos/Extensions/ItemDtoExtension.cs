using CoreWebApplication.Models;

namespace CoreWebApplication.Dtos.Extensions
{
    public static class ItemDtoExtension
    {
        public static Item AsItem(this ItemDto item)
        {
            return new Item
            {
                Id = item.Id,
                Price = item.Price,
                CreatedDate = item.CreatedDate,
                Name = item.Name
            };
        }
    }
}
