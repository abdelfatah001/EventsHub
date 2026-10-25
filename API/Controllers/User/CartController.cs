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
        public async Task<ActionResult<int>> GetOrCreateCart(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid User ID. ID must be a positive integer." });

            var cartId = await _cartService.CreateCart(userId);

            return Ok(cartId);
        }

        [Authorize(Roles = "User")]
        [HttpPut("MarkCartAsOrdered/{cartId}")]
        public async Task<ActionResult> MarkCartAsOrdered(int cartId)
        {
            if (cartId <= 0)
                return BadRequest(new { message = "Invalid Cart ID. ID must be a positive integer." });

            await _cartService.MarkCartAsOrdered(cartId);

            return Ok();
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetCartItems/{cartId}")]
        public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetCartItems(int cartId)
        {
            if (cartId <= 0)
                return BadRequest(new { message = "Invalid Cart ID. ID must be a positive integer." });

            var items = await _cartItemService.GetCartItemsByCartIdAsync(cartId);

            return Ok(items);
        }

        [Authorize(Roles = "User")]
        [HttpPost("AddItemToCart/{cartId}")]
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
        [HttpPut("UpdateCartItem/{cartId}/{itemId}")]
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
        [HttpDelete("RemoveItemFromCart/{cartId}/{itemId}")]
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
