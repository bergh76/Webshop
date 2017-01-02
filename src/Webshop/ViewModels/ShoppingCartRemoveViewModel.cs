namespace Webshop.ViewModels
{
    internal class ShoppingCartRemoveViewModel
    {
        public object CartCount { get; set; }
        public object CartTotal { get; set; }
        public int DeleteId { get; set; }
        public int ItemCount { get; set; }
        public string Message { get; set; }
    }
}