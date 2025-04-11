using Microsoft.EntityFrameworkCore;
using ODataDemo.Models;

namespace ODataDemo.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<InventTable> InventTables { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // กำหนดชื่อตารางใน SQL Server
            modelBuilder.Entity<InventTable>()
                .ToTable("InventTable");

            // กำหนด Primary Key
            modelBuilder.Entity<InventTable>()
                .HasKey(i => i.ItemId);

            // กำหนดขนาดของ string properties (ถ้าต้องการ)
            modelBuilder.Entity<InventTable>()
                .Property(i => i.ItemId)
                .HasMaxLength(50);

            modelBuilder.Entity<InventTable>()
                .Property(i => i.ItemName)
                .HasMaxLength(200);

            base.OnModelCreating(modelBuilder);
        }
    }
}