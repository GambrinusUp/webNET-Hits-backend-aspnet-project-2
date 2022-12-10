using Microsoft.EntityFrameworkCore;
using FoodDelivery.Models.DTO;
using FoodDelivery.Models;
using FoodDelivery.Models.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace FoodDelivery.Services
{
    public interface IDishService
    {
        DishPagedListDTO? GetDishPagedList(DishCategory[] categories, bool vegetarian,
            DishSorting sorting, int page);
        DishDTO? GetDish(Guid id);
        bool Check(Guid id, string token);
        string Set(Guid id, string token, int rating);
    }
    public class DishService : IDishService
    {
        private readonly Context _context;

        public DishService(Context context)
        {
            _context = context;
        }

        public DishPagedListDTO? GetDishPagedList(DishCategory[] categories, bool vegetarian,
            DishSorting sorting, int page)
        {
            List<Dish> dishes = new();

            //сделать сортировку
            if (vegetarian == true)
            {
                //вынести в context
                dishes = sorting switch
                {
                    DishSorting.NameAsc => _context.Dishes.Where(x => x.Vegetarian == vegetarian).OrderBy(s => s.Name).ToList(),
                    DishSorting.NameDesc => _context.Dishes.Where(x => x.Vegetarian == vegetarian).OrderByDescending(s => s.Name).ToList(),
                    DishSorting.PriceAsc => _context.Dishes.Where(x => x.Vegetarian == vegetarian).OrderBy(s => s.Price).ToList(),
                    DishSorting.PriceDesc => _context.Dishes.Where(x => x.Vegetarian == vegetarian).OrderByDescending(s => s.Price).ToList(),
                    DishSorting.RatingAsc => _context.Dishes.Where(x => x.Vegetarian == vegetarian).OrderBy(s => s.Rating).ToList(),
                    DishSorting.RatingDesc => _context.Dishes.Where(x => x.Vegetarian == vegetarian).OrderByDescending(s => s.Rating).ToList(),
                };
            }
            else
            {
                dishes = sorting switch
                {
                    DishSorting.NameAsc => _context.Dishes.OrderBy(s => s.Name).ToList(),
                    DishSorting.NameDesc => _context.Dishes.OrderByDescending(s => s.Name).ToList(),
                    DishSorting.PriceAsc => _context.Dishes.OrderBy(s => s.Price).ToList(),
                    DishSorting.PriceDesc => _context.Dishes.OrderByDescending(s => s.Price).ToList(),
                    DishSorting.RatingAsc => _context.Dishes.OrderBy(s => s.Rating).ToList(),
                    DishSorting.RatingDesc => _context.Dishes.OrderByDescending(s => s.Rating).ToList(),
                };
            }

            List <Dish> dishesOfPage = new();

            List<Dish> dishList = new();

            if(categories.Length != 0)
            {
                for (int i = 0; i < dishes.Count; i++)
                {
                    foreach (DishCategory category in categories)
                    {
                        if (dishes[i].DishCategory == category)
                            dishList.Add(dishes[i]);
                    }
                }
            }
            else
            {
                dishList = dishes;
            }

            PageInfoDTO pageInfo = new()
            {
                Size = 5,
                Count = (int)Math.Ceiling(dishList.Count() / 5.0),
                Current = page
            };

            if (page > pageInfo.Count)
            {
                return null;
            }

            if (page == pageInfo.Count)
                dishesOfPage = dishList.GetRange((page - 1) * pageInfo.Size, dishList.Count - (page - 1) * pageInfo.Size);
            else
                dishesOfPage = dishList.GetRange((page - 1) * pageInfo.Size, pageInfo.Size);

            return new DishPagedListDTO
            {
                Dishes = ConverterDTO.Dishes(dishesOfPage),
                Pagination = pageInfo
            };
        }

        public DishDTO? GetDish(Guid id)
        {
            var dish = _context.GetDishById(id);

            if (dish == null)
                return null;

            return ConverterDTO.Dish(dish);
        }

        public bool Check(Guid id, string token)
        {
            var user = _context.GetUserByToken(token);
            if (user == null)
                return false;

            var orders = user.Orders;
            if (orders.Count == 0)
                return false;

            foreach (var order in orders) 
            { 
                foreach(var dish in order.DishesInOrder)
                {
                    if (dish.IdOfDish == id.ToString())
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public string Set(Guid id, string token, int rating)
        {
            var user = _context.GetUserByToken(token);
            if (user == null)
                return "user not found";

            var dish = _context.GetDishById(id);
            if (dish == null)
                return "dish not found";

            if (Check(id, token))
            {
                var review = ConverterDTO.Review(user, dish, rating);
                double avgrating = 0;
                //проверка на существующий отзыв
                var reviews = _context.RatingUserReviews.Include(x => x.User).Include(x => x.Dish).ToList();
                foreach (var userreview in reviews)
                {
                    if(userreview.User.Id == user.Id && userreview.Dish.Id == dish.Id)
                    {
                        userreview.Rating = rating;
                        avgrating = 0;
                        foreach (var reviewscore in reviews)
                        {
                            if (reviewscore.Dish.Id == id)
                                avgrating += reviewscore.Rating;
                        }
                        avgrating = avgrating / reviews.Count();
                        dish.Rating = avgrating;
                        _context.SaveChanges();
                        return "rating changed";
                    }
                }
                _context.RatingUserReviews.Add(review);
                reviews.Add(review);
                _context.SaveChanges();
                avgrating = 0;
                //reviews = _context.RatingUserReviews.Include(x => x.User).Include(x => x.Dish).ToList();
                foreach (var reviewscore in reviews)
                {
                    if(reviewscore.Dish.Id == id)
                        avgrating += reviewscore.Rating;
                }
                avgrating = avgrating / reviews.Count();
                dish.Rating = avgrating;
                _context.SaveChanges();
                return reviews.Count().ToString();
            }

            return "error";
        }
    }
}
