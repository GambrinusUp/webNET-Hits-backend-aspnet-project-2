﻿using FoodDelivery.Models.DTO;
using FoodDelivery.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DeliveryTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime OrderTime { get; set; }

        [EnumDataType(typeof(OrderStatus), ErrorMessage = "Invalid order status")]
        public OrderStatus Status { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "The address is too short.")]
        public string Address { get; set; }

        public DishInBasket? DishInBasket { get; set; }
    }
}
