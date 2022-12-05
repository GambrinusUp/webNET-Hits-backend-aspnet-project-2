using FoodDelivery.Services;
using Microsoft.AspNetCore.Mvc;
using FoodDelivery.Models.DTO;

namespace FoodDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly ILogoutService _logoutService;

        public BasketController(IBasketService basketService, ILogoutService logoutService)
        {
            _basketService = basketService;
            _logoutService = logoutService;
        }

        [HttpGet]
        public ActionResult<BasketDTO> GetUserCart()
        {
            string token = Request.Headers["Authorization"].ToString().Split(' ')[1];
            if (_logoutService.IsUserLogout(token))
                return Unauthorized();

            var cart = _basketService.GetUserCart(token);
            if (cart == null)
                return BadRequest(new {message = "Empty cart"});
            else
                return Ok(cart);
            //return BadRequest();
        }

        [HttpPost("dish/{dishId}")]
        public IActionResult AddDish(Guid dishId)
        {
            string token = Request.Headers["Authorization"].ToString().Split(' ')[1];
            if (_logoutService.IsUserLogout(token))
                return Unauthorized();

            string status = _basketService.AddDishToCart(dishId, token);

            //var cart = _basketService.GetUserCart(token);
            return Ok(status);
        }

        [HttpDelete("dish/{dishId}")]
        public IActionResult DeleteDish(Guid dishId, bool increase = false) 
        {
            string token = Request.Headers["Authorization"].ToString().Split(' ')[1];
            if (_logoutService.IsUserLogout(token))
                return Unauthorized();

            string status = _basketService.DeleteDishFromCart(dishId, increase, token);

            return Ok(status);
        }
    }
}
