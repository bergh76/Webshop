using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop.Models
{
    public class Language
    {
        public int ID { get; set; }

        [Display(Name = "LangCode", ResourceType = typeof(Resources.Articles))]
        [DataType(DataType.Text)]
        [StringLength(5), RegularExpression("^[a-z]{{2}}(-[A-Z]{{2}})*$")]
        public string LangCode { get; set; }
        public string Country { get; set; }
    }
}
