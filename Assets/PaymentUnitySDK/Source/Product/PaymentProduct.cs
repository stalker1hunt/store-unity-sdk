namespace Scenes.PaymentUnitySDK
{
    public class PaymentProduct
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public PaymentProduct(string name, decimal price) 
        {
            Name = name;
            Price = price;
        }
    }
}