using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using System;
using System.Collections.Generic;
using UnityEngine.Purchasing.Security;
using System.Collections;
using System.Linq;
using System.Reflection;

public static class Data

{
    public static bool music
    {
        get => PlayerPrefs.GetInt("music", 1) == 1;
        set => PlayerPrefs.SetInt("music", value ? 1 : 0);
    }

    public static bool sfx
    {
        get => PlayerPrefs.GetInt("sfx", 1) == 1;
        set => PlayerPrefs.SetInt("sfx", value ? 1 : 0);
    }

    public static bool haptic
    {
        get => PlayerPrefs.GetInt("haptic", 1) == 1;
        set => PlayerPrefs.SetInt("haptic", value ? 1 : 0);
    }

    public static bool comfort
    {
        get => PlayerPrefs.GetInt("comfort", 1) == 1;
        set => PlayerPrefs.SetInt("comfort", value ? 1 : 0);
    }

    public static int hint
    {
        get => PlayerPrefs.GetInt("hint", 3);
        set => PlayerPrefs.SetInt("hint", value);
    }

    public static int shuffle
    {
        get => PlayerPrefs.GetInt("shuffle", 3);
        set => PlayerPrefs.SetInt("shuffle", value);
    }

    public static bool purchased
    {
        get
        {
            return PlayerPrefs.GetInt("Purchased", 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("Purchased", value ? 1 : 0);
        }
    }
    public static string packId
    {
        get
        {
            return PlayerPrefs.GetString("PackId", string.Empty);
        }
        set
        {
            PlayerPrefs.SetString("PackId", value);
        }
    }
    public static long purchaseTimestamp
    {
        get
        {
            return (long)PlayerPrefs.GetFloat("PurchaseTimestamp", 0);
        }
        set
        {
            PlayerPrefs.SetFloat("PurchaseTimestamp", (long)value);
        }
    }
    public static DateTime localExpiryDate;

    // New method to check if subscription is actually active
    public static bool IsActiveSubscription()
    {
        return purchased && SubscriptionManager.I != null && SubscriptionManager.I.HasActiveSubscription();
    }
}


[System.Serializable]
public class SubscriptionInfo
{
    public string productId;
    public long purchaseTimestamp;
    public DateTime expiryDate;
    public bool isActive;
    public bool isFromRestore;
    public string originalTransactionId;
}

public enum SubscriptionStatus
{
    None,           // No subscription
    Active,         // Currently active
    Expired,        // Had subscription but expired
    Cancelled       // Cancelled but still active until expiry
}

public class SubscriptionManager : MonoBehaviour, IStoreListener
{
    private static IStoreController storeController;
    private static IExtensionProvider storeExtensionProvider;

    // Product IDs
    public const string weeklyId = "com.cloudsoftware.asmr.mahjong.weekly";
    public const string monthlyId = "com.cloudsoftware.asmr.mahjong.monthly";
    public const string annualId = "com.cloudsoftware.asmr.mahjong.annual";

    string currentSelectedPack;
    public static SubscriptionManager I;
    public bool purchased { get; set; }
    public DateTime? expiredDate { get; set; }
    public SubscriptionStatus currentStatus { get; private set; } = SubscriptionStatus.None;

    public const int WEEKLY_REWARD = 5, MONTHLY_REWARD = 15, ANNUAL_REWARD = 50;

    // Local subscription storage
    private List<SubscriptionInfo> localSubscriptions = new List<SubscriptionInfo>();
    private const string SUBSCRIPTION_KEY = "LocalSubscriptions";
    private const string LAST_RESTORE_CHECK = "LastRestoreCheck";
    private const string LAST_STATUS_CHECK = "LastStatusCheck";

    void Awake()
    {
        if (I == null)
        {
            I = this;
            DontDestroyOnLoad(gameObject);
            LoadLocalSubscriptions();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (storeController == null)
        {
            InitializePurchasing();
        }

        // Check subscription status periodically
        InvokeRepeating(nameof(PeriodicStatusCheck), 0f, 30f); // Check every 30 seconds
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus) // App resumed
        {
            CheckSubscriptionStatus();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus) // App gained focus
        {
            CheckSubscriptionStatus();
        }
    }

    #region Enhanced Subscription Management

    void PeriodicStatusCheck()
    {
        var lastCheck = PlayerPrefs.GetString(LAST_STATUS_CHECK, "");
        var currentTime = DateTime.UtcNow.ToString("yyyy-MM-dd-HH");

        // Check status every hour
        if (lastCheck != currentTime)
        {
            CheckSubscriptionStatus();
            PlayerPrefs.SetString(LAST_STATUS_CHECK, currentTime);
            PlayerPrefs.Save();
        }
    }

    public void CheckSubscriptionStatus()
    {
        CleanExpiredSubscriptions();
        UpdateSubscriptionStatus();

        // Notify UI about status change
        if (NoAdsPopUp.I != null)
        {
            NoAdsPopUp.I.UpdateUI();
        }

        Debug.Log($"[Status Check] Current status: {currentStatus}, Active: {purchased}, Expires: {expiredDate}");
    }

    void SaveLocalSubscriptions()
    {
        try
        {
            string json = JsonUtility.ToJson(new SerializableList<SubscriptionInfo> { items = localSubscriptions });
            PlayerPrefs.SetString(SUBSCRIPTION_KEY, json);
            PlayerPrefs.Save();
            Debug.Log("[Local] Subscriptions saved successfully");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Local] Failed to save subscriptions: {ex.Message}");
        }
    }

    void LoadLocalSubscriptions()
    {
        try
        {
            if (PlayerPrefs.HasKey(SUBSCRIPTION_KEY))
            {
                string json = PlayerPrefs.GetString(SUBSCRIPTION_KEY);
                var wrapper = JsonUtility.FromJson<SerializableList<SubscriptionInfo>>(json);
                localSubscriptions = wrapper.items ?? new List<SubscriptionInfo>();
                Debug.Log($"[Local] Loaded {localSubscriptions.Count} subscriptions");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Local] Failed to load subscriptions: {ex.Message}");
            localSubscriptions = new List<SubscriptionInfo>();
        }
    }

    void AddOrUpdateSubscription(string productId, DateTime purchaseTime, bool isFromRestore = false, string transactionId = null)
    {
        // Don't add duplicate transactions
        if (!string.IsNullOrEmpty(transactionId))
        {
            var existing = localSubscriptions.FirstOrDefault(s => s.originalTransactionId == transactionId);
            if (existing != null)
            {
                Debug.Log($"[Local] Subscription already exists for transaction: {transactionId}");
                return;
            }
        }

        // For new purchases, remove existing subscriptions of the same product
        if (!isFromRestore)
        {
            localSubscriptions.RemoveAll(s => s.productId == productId);
        }

        DateTime expiryDate = CalculateExpiryDate(productId, purchaseTime);

        var newSubscription = new SubscriptionInfo
        {
            productId = productId,
            purchaseTimestamp = ((DateTimeOffset)purchaseTime).ToUnixTimeSeconds(),
            expiryDate = expiryDate,
            isActive = true,
            isFromRestore = isFromRestore,
            originalTransactionId = transactionId ?? Guid.NewGuid().ToString()
        };

        localSubscriptions.Add(newSubscription);
        SaveLocalSubscriptions();

        string source = isFromRestore ? "restored" : "new purchase";
        Debug.Log($"[Local] Added {source} subscription: {productId}, expires: {expiryDate}");
    }

    DateTime CalculateExpiryDate(string productId, DateTime purchaseTime)
    {
        switch (productId)
        {
            case weeklyId:
                return purchaseTime.AddDays(7);
            case monthlyId:
                return purchaseTime.AddMonths(1);
            case annualId:
                return purchaseTime.AddYears(1);
            default:
                Debug.LogWarning($"Unknown product ID: {productId}, defaulting to weekly");
                return purchaseTime.AddDays(7);
        }
    }

    public bool HasActiveSubscription()
    {
        CleanExpiredSubscriptions();
        return localSubscriptions.Any(s => s.isActive && s.expiryDate > DateTime.UtcNow);
    }

    public DateTime? GetActiveSubscriptionExpiry()
    {
        CleanExpiredSubscriptions();
        var activeSubscription = localSubscriptions
            .Where(s => s.isActive && s.expiryDate > DateTime.UtcNow)
            .OrderByDescending(s => s.expiryDate)
            .FirstOrDefault();

        return activeSubscription?.expiryDate;
    }

    public string GetActiveSubscriptionType()
    {
        CleanExpiredSubscriptions();
        var activeSubscription = localSubscriptions
            .Where(s => s.isActive && s.expiryDate > DateTime.UtcNow)
            .OrderByDescending(s => s.expiryDate)
            .FirstOrDefault();

        return activeSubscription?.productId;
    }

    public SubscriptionStatus GetSubscriptionStatus()
    {
        CleanExpiredSubscriptions();

        var hasActiveSubscription = HasActiveSubscription();
        var hasAnySubscription = localSubscriptions.Any();

        if (hasActiveSubscription)
        {
            return SubscriptionStatus.Active;
        }
        else if (hasAnySubscription)
        {
            // Had subscription but expired
            return SubscriptionStatus.Expired;
        }
        else
        {
            return SubscriptionStatus.None;
        }
    }

    void CleanExpiredSubscriptions()
    {
        bool hasChanges = false;
        var currentTime = DateTime.UtcNow;

        foreach (var sub in localSubscriptions.ToList())
        {
            if (sub.isActive && sub.expiryDate <= currentTime)
            {
                sub.isActive = false;
                hasChanges = true;
                Debug.Log($"[Local] Subscription expired: {sub.productId} (expired on {sub.expiryDate})");

                // Clear Data when subscription expires
                if (Data.packId == sub.productId)
                {
                    Data.purchased = false;
                    Debug.Log($"[Local] Cleared Data for expired subscription: {sub.productId}");
                }
            }
        }

        if (hasChanges)
        {
            SaveLocalSubscriptions();
        }
    }

    // Method to manually clear expired subscriptions (for rebuy scenarios)
    public void ClearExpiredSubscriptions()
    {
        Debug.Log("[Manual] Clearing expired subscriptions for rebuy");

        // Remove all expired subscriptions
        localSubscriptions.RemoveAll(s => !s.isActive || s.expiryDate <= DateTime.UtcNow);

        // Clear Data
        Data.purchased = false;
        Data.packId = string.Empty;

        SaveLocalSubscriptions();
        UpdateSubscriptionStatus();

        Debug.Log("[Manual] Expired subscriptions cleared, ready for rebuy");
    }

    void HandleRestoredPurchases()
    {
        if (storeController?.products == null) return;

        var lastRestoreCheck = PlayerPrefs.GetString(LAST_RESTORE_CHECK, "");
        var currentCheck = DateTime.UtcNow.ToString("yyyy-MM-dd");

        // Only process restored purchases once per day to avoid duplicates
        if (lastRestoreCheck == currentCheck)
        {
            Debug.Log("[Restore] Already processed restored purchases today");
            return;
        }

        foreach (var product in storeController.products.all)
        {
            if (product.hasReceipt && IsSubscriptionProduct(product.definition.id))
            {
                ProcessRestoredPurchase(product);
            }
        }

        PlayerPrefs.SetString(LAST_RESTORE_CHECK, currentCheck);
        PlayerPrefs.Save();
    }

    void ProcessRestoredPurchase(Product product)
    {
        try
        {
            var receiptInfo = ExtractReceiptInfo(product);
            if (receiptInfo.isValid)
            {
                DateTime purchaseTime = receiptInfo.purchaseTime ?? DateTime.UtcNow.AddDays(-30);

                AddOrUpdateSubscription(
                    product.definition.id,
                    purchaseTime,
                    isFromRestore: true,
                    transactionId: receiptInfo.transactionId
                );

                Debug.Log($"[Restore] Processed: {product.definition.id}, purchased: {purchaseTime}");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Restore] Failed to process {product.definition.id}: {ex.Message}");
        }
    }

    bool IsSubscriptionProduct(string productId)
    {
        return productId == weeklyId || productId == monthlyId || productId == annualId;
    }

    #endregion

    #region Receipt Processing

    struct ReceiptInfo
    {
        public bool isValid;
        public DateTime? purchaseTime;
        public string transactionId;
        public DateTime? expiryTime;
    }

    ReceiptInfo ExtractReceiptInfo(Product product)
    {
        var info = new ReceiptInfo { isValid = false };

        try
        {
            if (!IU.ValidateReceipt(product))
            {
                return info;
            }

            var validator = new CrossPlatformValidator(
                GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);

            var result = validator.Validate(product.receipt);

            foreach (IPurchaseReceipt receipt in result)
            {
                if (receipt.productID != product.definition.id) continue;

                if (receipt is AppleInAppPurchaseReceipt appleReceipt)
                {
                    info.isValid = true;

                    try
                    {
                        info.purchaseTime = appleReceipt.purchaseDate;
                    }
                    catch
                    {
                        Debug.LogWarning("[Apple] Could not get purchase date");
                    }

                    try
                    {
                        var receiptType = appleReceipt.GetType();
                        var transactionIdProp = receiptType.GetProperty("transactionID") ??
                                              receiptType.GetProperty("originalTransactionID");

                        if (transactionIdProp != null)
                        {
                            info.transactionId = transactionIdProp.GetValue(appleReceipt)?.ToString();
                        }
                        else
                        {
                            info.transactionId = Guid.NewGuid().ToString();
                        }
                    }
                    catch
                    {
                        info.transactionId = Guid.NewGuid().ToString();
                    }

                    try
                    {
                        var expiryProp = appleReceipt.GetType().GetProperty("subscriptionExpirationDate");
                        if (expiryProp != null)
                        {
                            var expiryValue = expiryProp.GetValue(appleReceipt);
                            if (expiryValue is DateTime expiryDate)
                            {
                                info.expiryTime = expiryDate;
                            }
                        }
                    }
                    catch
                    {
                        Debug.LogWarning("[Apple] Could not get subscription expiry date");
                    }

                    Debug.Log($"[Apple] Purchase time: {info.purchaseTime}, Transaction: {info.transactionId}");
                    break;
                }

                if (receipt is GooglePlayReceipt googleReceipt)
                {
                    info.isValid = true;
                    info.transactionId = googleReceipt.purchaseToken ?? Guid.NewGuid().ToString();
                    Debug.Log($"[Google] Transaction: {info.transactionId}");
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Receipt] Error extracting info: {ex.Message}");
        }

        return info;
    }

    #endregion

    public void Restore()
    {
        Debug.Log("[Restore] Starting restore process");
        // loading.ShowPopUp(popup);
        HandleRestoredPurchases();
        StartCoroutine(IHide());
    }

    IEnumerator IHide()
    {
        yield return new WaitForSeconds(2f);
        // var fpu = GameObject.FindObjectsByType<PopUpsController>(FindObjectsSortMode.None)
        //     .Where(s => s.description.ToLower() == "loading").FirstOrDefault();

        // if (fpu != null)
        // {
        //     fpu.CloseWindow();
        // }
    }

    public void OpenManageSubscription()
    {
#if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/account/subscriptions");
#elif UNITY_IOS
        Application.OpenURL("https://apps.apple.com/account/subscriptions");
#else
        Debug.LogWarning("Manage subscription not supported on this platform.");
#endif
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"IAP Initialization Failed: {error} - {message}");
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"Purchase failed: {product.definition.id}, Reason: {failureReason}");
    }

    List<string> products = new List<string> { weeklyId, monthlyId, annualId };

    public void InitializePurchasing()
    {
        if (storeController != null)
        {
            Debug.Log("IAP already initialized");
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        foreach (var product in products)
        {
            builder.AddProduct(product, ProductType.Subscription);
        }
        UnityPurchasing.Initialize(this, builder);
    }

    public void BuySubscription(string pack)
    {
        Debug.Log($"[Purchase] Attempting to buy: {pack}");

        if (storeController?.products == null)
        {
            Debug.LogError("Store not initialized");
            return;
        }

        // Clear expired subscriptions before new purchase
        if (currentStatus == SubscriptionStatus.Expired)
        {
            ClearExpiredSubscriptions();
        }

        Product product = storeController.products.WithID(pack);
        if (product?.availableToPurchase == true)
        {
            Debug.Log($"Purchasing: {pack}");
            storeController.InitiatePurchase(product);
        }
        else
        {
            Debug.LogError($"Product not available: {pack}");
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        storeController = controller;
        storeExtensionProvider = extensions;
        Debug.Log("IAP Initialized successfully");

        HandleRestoredPurchases();
        UpdateSubscriptionStatus();
    }

    void UpdateSubscriptionStatus()
    {
        purchased = HasActiveSubscription();
        expiredDate = GetActiveSubscriptionExpiry();
        currentStatus = GetSubscriptionStatus();

        Debug.Log($"Subscription Status - Active: {purchased}, Expires: {expiredDate}, Status: {currentStatus}");

        if (purchased)
        {
            string activeType = GetActiveSubscriptionType();
            Debug.Log($"Active subscription type: {activeType}");
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError($"IAP Initialization Failed: {error}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        string productId = args.purchasedProduct.definition.id;
        Debug.Log($"Processing purchase: {productId}");

        if (!IU.ValidateReceipt(args.purchasedProduct))
        {
            #if !UNITY_EDITOR
            Debug.LogError("Receipt validation failed!");
            return PurchaseProcessingResult.Complete;
            #endif
        }
        try
        {
            var receiptInfo = ExtractReceiptInfo(args.purchasedProduct);
            DateTime purchaseTime = receiptInfo.purchaseTime ?? DateTime.UtcNow;

            AddOrUpdateSubscription(productId, purchaseTime, false, receiptInfo.transactionId);
            ProcessSubscriptionRewards(productId);
            UpdateSubscriptionStatus();

            Debug.Log($"Purchase completed successfully: {productId}");
            NoAdsPopUp.I?.UpdateUI();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error processing purchase: {ex.Message}");
        }

        return PurchaseProcessingResult.Complete;
    }

    void ProcessSubscriptionRewards(string productId)
    {
        int shuffleReward, hintReward;

        switch (productId)
        {
            case annualId:
                shuffleReward = hintReward = ANNUAL_REWARD;
                Debug.Log("Annual subscription purchased");
                break;
            case monthlyId:
                shuffleReward = hintReward = MONTHLY_REWARD;
                Debug.Log("Monthly subscription purchased");
                break;
            case weeklyId:
                shuffleReward = hintReward = WEEKLY_REWARD;
                Debug.Log("Weekly subscription purchased");
                break;
            default:
                Debug.LogWarning($"Unknown product: {productId}");
                return;
        }

        Data.purchased = true;
        Data.packId = productId;

        // ShuffleHolder.Add(shuffleReward);
        // HintHolder.Add(hintReward);

        Debug.Log($"Rewards added - Shuffle: {shuffleReward}, Hint: {hintReward}");
    }
}

[System.Serializable]
public class SerializableList<T>
{
    public List<T> items;
}

public static class IU
{
    public static bool ValidateReceipt(Product product)
    {
        if (!product.hasReceipt)
        {
            Debug.Log($"[IAP] No receipt for product: {product.definition.id}");
            return false;
        }

        try
        {
            var validator = new CrossPlatformValidator(
                GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);

            var result = validator.Validate(product.receipt);

            foreach (IPurchaseReceipt receipt in result)
            {
                if (receipt.productID == product.definition.id)
                {
                    Debug.Log($"[IAP] Valid receipt for: {product.definition.id}");
                    return true;
                }
            }
        }
        catch (IAPSecurityException ex)
        {
            Debug.LogError($"[IAP] Receipt validation failed: {ex.Message}");
            return false;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[IAP] Validation error: {ex.Message}");
            return false;
        }

        Debug.LogWarning($"[IAP] No valid receipt found for: {product.definition.id}");
        return false;
    }

    public static bool IsSubscriptionExpired(Product product)
    {
        return SubscriptionManager.I?.HasActiveSubscription() != true;
    }

    public static DateTime? GetSubscriptionExpiryDate(Product product)
    {
        return SubscriptionManager.I?.GetActiveSubscriptionExpiry();
    }
}