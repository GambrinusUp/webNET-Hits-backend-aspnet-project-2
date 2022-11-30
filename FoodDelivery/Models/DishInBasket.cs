namespace FoodDelivery.Models
{
    public class DishInBasket
    {
        public Guid Id { get; set; }
        
        public User User { get; set; }

        public Dictionary<Dish, int> people = new Dictionary<Dish, int>();
    }
}
