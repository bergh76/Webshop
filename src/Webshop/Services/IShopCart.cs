namespace Webshop.Models
{
    public interface IShopCart
    {
        int ArtNumber { get; }

        int Amount { get; }

         decimal Price { get; }

        string ArtName { get; }

        decimal ReturnTotalAmount();

        string GetUserDetails();
    }
}