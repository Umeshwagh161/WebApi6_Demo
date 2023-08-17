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
                new Role { Id = 1, Name = "Student" },
                new Role { Id = 2, Name = "Admin" }
            );
                      

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    FirstName = "umesh",
                    LastName = "Admin",
                    Email = "ugw@narola.email",
                    Phone = "+46733284906",
                    PasswordHashed = BCrypt.Net.BCrypt.HashPassword("umesh123@admin"),
                    RoleId = 2
                }
            );
        }

        public DbSet<Role> Roles => Set<Role>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Student> Students => Set<Student>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    }
}
