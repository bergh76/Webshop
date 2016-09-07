using Microsoft.EntityFrameworkCore;


namespace Webshop.Models
{
    public class WebShopRepository : DbContext
    {
        public WebShopRepository(DbContextOptions<WebShopRepository> options)
            : base(options)
        { }

        public DbSet<ArticleModel> Articles { get; set; }
        public DbSet<VendorModel> Vendors { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<SubCategory> SubCategoryies { get; set; }
        public DbSet<ImageModel> Images { get; set; }
        //public DbSet<Cart> Carts { get; set; }
        //public DbSet<OrderModel> Orders { get; set; }
        //public DbSet<OrderDetail> OrderDetails { get; set; }

    }
}
