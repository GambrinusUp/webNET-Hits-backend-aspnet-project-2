using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Models
{
    public class Context: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Dish> Dishes{ get; set; }
        public DbSet<DishInBasket> DishInBaskets { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<UserReview> RatingUserReviews { get; set; }
        public DbSet<NumberOfDishes> NumberOfDishes { get; set; }

        public Context(DbContextOptions<Context> options): base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasKey(x => x.Id);
            modelBuilder.Entity<Dish>().HasKey(x => x.Id);
            modelBuilder.Entity<DishInBasket>().HasKey(x => x.Id);
            modelBuilder.Entity<Order>().HasKey(x => x.Id);
            modelBuilder.Entity<UserReview>().HasKey(x => x.Id);
            modelBuilder.Entity<NumberOfDishes>().HasKey(x => x.Id);
        }

    }
}
