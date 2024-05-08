using UnityEngine;

namespace Scenes.PaymentUnitySDK
{
    public class PaymentPopupController : MonoBehaviour
    {
        [SerializeField]
        private GameObject popupRoot;
        
        public BaseProduct ShowProductBundle(ProductBundle productBundle)
        {
            if (productBundle != null && productBundle.ProductPrefab != null)
            {
                var productInstance = Instantiate(productBundle.ProductPrefab, popupRoot.transform);
                if (productInstance != null)
                {
                    productInstance.Initialize(productBundle.ProductName, productBundle.Price);
                    productInstance.DisplayInfo();
                }

                return productInstance;
            }

            return null;
        }
    }
}