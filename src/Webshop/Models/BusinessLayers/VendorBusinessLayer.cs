using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Localization;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Controllers;
using Webshop.Models;

namespace Webshop.Models.BusinessLayers
{
    public class VendorBusinessLayer
    {
        private WebShopRepository _context;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly IStringLocalizer<ArticleController> _localizer;


        public VendorBusinessLayer()
        {

        }
        public VendorBusinessLayer(IStringLocalizer<ArticleController> localizer, WebShopRepository context, IHostingEnvironment hostEnviroment)
        {
            _context = context;
            _hostEnvironment = hostEnviroment;
            _localizer = localizer;
        }
        internal async Task<VendorModel> GetVendors(string name, string url, bool isactive, VendorModel vendor )
        {
            var nameInput = vendor.VendorName;
            var exists = _context.Vendors.ToList().Where(x => x.VendorName == nameInput).Select(x => x.VendorName).FirstOrDefault();
            do while (nameInput == exists)
                {
                    ArticleController create = new ArticleController(_context, _hostEnvironment,_localizer);
                    create.Create();
                }
            while (false);
            var v = _context.Vendors.ToList().Select(x => x.VendorID).Count();
            if (v == 0)
            {
                int tempV = 9001;
                vendor.VendorID = tempV;
            }
            else
            {
                var getLastID = _context.Vendors.ToList().OrderBy(x => x.VendorID).Select(x => x.VendorID).Last();
                vendor.VendorID = getLastID + 1;
            }
            _context.Add(vendor);
            await _context.SaveChangesAsync();
            return vendor;
        }
    }
}