using Application.CartItems;
using Application.CartItems.Dto.Request;
using Application.CartItems.Dto.Response;
using Application.Carts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.UserControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : BaseController
    {
        private readonly ICartService _cartService;
        private readonly ICartItemService _cartItemService;

        public CartController(ICartService cartService, ICartItemService cartItemService)
        {
            _cartService = cartService;
            _cartItemService = cartItemService;
        }

        [Authorize(Roles = "User")]
        [HttpPost("GetOrCreateCart/{userId}")]
        public async Task<ActionResult<int>> GetOrCreateCart()
        {
            if (GetAuthenticatedUserId() is not int userId)
                return Unauthorized(new { message = "User ID not found in token." });

            var cartId = await _cartService.CreateCart(userId);

            return Ok(cartId);
        }

        [Authorize(Roles = "User")]
        [HttpPut("MarkCartAsOrdered")]
        public async Task<ActionResult> MarkCartAsOrdered()
        {
            if (GetAuthenticatedUserId() is not int userId)
                return Unauthorized(new { message = "User ID not found in token." });

            await _cartService.MarkCartAsOrdered(userId);

            return Ok();
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetCartItems")]
        public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetCartItems()
        {
            if (GetAuthenticatedUserId() is not int userId)
                return Unauthorized(new { message = "User ID not found in token." });

            var items = await _cartItemService.GetCartItemsByCartIdAsync(userId);

            return Ok(items);
        }

        [Authorize(Roles = "User")]
        [HttpPost("AddItemToCart")]
        public async Task<ActionResult<ItemResponseDto>> AddItemToCart([FromBody] AddCartItemDto dto)
        {
            if (dto is null)
                return BadRequest(new { message = "Request body cannot be null." });

            if (GetAuthenticatedUserId() is not int userId)
                return Unauthorized(new { message = "User ID not found in token." });

            await _cartItemService.AddAsync(userId, dto);
           
            return Ok();
        }


        [Authorize(Roles = "User")]
        [HttpPut("UpdateCartItem/{itemId}")]
        public async Task<ActionResult> UpdateItemQuantity (int itemId, int newQuantity)
        {
            if (itemId <= 0)
                return BadRequest(new { message = "Invalid Item ID. ID must be a positive integer." });
            if (newQuantity <= 0)
                return BadRequest(new { message = "Invalid quantity. Quantity must be a positive integer." });

            if (GetAuthenticatedUserId() is not int userId)
                return Unauthorized(new { message = "User ID not found in token." });

            await _cartItemService.UpdateItemQuantityAsync(userId, itemId, newQuantity);

            return Ok();
        }

        [Authorize(Roles = "User")]
        [HttpDelete("RemoveItemFromCart/{itemId}")]
        public async Task<ActionResult> DeleteItemFromCart (int itemId)
        {
            if (itemId <= 0)
                return BadRequest(new { message = "Invalid Item ID. ID must be a positive integer." });

            if (GetAuthenticatedUserId() is not int userId)
                return Unauthorized(new { message = "User ID not found in token." });

            await _cartItemService.DeleteItem(userId, itemId);

            return Ok();
        }
    }
}
