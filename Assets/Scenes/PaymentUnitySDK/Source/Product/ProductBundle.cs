
namespace Scenes.PaymentUnitySDK
{
    public class ProductBundle
    {
        public readonly BaseProduct ProductPrefab;
        public readonly string ProductName;
        public readonly decimal Price;

        public ProductBundle(BaseProduct productPrefab, string productName, decimal price)
        {
            ProductPrefab = productPrefab;
            ProductName = productName;
            Price = price;
        }
    }
}