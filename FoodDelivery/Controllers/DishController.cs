using FoodDelivery.Models.DTO;
using FoodDelivery.Models.Enum;
using FoodDelivery.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace FoodDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController : ControllerBase
    {
        private readonly IDishService _dishService;

        public DishController(IDishService dishService)
        {
            _dishService = dishService;
        }

        [HttpGet]
        public ActionResult<DishPagedListDTO> GetDishes([FromQuery] DishCategory []categories, [FromQuery] bool vegetarian, 
            [FromQuery] DishSorting sorting, [FromQuery] int page)
        {
            if (page <= 0)
                return BadRequest(new { errorText = "Incorrect page number" });
            try
            {
                DishPagedListDTO? pageList = _dishService.GetDishPagedList(categories, vegetarian, sorting, page);
                if (pageList == null)
                    return BadRequest(new { errorText = "Incorrect page number" });

                return Ok(pageList);
            }
            catch
            {
                //переделать на 500 код
                return BadRequest(new { errorText = "Internal error" });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<DishDTO> GetDish(Guid id)
        {
            try
            {
                DishDTO? dish = _dishService.GetDish(id);

                if(dish == null)
                    return NotFound(new { message = "Dish not found" });

                return Ok(dish);
            }
            catch
            {
                return BadRequest(new { errorText = "Internal error" });
            }
        }
    }
}
