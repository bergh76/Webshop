using System.ComponentModel.DataAnnotations;

namespace Webshop.Models
{
    public class SubCategory
    {
        [Key]
        public int ID { get; set; }
        public int SubCategoryID { get; set; }
        public string SubCategoryName { get; set; }
        public bool ISActive { get; set; }
        //public IEnumerable<SubCategory> subCatNameList { get; set; }
        //private ApplicationDbContext _context;

        //public SubCategory(ApplicationDbContext context)
        //{
        //    _context = context;
        //}

        //public SubCategory()
        //{
        //    var subCatnameList = new SubCategory();
        //   subCatNameList = _context.SubCategory.ToList();

        //}

    }
}