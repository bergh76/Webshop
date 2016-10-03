using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Webshop.Models;

namespace Webshop.Services
{
    public class FixerIO
    {
        public static HttpClient _client = new HttpClient();

        public FixerIO() { }

        public static decimal GetUDSToRate(string ISOCurrencySymbol)
        {
            if(string.IsNullOrEmpty(ISOCurrencySymbol) || ISOCurrencySymbol.Length != 3 || ISOCurrencySymbol == "SEK")
            {
                return 1.0M;
            }
            var result = _client.GetAsync("http://api.fixer.io/latest?base="+ ISOCurrencySymbol).Result;
            var obj = JsonConvert.DeserializeObject<RootObj>(result.Content.ReadAsStringAsync().Result);
            return obj.rates.SEK;
        }
        public class Rate
        {
            public decimal USD { get; set; }
            public decimal SEK { get; set; }
            public decimal EUR { get; set; }
            public decimal GBP { get; set; }
        }

        public class RootObj
        {
            public string @base { get; set; }
            public string date { get; set; }
            public Rate rates { get; set; }
        }
    }
}