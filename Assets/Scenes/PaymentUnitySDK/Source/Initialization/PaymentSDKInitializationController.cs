using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Scenes.PaymentUnitySDK.Initialization
{
    public class PaymentSDKInitializationController
    {
        private static readonly List<IPaymentCommand> InitializationCommands = new List<IPaymentCommand>();

        public static async Task InitializeAsync() 
        {
            RegisterCommand(new LoadPrefabCommand("PaymentSDKPrefab"));
            
            foreach (var command in InitializationCommands) {
                await command.ExecuteAsync();
            }

            Debug.Log("Payment SDK Initialization completed.");
        }
        
        private static void RegisterCommand(IPaymentCommand command)
        {
            InitializationCommands.Add(command);
        }

    }
}