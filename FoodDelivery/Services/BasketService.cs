using FoodDelivery.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace FoodDelivery.Services
{
    public interface IBasketService
    {
        Task<bool> AddDishToCart(Guid id, string token);
    }

    public class BasketService : IBasketService
    {
        private readonly Context _context;

        public BasketService(Context context)
        {
            _context = context;
        }

        public async Task<bool> AddDishToCart(Guid id, string token)
        {
            var user = _context.GetUserByToken(token);
            if (user == null)
                return false;

            //find the dish
            var dish = _context.GetDishById(id);
            if (dish == null)
                return false;

            //найти блюдо уже в существующей корзине

            return false;

        }
    }
}
