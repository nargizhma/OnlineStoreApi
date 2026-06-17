using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStoreApi.Business.DTOs;
using OnlineStoreApi.Business.Services;
using System.Security.Claims;

namespace OnlineStoreApi.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all orders for the authenticated user
        /// </summary>
        [HttpGet("my-orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetMyOrders()
        {
            try
            {
                var userId = GetUserId();
                var orders = await _service.GetOrdersByUserAsync(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
        {
            try
            {
                var orders = await _service.GetAllOrdersAsync();
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            try
            {
                var userId = GetUserId();
                var order = await _service.GetOrderByIdAsync(id, userId);
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("customer/{customerId}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetByCustomer(int customerId)
        {
            try
            {
                var orders = await _service.GetOrdersByCustomerAsync(customerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create([FromBody] CreateOrderDto dto)
        {
            try
            {
                var userId = GetUserId();
                // Create order for authenticated user instead of using CustomerId from DTO
                var order = await _service.CreateOrderForUserAsync(userId);
                return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{orderId}/items")]
        public async Task<ActionResult<OrderDto>> AddItem(int orderId, [FromBody] CreateOrderDetailDto dto)
        {
            try
            {
                var userId = GetUserId();
                await _service.AddItemToOrderAsync(orderId, dto, userId);
                var order = await _service.GetOrderByIdAsync(orderId, userId);
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{orderId}/items/{itemId}")]
        public async Task<ActionResult<OrderDto>> RemoveItem(int orderId, int itemId)
        {
            try
            {
                var userId = GetUserId();
                await _service.RemoveItemFromOrderAsync(orderId, itemId, userId);
                var order = await _service.GetOrderByIdAsync(orderId, userId);
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("{orderId}/status")]
        public async Task<ActionResult<OrderDto>> ChangeStatus(int orderId, [FromBody] ChangeOrderStatusDto dto)
        {
            try
            {
                var userId = GetUserId();
                await _service.ChangeOrderStatusAsync(orderId, dto, userId);
                var order = await _service.GetOrderByIdAsync(orderId, userId);
                return Ok(order);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var userId = GetUserId();
                await _service.DeleteOrderAsync(id, userId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Extract UserId from JWT claims
        /// </summary>
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst("UserId") ?? User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                throw new UnauthorizedAccessException("Invalid or missing user ID in token");
            return userId;
        }
    }
}
