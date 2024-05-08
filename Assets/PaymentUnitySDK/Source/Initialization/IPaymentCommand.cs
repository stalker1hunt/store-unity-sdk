namespace Scenes.PaymentUnitySDK.Initialization
{
    public interface IPaymentCommand
    {
        System.Threading.Tasks.Task ExecuteAsync();
    }
}