using System;

namespace Webshop.Models
{
    public class Cart: IShopCart
    {

        int IShopCart.ArtNumber
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        int IShopCart.Amount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        decimal IShopCart.Price
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        string IShopCart.ArtName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        decimal IShopCart.ReturnTotalAmount()
        {
            throw new NotImplementedException();
        }

        string IShopCart.GetUserDetails()
        {
            throw new NotImplementedException();
        }
    }
}
