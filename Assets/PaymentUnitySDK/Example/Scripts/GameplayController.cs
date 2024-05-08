using TMPro;
using UnityEngine;

namespace Scenes.PaymentUnitySDK.Example.Scripts
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField]
        private GoldProduct goldProduct;
        private BaseProduct m_GoldProductInstance;
        
        private void Awake()
        {
            PaymentEvents.OnPurchaseSuccess += PaymentEventsOnOnPurchaseSuccess;
        }
        
        private void OnDestroy()
        {
            PaymentEvents.OnPurchaseSuccess -= PaymentEventsOnOnPurchaseSuccess;
        }

        private void PaymentEventsOnOnPurchaseSuccess(string message)
        {
            Debug.Log($"Purchase success: {message}");
      
            m_GoldProductInstance.CloseProduct();
            m_GoldProductInstance = null;
        }
        
        public void ShowGoldPack1()
        {
            var productBundle = new ProductBundle(goldProduct, "BuyGoldPack1", 5.99m);
            m_GoldProductInstance = PaymentSDK.Instance.ShowProductBundle(productBundle);
        }
        
        public void ShowGoldPack2()
        {
            var productBundle = new ProductBundle(goldProduct, "BuyGoldPack2", 7.99m);
            m_GoldProductInstance = PaymentSDK.Instance.ShowProductBundle(productBundle);
        }
        
        public void ShowGoldPack3()
        {
            var productBundle = new ProductBundle(goldProduct, "BuyGoldPack3", 9.99m);
            m_GoldProductInstance = PaymentSDK.Instance.ShowProductBundle(productBundle);
        }
        
        public void ShowGoldPack4()
        {
            var productBundle = new ProductBundle(goldProduct, "BuyGoldPack4", 15.99m);
            m_GoldProductInstance = PaymentSDK.Instance.ShowProductBundle(productBundle);
        }
    }
}