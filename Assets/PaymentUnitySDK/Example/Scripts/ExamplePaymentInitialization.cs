using UnityEngine;

namespace Scenes.PaymentUnitySDK.Example.Scripts
{
    public class ExamplePaymentInitialization : MonoBehaviour
    {
        private async void Start()
        {
            await Initialization.PaymentSDKInitializationController.InitializeAsync();
        }
    }
}