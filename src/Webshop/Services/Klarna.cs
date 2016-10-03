using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Webshop.Models;
using Webshop.ViewModels;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Net;

namespace Webshop.Services
{
    public class Klarna
    {
        public static HttpClient _client = new HttpClient();
        public readonly string Shared_Secret = "tE94QeKzSdUVJGe";

        //public void CreateAutorization()
        //{
        //    string nubie = CreateAutho();
        //}

        private string CreateAuthorization(string data)
        {
            //base64(hex(sha256 (request_payload + shared_secret)))
            using (var algorithm = SHA256.Create())
            {
                var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(data));
                var base64 = System.Convert.ToBase64String(hash);
                return base64;
            }
        }

        public void CreateOrder(string jsondata)
        {
            HttpRequestMessage message = new HttpRequestMessage();
            message.RequestUri = new Uri("https://checkout.testdrive.klarna.com/checkout/orders");
            message.Method = HttpMethod.Post;
            message.Headers.Authorization = new AuthenticationHeaderValue("Klarna", CreateAuthorization(jsondata + Shared_Secret));
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.klarna.checkout.aggregated-order-v2+json"));
            message.Content = new StringContent(jsondata, Encoding.UTF8, "application/vnd.klarna.checkout.aggregated-order-v2+json");

            var response = _client.SendAsync(message).Result;
            if (response.StatusCode == HttpStatusCode.Created)
            {
                var location = response.Headers.Location.AbsoluteUri;
                // hämta ordern
                HttpRequestMessage getmessage = new HttpRequestMessage();
                getmessage.RequestUri = new Uri(location);
                getmessage.Method = HttpMethod.Get;
                getmessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.klarna.checkout.aggregated-order-v2+json"));
                getmessage.Headers.Authorization = new AuthenticationHeaderValue("Klarna", CreateAuthorization(Shared_Secret));
            }
            var outN = response.StatusCode;
        }

        //public void CreateOrder(string jsondata)
        //{
        //    HttpRequestMessage message = new HttpRequestMessage();
        //    var response = _client.SendAsync(message).Result;
        //    if (response.StatusCode == HttpStatusCode.Created)
        //    {
        //        message.RequestUri = new Uri("https://checkout.testdrive.klarna.com/checkout/orders");
        //        message.Method = HttpMethod.Post;
        //        message.Headers.Authorization = new AuthenticationHeaderValue("Klarna", CreateAuthorization(jsondata + Shared_Secret));
        //        message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.klarna.checkout.aggregated-order-v2+json"));
        //        message.Content = new StringContent(jsondata, Encoding.UTF8, "application/vnd.klarna.checkout.aggregated-order-v2+json");
        //        response = _client.SendAsync(message).Result;
        //    }
        //    //var response = _client.SendAsync(message).Result;
        //    if (response.StatusCode == HttpStatusCode.Created)
        //    {
        //        //var location = response.Headers.Location.AbsoluteUri;
        //        // hämta ordern
        //        string location = "https://checkout.testdrive.klarna.com/checkout/orders/";
        //        HttpRequestMessage getmessage = CreateGetRequestMessage(location);
        //        var getresponse = _client.SendAsync(getmessage).Result;
        //        // var getresponsebody = getresponse.Content.ReadAsString().Result;
        //        //HttpRequestMessage getmessage = new HttpRequestMessage();
        //        getmessage.RequestUri = new Uri(location);
        //        getmessage.Method = HttpMethod.Get;
        //        getmessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.klarna.checkout.aggregated-order-v2+json"));
        //        getmessage.Headers.Authorization = new AuthenticationHeaderValue("Klarna", CreateAuthorization(Shared_Secret));

        //        var guisnippet = JsonConvert.DeserializeObject<KlarnaGetCartResponse>(response).gui.snippet;
        //        return guisnippet;
        //    }
        //    //return response.StatusCode;
        //}

        private HttpRequestMessage CreateGetRequestMessage(string location)
        {
            HttpRequestMessage getmessage = new HttpRequestMessage();
            getmessage.RequestUri = new Uri(location);
            getmessage.Method = HttpMethod.Get;
            getmessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.klarna.checkout.aggregated-order-v2+json"));
            getmessage.Headers.Authorization = new AuthenticationHeaderValue("Klarna", CreateAuthorization(Shared_Secret));
            return getmessage;
        }


        public class Gui
        {
            public string layout { get; set; }
            public string snippet { get; set; }
        }
        public class KlarnaGetCartResponse
        {
            public string id { get; set; }
            public Gui gui { get; set; }
        }

        //private static string KlarnaConnect()
        //{
        //    var result = _client.GetAsync("https://checkout.testdrive.klarna.com/checkout/orders").Result;
        //    var obj = JsonConvert.DeserializeObject<>(result.Content.ReadAsStringAsync().Result);
        //    return obj.rates.SEK;


        //    //var connector = Connector.Create("sharedSecret", Connector.TestBaseUri);
        //    //Order order = new Order(connector);
        //    //order.Create(data);
        //}
    }
}