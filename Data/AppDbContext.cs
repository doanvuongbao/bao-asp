using bao_asp.Controllers;
using bao_asp.Models;
using Microsoft.EntityFrameworkCore;

namespace bao_asp.Data
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Student> Students { get; set; }
       // public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>()
                .HasKey(x => new { x.OrderId, x.BookId });
            // 2. THÊM VÀO ĐÂY: Cấu hình kiểu decimal để tránh lỗi làm tròn tiền
            modelBuilder.Entity<Book>()
                .Property(b => b.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalPrice)
                .HasColumnType("decimal(18,2)");
        }
    }
}