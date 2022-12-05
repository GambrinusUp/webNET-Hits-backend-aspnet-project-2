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

        /*[HttpGet]
        public ActionResult<BasketDTO> GetUserCart()
        {

        }*/

        /*[HttpPost("dish/{dishId}")]
        public IActionResult AddDish(Guid Id)
        {
            string token = Request.Headers["Authorization"].ToString().Split(' ')[1];
            if (_logoutService.IsUserLogout(token))
                return Unauthorized();

            //try catch
            if(_basketService.AddDishToCart(Id, token))
                return Ok();
        }*/

    }
}
