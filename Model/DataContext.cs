using Microsoft.EntityFrameworkCore;

namespace WebAPI6_Demo.Model
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "Teacher" }
            );
             
            //When we first time add code migration then default admin role is added.
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "umesh",
                    LastName = "Admin",
                    Email = "ugw@narola.email",
                    Phone = "+46733284906",
                    PasswordHashed = BCrypt.Net.BCrypt.HashPassword("umesh123@admin"),
                    RoleId = 1
                }
            );
        }
        // Define all you're entity here
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    }
}
