package com.example.paymentsdk;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;
import android.webkit.JavascriptInterface;
import android.webkit.WebView;

import androidx.appcompat.app.AppCompatActivity;

import com.google.android.gms.common.api.Status;
import com.google.android.gms.wallet.AutoResolveHelper;
import com.google.android.gms.wallet.CardRequirements;
import com.google.android.gms.wallet.PaymentData;
import com.google.android.gms.wallet.PaymentDataRequest;
import com.google.android.gms.wallet.PaymentMethodTokenizationParameters;
import com.google.android.gms.wallet.PaymentsClient;
import com.google.android.gms.wallet.TransactionInfo;
import com.google.android.gms.wallet.Wallet;
import com.google.android.gms.wallet.WalletConstants;
import com.unity3d.player.UnityPlayer;

import java.util.Arrays;

public class PaymentActivity extends AppCompatActivity {
    private static final int LOAD_PAYMENT_DATA_REQUEST_CODE = 1;
    private PaymentsClient paymentsClient;
    private String productName;
    private String price;
    private static final String TAG = "PaymentActivity";
    private boolean isDebugMode = false;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        setTheme(R.style.AppTheme);
        super.onCreate(savedInstanceState);
        Log.d(TAG, "onCreate: Starting PaymentActivity");
        setContentView(R.layout.activity_payment);

        productName = getIntent().getStringExtra("productName");
        price = getIntent().getStringExtra("price");

        createPaymentsClient(this);

        try {
            WebView webView = findViewById(R.id.webview);
            webView.getSettings().setJavaScriptEnabled(true);

            webView.addJavascriptInterface(new WebAppInterface(), "Android");

            if (productName != null && price != null) {
                loadProductInfo(webView);
            }
        } catch (Exception e) {
            Log.e(TAG, "Error during onCreate", e);
        }
    }

    public void setProductInfo(String productName, String price) {
        Log.d(TAG, "setProductInfo: Setting product info with name: " + productName + " and price: " + price);
        this.productName = productName;
        this.price = price;

        WebView webView = findViewById(R.id.webview);
        if (webView == null) {
            Log.d(TAG, "WebView not found");
        } else {
            loadProductInfo(webView);
        }
    }

    private void loadProductInfo(WebView webView) {
        String htmlData = "<html><body>" +
                "<h1>" + productName + "</h1>" +
                "<p>Price: $" + price + "</p>" +
                "<label>" +
                "<input type='checkbox' id='useMockPayment' /> Use Mock Payment" +
                "</label><br>" +
                "<button onclick='Android.purchaseProduct()'>Buy Now</button>" +
                "<button onclick='initiatePayment()'>Pay with Google</button>" +
                "<script type='text/javascript'>" +
                "function initiatePayment() {" +
                "   var useMock = document.getElementById('useMockPayment').checked;" +
                "   Android.initiateGooglePay(useMock);" +
                "}" +
                "</script>" +
                "</body></html>";
        webView.loadData(htmlData, "text/html", "UTF-8");
        Log.d(TAG, "loadProductInfo: Loaded HTML data into WebView");
    }

    private void createPaymentsClient(Activity activity) {
        Wallet.WalletOptions walletOptions = new Wallet.WalletOptions.Builder()
                .setEnvironment(WalletConstants.ENVIRONMENT_TEST)
                .build();
        paymentsClient = Wallet.getPaymentsClient(activity, walletOptions);
        Log.d(TAG, "createPaymentsClient: PaymentsClient created");
    }

    private PaymentDataRequest createPaymentDataRequest() {
        return PaymentDataRequest.newBuilder()
                .setTransactionInfo(TransactionInfo.newBuilder()
                        .setTotalPriceStatus(WalletConstants.TOTAL_PRICE_STATUS_FINAL)
                        .setTotalPrice("10.00")
                        .setCurrencyCode("USD")
                        .build())
                .addAllowedPaymentMethod(WalletConstants.PAYMENT_METHOD_CARD)
                .addAllowedPaymentMethod(WalletConstants.PAYMENT_METHOD_TOKENIZED_CARD)
                .setCardRequirements(
                        CardRequirements.newBuilder()
                                .addAllowedCardNetworks(Arrays.asList(
                                        WalletConstants.CARD_NETWORK_VISA,
                                        WalletConstants.CARD_NETWORK_MASTERCARD))
                                .build())
                .setPaymentMethodTokenizationParameters(
                        PaymentMethodTokenizationParameters.newBuilder()
                                .setPaymentMethodTokenizationType(WalletConstants.PAYMENT_METHOD_TOKENIZATION_TYPE_PAYMENT_GATEWAY)
                                .addParameter("gateway", "example")
                                .addParameter("gatewayMerchantId", "exampleMerchantId")
                                .build())
                .build();
    }

    public void requestPayment() {
        if (isDebugMode) {
            Log.d(TAG, "requestPayment: Processing mock payment");
            paymentRegular();
        } else {
            PaymentDataRequest request = createPaymentDataRequest();
            Log.d(TAG, "requestPayment: Requesting payment via Google Pay");
            AutoResolveHelper.resolveTask(
                    paymentsClient.loadPaymentData(request),
                    this,
                    LOAD_PAYMENT_DATA_REQUEST_CODE);
        }
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        Log.d(TAG, "onActivityResult: requestCode=" + requestCode + ", resultCode=" + resultCode);
        if (requestCode == LOAD_PAYMENT_DATA_REQUEST_CODE) {
            switch (resultCode) {
                case Activity.RESULT_OK:
                    PaymentData paymentData = PaymentData.getFromIntent(data);
                    handlePaymentSuccess(paymentData);
                    break;
                case Activity.RESULT_CANCELED:
                    Log.d(TAG, "onActivityResult: Payment was canceled");
                    break;
                case AutoResolveHelper.RESULT_ERROR:
                    Status status = AutoResolveHelper.getStatusFromIntent(data);
                    handleError();
                    break;
            }
        }
    }

    private void paymentRegular() {
        Log.d(TAG, "paymentRegular: Processing regular payment");
        runOnUiThread(() -> {
            UnityPlayer.UnitySendMessage("PurchaseManager", "OnPurchaseSuccess", "Purchase was successful.");
            confirmAndClose();
        });
    }

    private void handleError() {
        Log.d(TAG, "handleError: Handling payment error");
        runOnUiThread(() -> {
            UnityPlayer.UnitySendMessage("PurchaseManager", "OnPurchaseFailed", "Purchase was failure.");
            confirmAndCloseError();
        });
    }

    private void handlePaymentSuccess(PaymentData paymentData) {
        Log.d(TAG, "handlePaymentSuccess: Payment was successful");
        paymentRegular();
    }

    private void confirmAndClose() {
        Log.d(TAG, "confirmAndClose: Displaying success dialog");
        new AlertDialog.Builder(this)
                .setTitle("Purchase Successful")
                .setMessage("Your purchase was successful.")
                .setPositiveButton("OK", (dialog, which) -> {
                    finish();
                })
                .show();
    }

    private void confirmAndCloseError() {
        Log.d(TAG, "confirmAndCloseError: Displaying error dialog");
        new AlertDialog.Builder(this)
                .setTitle("Purchase Failed")
                .setMessage("Your purchase was Failed.")
                .setPositiveButton("OK", (dialog, which) -> {
                    finish();
                })
                .show();
    }

    private class WebAppInterface {
        @JavascriptInterface
        public void purchaseProduct() {
            Log.d(TAG, "WebView: purchaseProduct called");
            runOnUiThread(PaymentActivity.this::paymentRegular);
        }

        @JavascriptInterface
        public void initiateGooglePay(boolean useMock) {
            Log.d(TAG, "WebView: initiateGooglePay called with useMock=" + useMock);
            isDebugMode = useMock;
            runOnUiThread(PaymentActivity.this::requestPayment);
        }
    }
}
