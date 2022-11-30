using FoodDelivery.Models.Enum;
using System.ComponentModel.DataAnnotations;
using FoodDelivery.Models.DTO;
using Humanizer.Localisation;

namespace FoodDelivery.Models
{
    public class Dish
    {
        public Guid Id { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "The name is too short.")]
        public string Name { get; set; }

        public string? Description { get; set; }

        public double Price { get; set; }

        public bool Vegetarian { get; set; } = false;
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();

        [EnumDataType(typeof(DishCategory), ErrorMessage = "Invalid dish category")]
        public DishCategory DishCategory { get; set; }
    }
}
