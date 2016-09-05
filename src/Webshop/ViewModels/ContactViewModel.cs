using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Webshop.ViewModels
{
    public class ContactViewModel
    {
        public string CurrentDateAndTime { get; set; }
        public int id { get; set; }
        public IEnumerable<string> Names { get; set; }
    }
}