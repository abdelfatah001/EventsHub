using Application.CartItems.Dto.Request;
using Application.Carts;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.CartItems
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepo _repo;
        private readonly ICartRepo _cartRepo;
        private readonly ITicketTypeRepo _ticketTypeRepo;
        private readonly ICartService _cartService;

        public CartItemService(ICartItemRepo repo, ICartRepo cartRepo, ITicketTypeRepo ticketTypeRepo, ICartService cartService)
        {
            _repo = repo;
            _cartRepo = cartRepo;
            _ticketTypeRepo = ticketTypeRepo;
            _cartService = cartService;
        }

        public async Task AddAsync (int userId, AddCartItemDto newItem)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(nameof(userId));

            if (newItem is null)
                throw new ArgumentNullException("Cart item payload can't be null.");

            if (!await _ticketTypeRepo.IsThereAvaliableTicketsAsync(newItem.TicketTypeId, newItem.Quantity))
                throw new NotEnoughTicketException("Not enough tickets available.");

            var cartId = await _cartService.AddCart(userId);

            // if item existed return its Id if not return 0
            var itemId = await _repo.IsCartItemRepeated(cartId, newItem.TicketTypeId);

            if (itemId == 0)
            {
                var cartItem = new CartItem
                {
                    CartId = await _cartService.AddCart(userId),
                    TicketTypeId = newItem.TicketTypeId,
                    Quantity = newItem.Quantity
                };

                await _repo.AddAsync(cartItem);
            }

            else
            {
                await UpdateItemQuantityAsync(cartId, itemId, newItem.Quantity);
            }

        }

        public async Task UpdateItemQuantityAsync(int cartId, int itemId, int newQuantity)
        {
            if (itemId <= 0)
                throw new ArgumentOutOfRangeException("Item Id must be a positive integer.");

            if (newQuantity <= 0)
                throw new ArgumentOutOfRangeException("New quantity must be a positive integer.");

            var item = await _repo.GetById(itemId);

            if (item == null)
                throw new KeyNotFoundException($"Cart item with Id {itemId} is not existed.");

            item.Quantity = newQuantity;

            await _repo.UpdateAsync(item);

            await _cartRepo.UpdateTotalAmount(cartId, await _repo.CalculateCartTotalAmount(cartId));


        } 
        
        public async Task DeleteItem(int itemId, int cartId)
        {
            if (itemId <= 0)
                throw new ArgumentOutOfRangeException("Item Id must be a positive integer.");

            var item = await _repo.GetById(itemId);

            if (item is null)
                throw new KeyNotFoundException($"Item with Id {itemId} is not existed");

            await _repo.DeleteAsync(item);

            await _cartRepo.UpdateTotalAmount(cartId, await _repo.CalculateCartTotalAmount(cartId));
        }

    }
}
