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

            var OrderCreateDTO = _orderService.CreateOrderFromBasket(token);

            if(OrderCreateDTO != null)
                return Ok(OrderCreateDTO);
            else
                return BadRequest();
        }
    }
}
