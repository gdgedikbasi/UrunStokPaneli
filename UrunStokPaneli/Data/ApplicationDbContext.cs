using Microsoft.EntityFrameworkCore;
using UrunStokPaneli.Models;


namespace UrunStokPaneli.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; } //Product modelinden Products tablosu oluştur.

        public DbSet<Category> Categories { get; set; } //Category modelinden Categories tablosu oluştur.
    }
}
