﻿using FoodDelivery.Models.DTO;
using FoodDelivery.Models;

namespace FoodDelivery.Services
{
    public interface IOrderService
    {
        OrderCreateDTO? CreateOrderFromBasket(string token);
    }

    public class OrderService : IOrderService
    {
        private readonly Context _context;

        public OrderService(Context context)
        {
            _context = context;
        }

        public OrderCreateDTO? CreateOrderFromBasket(string token)
        {
            var user = _context.GetUserByToken(token);
            if (user == null)
                return null;

            var basket = user.Cart;
            if (basket.Count == 0)
                return null;

            var order = ConverterDTO.Order(basket, user.Address);

            user.Orders.Add(order);
            user.Cart.Clear();
            _context.SaveChanges();

            return new OrderCreateDTO
            {
                Address = order.Address,
                DeliveryTime = order.DeliveryTime
            };
        }
    }
}
