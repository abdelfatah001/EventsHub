using Application.CartItems.Dto.Request;
using Application.CartItems.Dto.Response;
using Domain.Entities;

namespace Application.CartItems
{
    public interface ICartItemService
    {
        Task AddAsync(int userId, AddCartItemDto newItem);
        Task<IEnumerable<ItemResponseDto>> GetCartItemsByCartIdAsync(int userId);
        Task UpdateItemQuantityAsync(int cartId, int itemId, int newQuantity);

        Task DeleteItem (int itemId, int cartId);
    }
}
