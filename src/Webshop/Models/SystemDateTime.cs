using System;
namespace Webshop.Interfaces
{
    public class SystemDateTime : IDateTime
    {
        // AddSingelton skapar endast ett object och håller det vid liv fram tills det avslutas av användaren
        //private DateTime _datetime;
        //public SystemDateTime()
        //{
        //    _datetime = DateTime.Now;
        //}
        //public DateTime Now {
        //    get {
        //        return _datetime;
        //    }
        //}

        // AddTransient
        public DateTime Now {
            get {
                return DateTime.Now;
            }
        }
    }
}
