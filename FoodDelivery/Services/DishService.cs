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
    }
}
