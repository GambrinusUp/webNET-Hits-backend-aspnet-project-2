using FoodDelivery.Services;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Models.DTO;

namespace FoodDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogoutService _logoutService;

        public OrderController(IOrderService orderService, ILogoutService logoutService)
        {
            _orderService = orderService;
            _logoutService = logoutService;
        }

        [HttpPost]
        public ActionResult<OrderCreateDTO> CreateOrder()
        {
            string token = Request.Headers["Authorization"].ToString().Split(' ')[1];
            if (_logoutService.IsUserLogout(token))
                return Unauthorized();

            var orderCreateDTO = _orderService.CreateOrderFromBasket(token);

            if(orderCreateDTO != null)
                return Ok(orderCreateDTO);
            else
                return BadRequest();
        }

        [HttpPost("{id}")]
        public ActionResult<OrderDTO> GetOrderById(Guid id)
        {
            string token = Request.Headers["Authorization"].ToString().Split(' ')[1];
            if (_logoutService.IsUserLogout(token))
                return Unauthorized();

            var order = _orderService.GetOrderById(id, token);
            return Ok(order);
        }
    }
}
