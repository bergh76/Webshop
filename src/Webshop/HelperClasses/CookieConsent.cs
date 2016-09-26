//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Webshop.Models;

//namespace Webshop.HelperClasses
//{
//    public class CookieConsent
//    {
//        public static void SetCookieConsent(HttpResponse response, bool consent)
//        {
//            var consentCookie = new HttpCookie(CookieConsentAttribute.CONSENT_COOKIE_NAME);
//            consentCookie.Value = consent ? "true" : "false";
//            consentCookie.Expires = DateTime.UtcNow.AddYears(1);
//            response.Cookies.Append(consentCookie);
//        }

//        public static bool AskCookieConsent(ViewContext context)
//        {
//            return context.ViewBag.AskCookieConsent ?? false;
//        }

//        public static bool HasCookieConsent(ViewContext context)
//        {
//            return context.ViewBag.HasCookieConsent ?? false;
//        }
//    }
//}
