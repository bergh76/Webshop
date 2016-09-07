using Microsoft.EntityFrameworkCore;
namespace Webshop.Models
{
    public class WebShopRepository : DbContext
    {
        public WebShopRepository(DbContextOptions<WebShopRepository> options)
            : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductsCategories { get; set; }


    }
}
