using Microsoft.EntityFrameworkCore;
using WorkProj.Entity.Entity;
using WorkProj.Services;

namespace WorkProj.Entity
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Виконуйте налаштування моделі бази даних
            // Наприклад, встановлюйте обмеження, зв'язки тощо
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.FavoriteBooks)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserFavoriteBooks"));

            modelBuilder.Entity<User>()
                .HasMany(u => u.CartBooks)
                .WithMany()
                .UsingEntity(j => j.ToTable("UserCartBooks"));

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Book)
                .WithMany(b => b.Orders)
                .HasForeignKey(o => o.BookId);

            modelBuilder.Entity<Book>()
                .HasMany(b => b.Orders)
                .WithOne(o => o.Book)
                .HasForeignKey(o => o.BookId);
        }

        public void FillDB()
        {
            DbInitializer.Initialize(this);
        }
    }
}
