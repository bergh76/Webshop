using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webshop.Interfaces;

namespace Webshop.Services
{
    public class StaticServerDateTime : IDateTime
    {
        public DateTime Now {
            get { return DateTime.Now; }
        }
    }
}
