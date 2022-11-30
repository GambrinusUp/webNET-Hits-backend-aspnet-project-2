using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Models
{
    public class Context: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
        public Context(DbContextOptions<Context> options): base(options)
        {
            Database.EnsureCreated();
        }
    }
}
