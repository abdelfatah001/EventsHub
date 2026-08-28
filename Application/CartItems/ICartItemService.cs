using Application.CartItems.Dto.Request;
using Domain.Entities;

namespace Application.CartItems
{
    public interface ICartItemService
    {
        Task AddAsync(int userId, AddCartItemDto newItem);

        Task UpdateItemQuantityAsync(int cartId, int itemId, int newQuantity);

        Task DeleteItem (int itemId, int cartId);
    }
}
