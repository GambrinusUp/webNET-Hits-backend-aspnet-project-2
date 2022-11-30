namespace FoodDelivery.Models.DTO
{
    public class DishPagedListDTO
    {
        public ICollection<DishDTO>? Dishes { get; set; }

        public PageInfoDTO PageInfo { get; set; }
    }
}
