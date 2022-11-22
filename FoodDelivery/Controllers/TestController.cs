using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "This is get";
        }
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"This is GET with id = {id}";
        }
        [HttpDelete]
        public string Delete(int id)
        {
            return $"This is GET with id = {id}";
        }
    }
}
