using FoodDelivery.Models.DTO;
using FoodDelivery.Models;

namespace FoodDelivery.Services
{
    public interface IOrderService
    {

    }

    public class OrderService : IOrderService
    {
        private readonly Context _context;

        public OrderService(Context context)
        {
            _context = context;
        }

    }
}
