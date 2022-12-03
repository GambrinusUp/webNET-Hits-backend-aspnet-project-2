using Microsoft.EntityFrameworkCore;
using FoodDelivery.Models.DTO;
using FoodDelivery.Models;
using FoodDelivery.Models.Enum;
using Microsoft.AspNetCore.Mvc;

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

            if(vegetarian == true)
                dishes = _context.Dishes.Where(x => x.Vegetarian == vegetarian).OrderBy(x => x.Price).ToList();
            else
                dishes = _context.Dishes.OrderBy(x => x.Price).ToList();

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
