using Application.CartItems.Dto.Request;
using Application.CartItems.Dto.Response;
using Application.Carts;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public CartItemService(ICartItemRepo repo, ICartRepo cartRepo, 
            ITicketTypeRepo ticketTypeRepo, ICartService cartService, IMapper mapper)
        {
            _repo = repo;
            _cartRepo = cartRepo;
            _ticketTypeRepo = ticketTypeRepo;
            _cartService = cartService;
            _mapper = mapper;
        }

        public async Task UpdateItemQuantityAsync(int userId, int itemId, int newQuantity)
        {
            if (itemId <= 0)
                throw new ArgumentOutOfRangeException("Item Id must be a positive integer.");

            if (userId <= 0)
                throw new ArgumentOutOfRangeException("User Id must be a positive integer.");

            if (newQuantity < 0)
                throw new ArgumentOutOfRangeException("New quantity must be a non-negative integer.");

            var item = await OwnershipCheckAsync(userId, itemId);
            
            var additionalQuantity = newQuantity - item.Quantity;

            if (newQuantity == item.Quantity)
                return;

            else if (newQuantity > 0 && ! await _ticketTypeRepo.IsThereAvaliableTicketsAsync(item.TicketTypeId, additionalQuantity))
                throw new NotEnoughTicketException("Not enough tickets available.");

            else if (newQuantity == 0)
                await _repo.DeleteAsync(item);

            else 
            {
                // update the quantity of the item
                item.Quantity = newQuantity;
                await _repo.UpdateAsync(item);
            }
                
            
            await _cartRepo.UpdateTotalAmount(item.CartId, await _repo.CalculateCartTotalPrice(item.CartId));

        } 
        
        public async Task AddAsync (int userId, AddCartItemDto newItem)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException("User Id must be a positive integer.");

            if (newItem is null)
                throw new ArgumentNullException("Cart item payload can't be null.");

            if (!await _ticketTypeRepo.IsThereAvaliableTicketsAsync(newItem.TicketTypeId, newItem.Quantity))
                throw new NotEnoughTicketException("Not enough tickets available.");

            var cartId = await _cartService.CreateCartAsync(userId);

            // if item existed return its Id if not return 0
            var item = await _repo.IsCartItemRepeated(cartId, newItem.TicketTypeId);

            if (item == null)
            {
                var cartItem = new CartItem
                {
                    CartId = cartId,
                    TicketTypeId = newItem.TicketTypeId,
                    Quantity = newItem.Quantity
                };

                await _repo.AddAsync(cartItem);    
            }

            else
            {
                item.Quantity += newItem.Quantity;
                await _repo.UpdateAsync(item);
            }
            
            await _cartRepo.UpdateTotalAmount(cartId, await _repo.CalculateCartTotalPrice(cartId));

        }

        public async Task<CartItem> OwnershipCheckAsync (int userId, int itemId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException("User Id must be a positive integer.");

            if (itemId <= 0)
                throw new ArgumentOutOfRangeException("Item Id must be a positive integer.");

            var item = await _repo.GetByIdAsync(itemId);

            if (item == null)
                throw new KeyNotFoundException($"Cart item with Id {itemId} is not existed.");

            if (item.Cart == null)
                throw new KeyNotFoundException($"Cart with Id {item.CartId} is not existed.");

            if (userId != item.Cart.UserId)
                throw new UnauthorizedAccessException("User is not authorized to update this cart item.");

            return item;
        }

        public async Task DeleteAsync(int userId, int itemId)
        {
            if (itemId <= 0)
                throw new ArgumentOutOfRangeException("Item Id must be a positive integer.");

            if (userId <= 0)
                throw new ArgumentOutOfRangeException("User Id must be a positive integer.");

            var item = await OwnershipCheckAsync(userId, itemId);

            await _repo.DeleteAsync(item);

            await _cartRepo.UpdateTotalAmount(item.CartId, await _repo.CalculateCartTotalPrice(item.CartId));
        }

        public async Task<IEnumerable<ItemResponseDto>> GetCartItemsByCartIdAsync(int userId)
        {
            var cart = await _cartService.GetCurrentCartByUserIdAsync(userId);
            
            if (cart == null)
                throw new KeyNotFoundException($"No active cart found for user with Id {userId}.");

            var items = await _repo.GetItemsByCartId(cart.Id);

            return _mapper.Map<IEnumerable<ItemResponseDto>>(items);    
        }
    }
}
