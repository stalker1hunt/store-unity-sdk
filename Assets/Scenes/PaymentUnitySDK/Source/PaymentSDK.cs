using System.Globalization;
using UnityEngine;

namespace Scenes.PaymentUnitySDK
{
    public class PaymentSDK : MonoBehaviour
    {
        public static PaymentSDK Instance { get; private set; }

        [SerializeField] private PaymentPopupController paymentPopupController;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public BaseProduct ShowProductBundle(ProductBundle productBundle)
        {
            return paymentPopupController.ShowProductBundle(productBundle);
        }

        public void StartPurchase(PaymentProduct paymentProduct)
        {
#if UNITY_EDITOR
            OnPurchaseSuccess("Purchase successful");
            return; 
#endif
            
            if (Application.platform == RuntimePlatform.Android)
            {
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                    AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", activity,
                        new AndroidJavaClass("com.example.paymentsdk.PaymentActivity"));

                    intent.Call<AndroidJavaObject>("putExtra", "productName", paymentProduct.Name);
                    intent.Call<AndroidJavaObject>("putExtra", "price",
                        paymentProduct.Price.ToString(CultureInfo.InvariantCulture));

                    activity.Call("startActivity", intent);
                }
            }
            else
            {
                OnPurchaseFailed("Platform not supported");
                Debug.LogWarning("StartPurchase is only implemented for Android platform.");
            }
        }

        public void OnPurchaseSuccess(string message)
        {
            PaymentEvents.RaisePurchaseSuccess(message);
        }

        public void OnPurchaseFailed(string message)
        {
            PaymentEvents.RaisePurchaseFailed(message);
        }
    }
}