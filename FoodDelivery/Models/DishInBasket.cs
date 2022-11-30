namespace FoodDelivery.Models
{
    public class DishInBasket
    {
        public Guid Id { get; set; }
        
        public User User { get; set; }

        public ICollection<NumberOfDishes> Dishes = new HashSet<NumberOfDishes>();
    }
}
