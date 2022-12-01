using FoodDelivery.Models;
using FoodDelivery.Models.DTO;

namespace FoodDelivery.Services
{
    public class ConverterDTO
    {
        public static LoginCredentials Login(UserRegisterDTO model)
        {
            return new LoginCredentials { Email = model.Email, Password = model.Password };
        }

        public static User Register(UserRegisterDTO model)
        {
            return new User
            {
                FullName = model.FullName,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                Address = model.Address,
                Email = model.Email,
                Password = model.Password,
                PhoneNumber = model.PhoneNumber
            };
        }
    }
}
