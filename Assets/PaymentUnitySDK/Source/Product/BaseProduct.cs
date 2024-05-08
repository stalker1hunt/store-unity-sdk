using UnityEngine;

namespace Scenes.PaymentUnitySDK
{
    public class BaseProduct : MonoBehaviour
    {
        private string m_ProductName;
        private decimal m_Price;

        public virtual void Initialize(string productName, decimal cost)
        {
            m_ProductName = productName;
            m_Price = cost;
        }

        public virtual void DisplayInfo()
        {
            Debug.Log($"Product: {m_ProductName}, Price: {m_Price}");
        }
        
        public virtual void BuyProduct()
        {
            Debug.Log($"Buying product: {m_ProductName}");
            PaymentSDK.Instance.StartPurchase(new PaymentProduct(m_ProductName, m_Price));
        }
        
        public virtual void CloseProduct()
        {
            Destroy(gameObject);
        }
    }
}