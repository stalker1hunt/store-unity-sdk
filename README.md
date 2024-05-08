Here's a formatted version for your GitHub README:

# store-unity-sdk

The repository contains two projects: a Unity project and an Android project.

### Android Project
A native Java library is developed that integrates seamlessly with Unity. The main features include:

1. **WebView Support:** Incorporates WebView for in-app browsing.
2. **JavaScript Interface:** Allows interaction between JavaScript and WebView.
3. **Google Play Support:** Compatible with Google Play services.
4. **Payment Provider Integration:** Enables integration with various payment providers.
5. **Mock Data:** Provides mock data if Google Pay isn't configured.

The primary library class can be found here:
[PaymentActivity.java](https://github.com/stalker1hunt/store-unity-sdk/blob/main/AndroidStudio/paymentsdk/src/main/java/com/example/paymentsdk/PaymentActivity.java)

### Unity Project
The Unity project folder structure:

1. **Editor:** Contains post-processors that import dependencies necessary for the library.
2. **Example:** Demonstrates how to use the SDK.
3. **Plugins:** Houses the native Java library.
4. **Prefabs:** Contains product prefabs shown before purchase.
5. **Resources:** Holds a crucial prefab that initializes and activates the SDK automatically.
6. **Source:** Contains the SDK source code.

**Key Features of the Source Folder:**

- **Initialization:** The Command pattern enables asynchronous initialization for simplicity. One line is sufficient:
  
  ```csharp
  await Initialization.PaymentSDKInitializationController.InitializeAsync();
  ```
  
  The SDK will then be ready for use.

- **PaymentEvents:** An Observer class that allows subscription and callback registration from any part of the code for smooth SDK integration.

- **PaymentPopupController:** Manages informational popups for products being purchased.

- **BaseProduct:** Developers can extend `BaseProduct` while keeping the base API closed, following SOLID principles.

- **PaymentSDK:** The main SDK class, providing the following:
  1. `ShowProductBundle` - Displays the product.
  2. `StartPurchase` - Invokes native library purchase methods.
  3. Flask processing from the native library.

### Dependencies
To use the SDK, you must install the [Google Play Games Plugin for Unity](https://github.com/playgameservices/play-games-plugin-for-unity), as it supports Google Pay. 

