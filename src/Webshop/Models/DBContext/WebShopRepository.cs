using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Webshop.Models
{
    public class WebShopRepository : IdentityDbContext<ApplicationUser>
    {
        public WebShopRepository(DbContextOptions<WebShopRepository> options)
            : base(options)
        { }

        public DbSet<Articles> Articles { get; set; }
        public DbSet<VendorModel> Vendors { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<SubCategoryModel> SubCategories { get; set; }
        public DbSet<ImageModel> Images { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<ArticleTranslation> ArticleTranslations { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ArticleTranslation>()
                .HasKey(c => new { c.ArticleId, c.LangCode });
            //modelBuilder.Entity<VendorModel>()
            //    .HasKey(c => new { c.VendorID, c.LangCode });
            //modelBuilder.Entity<ProductModel>()
            //   .HasKey(c => new { c.ProductID, c.LangCode });
            //modelBuilder.Entity<CategoryModel>()
            //   .HasKey(c => new { c.CategoryID, c.LangCode });
            //modelBuilder.Entity<SubCategoryModel>()
            //   .HasKey(c => new { c.SubCategoryID, c.LangCode });
        }
    }
}
