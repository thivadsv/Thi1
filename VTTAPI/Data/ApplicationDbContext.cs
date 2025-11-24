using Microsoft.EntityFrameworkCore;
using VTTAPI.Models; // Đảm bảo đúng namespace của bạn

namespace VTTAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tạo DbSet để EF Core ánh xạ Product Class sang bảng Products trong DB
        public DbSet<Product> Products { get; set; } = null!;
    }
}