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

        private string CreateAuthorization(string data)
        {
            //base64(hex(sha256 (request_payload + shared_secret)))
            using (HashAlgorithm algorithm = SHA256.Create())
            //using (var algorithm = SHA256.Create())
            {
                var hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(data));
                var base64 = System.Convert.ToBase64String(hash);
                return base64;
            }
        }

        public string CreateOrder(string jsondata)
        {
            HttpRequestMessage message = SendKlarnaRequestMessage(jsondata);
            var response = _client.SendAsync(message).Result;
            if (response.StatusCode == HttpStatusCode.Created)
            {
                var location = response.Headers.Location.AbsoluteUri;
                // hämta ordern
                HttpRequestMessage getmessage = GetKlarnaResponseMessage(location);
                var getresponse = _client.SendAsync(getmessage).Result;
                var guisnippet = JsonConvert.DeserializeObject<KlarnaGetCartResponse>(getresponse.Content.ReadAsStringAsync().Result).gui.snippet;
                var obj = JsonConvert.DeserializeObject<KlarnaGetCartResponse>(getresponse.Content.ReadAsStringAsync().Result);
                return guisnippet;
            }
            return response.StatusCode.ToString();
        }

        private HttpRequestMessage SendKlarnaRequestMessage(string jsondata)
        {
            HttpRequestMessage message = new HttpRequestMessage();
            message.RequestUri = new Uri("https://checkout.testdrive.klarna.com/checkout/orders");
            message.Method = HttpMethod.Post;
            message.Headers.Authorization = new AuthenticationHeaderValue("Klarna", CreateAuthorization(jsondata + Shared_Secret));
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.klarna.checkout.aggregated-order-v2+json"));
            message.Content = new StringContent(jsondata, Encoding.UTF8, "application/vnd.klarna.checkout.aggregated-order-v2+json");
            return message;
        }

        private HttpRequestMessage GetKlarnaResponseMessage(string location)
        {
            HttpRequestMessage getmessage = new HttpRequestMessage();
            getmessage.RequestUri = new Uri(location);
            getmessage.Method = HttpMethod.Get;
            getmessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.klarna.checkout.aggregated-order-v2+json"));
            getmessage.Headers.Authorization = new AuthenticationHeaderValue("Klarna", CreateAuthorization(Shared_Secret));
            return getmessage;
        }

        public string GetKlarnaOrderUpdate(string jsondata)
        {
            HttpRequestMessage getmessage = new HttpRequestMessage();
            getmessage.Method = HttpMethod.Get;
            getmessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.klarna.checkout.aggregated-order-v2+json"));
            getmessage.Headers.Authorization = new AuthenticationHeaderValue("Klarna", CreateAuthorization(Shared_Secret));
            // hämta ordern
            var getresponse = _client.SendAsync(getmessage).Result;

            var obj = JsonConvert.DeserializeObject<KlarnaGetCartResponse>(getresponse.Content.ReadAsStringAsync().Result);
            var idOut = obj.id;
            return idOut;
        }

        public string KlarnaConfirmation(string id)
        {
            HttpRequestMessage getmessage = new HttpRequestMessage();
            getmessage.Method = HttpMethod.Get;
            getmessage.RequestUri = new Uri("https://checkout.testdrive.klarna.com/checkout/orders/" +id );
            getmessage.Headers.Authorization = new AuthenticationHeaderValue("Klarna", CreateAuthorization(Shared_Secret));
            getmessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.klarna.checkout.aggregated-order-v2+json"));
            // hämta ordern
            var getresponse = _client.SendAsync(getmessage).Result;

            var obj = JsonConvert.DeserializeObject<KlarnaGetCartResponse>(getresponse.Content.ReadAsStringAsync().Result);
            var snippOut = obj.gui.snippet;
            return snippOut;
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
    }
}