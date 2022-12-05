using FoodDelivery.Models;
using FoodDelivery.Models.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Diagnostics;

namespace FoodDelivery.Services
{
    public interface IBasketService
    {
        //Task<string> AddDishToCart(Guid id, string token);
        string AddDishToCart(Guid id, string token);
        BasketDTO? GetUserCart(string token);
    }

    public class BasketService : IBasketService
    {
        private readonly Context _context;

        public BasketService(Context context)
        {
            _context = context;
        }

        public string AddDishToCart(Guid id, string token)
        {
            var user = _context.GetUserByToken(token);
            var dish = _context.GetDishById(id);
            if (user == null || dish == null)
                return "user or dish are not exists";

            var dishInBasket = new DishBasket();
            if (user.Cart.Count != 0)
            {
                foreach(DishBasket dishBasket in user.Cart)
                {
                    if(dishBasket.Id == id)
                    {
                        dishBasket.Amount = dishBasket.Amount + 1;
                        dishBasket.TotalPrice = dish.Price * dishBasket.Amount;
                        _context.SaveChanges();
                        return "dish in basket";
                    }
                }
                dishInBasket = new DishBasket
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    TotalPrice = dish.Price,
                    Amount = 1,
                    Image = dish.Image
                };
                _context.DishBasket.Add(dishInBasket);
                user.Cart.Add(dishInBasket);
                _context.SaveChanges();
                return "dish in basket";
            }
            else
            {
                dishInBasket = new DishBasket
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Price = dish.Price,
                    TotalPrice = dish.Price,
                    Amount = 1,
                    Image = dish.Image
                };
                _context.DishBasket.Add(dishInBasket);
                user.Cart.Add(dishInBasket);
                _context.SaveChanges();
                return "dish in basket";
            }

            /*var dishInBasket = new DishBasket
            {
                Id = dish.Id,
                Name = dish.Name,
                Price = dish.Price,
                TotalPrice = dish.Price,
                Amount = 1,
                Image = dish.Image
            };*/

            
            //
        }

        public BasketDTO? GetUserCart(string token)
        {
            var user = _context.GetUserByToken(token);
            if (user == null)
                return null;

            var basket = user.Cart;

            return ConverterDTO.Cart(basket);
        }
    }
}
