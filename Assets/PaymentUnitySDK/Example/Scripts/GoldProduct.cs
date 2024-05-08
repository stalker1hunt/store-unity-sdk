using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes.PaymentUnitySDK.Example.Scripts
{
    public class GoldProduct : BaseProduct
    {
        [SerializeField]
        private TMP_Text productNameText;
        [SerializeField]
        private TMP_Text productPriceText;
        
        [SerializeField]
        private Button buyButton;

        [SerializeField] 
        private Button closeButton;

        private void Awake()
        {
            buyButton.onClick.AddListener(BuyProduct);
            closeButton.onClick.AddListener(CloseProduct);
        }

        private void OnDestroy()
        {
            buyButton.onClick.RemoveListener(BuyProduct);
            closeButton.onClick.RemoveListener(CloseProduct);
        }

        public override void Initialize(string productName, decimal cost)
        {
            base.Initialize(productName, cost);
            productNameText.text = productName;
            productPriceText.text = "price: " + cost.ToString(CultureInfo.InvariantCulture);
        }
    }
}