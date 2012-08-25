namespace GasyTek.Lakana.Mvvm.Tests.Fakes
{
    /// <summary>
    /// An obhect used as Model for the view model.
    /// </summary>
    class Product
    {
        public string Code { get; set; }
        public int Quantity { get; set; }
        public int PurchasingPrice { get; set; }
        public int SellingPrice { get; set; }
        public string SellerEmail { get; set; }
    }
}
