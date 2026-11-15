using Application.Orders;
using Application.Orders.Dto;
using Application.Payments;
using Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.UserControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : BaseController
    {

        private readonly IOrderService _orderService;
        private readonly OrderCheckout _orderCheckout;
        private readonly PaymobIntentService _paymobIntentService;
        private readonly IUserService _userService;

        public OrderController(IOrderService orderService, PaymobIntentService paymobIntentService, IUserService userService)
        {
            _orderService = orderService;
            _paymobIntentService = paymobIntentService;
            _userService = userService;
            _orderCheckout = new OrderCheckout(_orderService, _paymobIntentService);

        }

        [Authorize(Roles = "User")]
        [HttpPost("Checkout-Order")]
        public async Task<IActionResult> CheckoutOrderAsync()
        {
            if (GetAuthenticatedUserId() is not int userId)
                return Unauthorized("User is not authenticated.");

            var user = await _userService.GetUserByIdAsync(userId);

            if (user is null)
                return NotFound("User not found.");

            var result = await _orderCheckout.CheckoutOrderAsync(user);

            return Ok(result);
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetOrders")]
        public async Task<ActionResult<IEnumerable<GetOrderDto>>> GetOrdersAsync()
        {
            if (GetAuthenticatedUserId() is not int userId)
                return Unauthorized("User is not authenticated.");

            var orders = await _orderService.GetOrders(userId);

            if (orders is null)
                return NotFound("There is no founded orders.");

            return Ok(orders);

        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrdersByUserId")]
        public async Task<ActionResult<IEnumerable<GetDetailedOrderDto>>> GetOrdersOfUserAsync(int userId)
        {
            if (userId <= 0)
                return BadRequest("Invalid user ID.");

            var orders = await _orderService.GetUserOrders(userId);

            if (orders is null)
                return NotFound("User has no orders.");

            return Ok(orders);
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetUserOrderById")]
        public async Task<ActionResult> GetUserOrderById(int orderId)
        {
            if (orderId <= 0)
                return BadRequest("Invalid order ID.");

            if (GetAuthenticatedUserId() is not int userId)
                return Unauthorized("User is not authenticated.");

            if (!await _orderService.IsOrderOwnedByUserAsync(orderId, userId))
                return Forbid("User does not own this order.");

            var orderSummary = await _orderService.GetOrderSummaryWithItems(orderId);

            if (orderSummary is null)
                return NotFound("Order not found.");

            return Ok(orderSummary);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrderById")]
        public async Task<ActionResult<OrderSummary>> GetOrderById(int orderId)
        {
            if (orderId <= 0)
                return BadRequest("Invalid order ID.");

            var orderSummary = await _orderService.GetOrderSummaryWithItems(orderId);

            if (orderSummary is null)
                return NotFound("Order not found.");

            return Ok(orderSummary);
        }

        [Authorize(Roles = "User")]
        [HttpGet("GetUserOrderItemById")]
        public async Task<ActionResult<ItemDto>> GetUserOrderItemById(int orderItemId)
        {
            if (orderItemId <= 0)
                return BadRequest("Invalid order item ID.");

            if (GetAuthenticatedUserId() is not int userId)
                return Unauthorized("User is not authenticated.");

            var orderItem = await _orderService.GetOrderItemByIdAsync(orderItemId);

            if (orderItem is null)
                return NotFound("Order item not found.");

            if (!await _orderService.IsOrderOwnedByUserAsync(orderItem.OrderId, userId))
                return Forbid("User does not own this order item.");

            return Ok(orderItem);
        }



        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrderItemById")]
        public async Task<ActionResult<ItemDto>> GetOrderItemById(int orderItemId)
        {
            if (orderItemId <= 0)
                return BadRequest("Invalid order item ID.");

            var orderItem = await _orderService.GetOrderItemByIdAsync(orderItemId);

            if (orderItem is null)
                return NotFound("Order item not found.");

            return Ok(orderItem);
        }

    }
}