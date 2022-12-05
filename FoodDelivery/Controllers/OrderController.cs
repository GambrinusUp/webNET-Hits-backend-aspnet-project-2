using FoodDelivery.Services;
using Microsoft.AspNetCore.Mvc;

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
    }
}
