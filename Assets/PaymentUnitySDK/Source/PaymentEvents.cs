using System;

namespace Scenes.PaymentUnitySDK
{
    public static class PaymentEvents
    {
        public static event Action<string> OnPurchaseSuccess;
        public static event Action<string> OnPurchaseFailed;

        public static void RaisePurchaseSuccess(string message)
        {
            OnPurchaseSuccess?.Invoke(message);
        }

        public static void RaisePurchaseFailed(string message)
        {
            OnPurchaseFailed?.Invoke(message);
        }
    }
}