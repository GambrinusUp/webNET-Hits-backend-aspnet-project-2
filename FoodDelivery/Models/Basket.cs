using FoodDelivery.Models.DTO;

namespace FoodDelivery.Models
{
    public class Basket
    {
        public Guid Id { get; set; }

        public ICollection<DishBasketDTO> Dishes { get; set; } = new List<DishBasketDTO>();

        public User User { get; set; }
    }
}
