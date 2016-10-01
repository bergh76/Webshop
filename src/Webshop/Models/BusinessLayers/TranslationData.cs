using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop.Models
{
    public class TranslationData
    {
        public int _count { get; set; }
        public IEnumerable<ArticleTranslation> _names { get; set; }

        private readonly WebShopRepository _context;

        public TranslationData(WebShopRepository context)
        {
            _context = context;
        }

        public Task<int> CountTranslation()
        {
            return _context
                .ArticleTranslations
                .Where(x => x.ISTranslated != true)
                .Select(x => x.ArticleId)
                .CountAsync();
        }

        public Task<List<ArticleTranslation>> GetNonTranslated()
        { 
            return _context
                .ArticleTranslations
                .Where(x => x.ISTranslated != true)
                .ToListAsync();
        }
    }
}
//public Task GetNonTranslated()
//{
//    List<ArticleTranslation> nonTransList = new List<ArticleTranslation>();
//    var items = _context
//    .ArticleTranslations
//    .Where(x => x.ISTranslated != true)
//    .ToList();
//    nonTransList.AddRange(items);
//    //return _names = nonTransList.ToList();
//}