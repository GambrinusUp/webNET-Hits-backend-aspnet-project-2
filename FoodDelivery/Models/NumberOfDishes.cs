namespace FoodDelivery.Models
{
    public class NumberOfDishes
    {
        public Guid Id { get; set; }

        public Dish Dish { get; set; }

        public int Number { get; set; }

        public User User { get; set; }
    }
}
