using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public enum PackType
{
    Weekly,
    Monthly,
    Annual
}


public class NoAdsPopUp  : MonoBehaviour
{
    [Header("Subscription Packs")]
    public NoAdsPack weekly, monthly, annual;

    [Header("UI Elements")]
    public Button subscribe;
    public Button restoreButton;
    public Button ManageSubscriptionButton;
    public GameObject unsubscribed, subscribed, restore;
    public List<GameObject> subscribedObjects;
    public Text expiredDate;
    // public Text hintReward, shuffleReward;

    // [Header("Status Messages")]
    // public GameObject expiredPanel; // Panel to show when subscription expired
    // public Text statusMessage;      // General status message
    // public Text subscriptionTypeText; // Show current subscription type
    // public Text duration;

    static public NoAdsPopUp I;

    PackType type;
    bool isInitialized = false;

    void Awake()
    {
        I = this;
    }

    void Start()
    {
        SetupUI();
        SelectDefaultPack();
        UpdateUI();
        isInitialized = true;
    }

    void SetupUI()
    {
        // Setup click listeners
        weekly.click.onClick.AddListener(SelectWeekly);
        monthly.click.onClick.AddListener(SelectMonthly);
        annual.click.onClick.AddListener(SelectAnnual);
        subscribe.onClick.AddListener(Subscribe);

        if (restoreButton != null)
        {
            restoreButton.onClick.AddListener(RestorePurchases);
        }

        ManageSubscriptionButton.onClick.AddListener(SubscriptionManager.I.OpenManageSubscription);

    }

    void SelectDefaultPack()
    {
        // Select pack based on current subscription or default to weekly
        if (SubscriptionManager.I != null && SubscriptionManager.I.purchased)
        {
            string activeType = SubscriptionManager.I.GetActiveSubscriptionType();
            switch (activeType)
            {
                case SubscriptionManager.monthlyId:
                    SelectMonthly();
                    break;
                case SubscriptionManager.annualId:
                    SelectAnnual();
                    break;
                default:
                    SelectWeekly();
                    break;
            }
        }
        else
        {
            SelectWeekly();
        }
    }

    // void OnEnable()
    // {
    //     if (isInitialized)
    //     {
    //         // Force check subscription status when popup opens
    //         if (SubscriptionManager.I != null)
    //         {
    //             SubscriptionManager.I.CheckSubscriptionStatus();
    //             UpdateUI();
    //         }
    //     }
    // }

    void SetReward(int value)
    {
        // hintReward.text = $"{value} Hints";
        // shuffleReward.text = $"{value} Shuffles";
    }

    void SelectWeekly()
    {
        DeselectAll();
        weekly.transform.SetAsLastSibling();
        weekly.Select();
        type = PackType.Weekly;
        SetReward(SubscriptionManager.WEEKLY_REWARD);
        // duration.text = "2.99$ per week";
    }

    void SelectMonthly()
    {
        DeselectAll();
        monthly.transform.SetAsLastSibling();
        monthly.Select();
        type = PackType.Monthly;
        SetReward(SubscriptionManager.MONTHLY_REWARD);
        // duration.text = "6.99$ per month";

    }

    void SelectAnnual()
    {
        DeselectAll();
        annual.transform.SetAsLastSibling();
        annual.Select();
        type = PackType.Annual;
        SetReward(SubscriptionManager.ANNUAL_REWARD);
        // duration.text = "29.99$ per year";

    }

    void DeselectAll()
    {
        weekly.DeSelect();
        monthly.DeSelect();
        annual.DeSelect();
    }

    void Subscribe()
    {
        if (SubscriptionManager.I == null)
        {
            Debug.LogError("SubscriptionManager not available");
            ShowStatusMessage("Subscription service not available. Please try again later.");
            return;
        }

        string pack = GetSelectedPack();
        Debug.Log($"[NoAdsPopUp] Attempting to subscribe to: {pack}");

        SubscriptionManager.I.BuySubscription(pack);
    }

    void RestorePurchases()
    {
        StartCoroutine(IRestore());

        if (SubscriptionManager.I == null)
        {
            Debug.LogError("SubscriptionManager not available");
            ShowStatusMessage("Restore service not available. Please try again later.");
            return;
        }

        Debug.Log("[NoAdsPopUp] Restoring purchases");
        SubscriptionManager.I.Restore();
    }

    IEnumerator IRestore()
    {
        restore.SetActive(true);
        yield return new WaitForSeconds(2f);
        restore.SetActive(false);
    }


    string GetSelectedPack()
    {
        switch (type)
        {
            case PackType.Monthly:
                return SubscriptionManager.monthlyId;
            case PackType.Annual:
                return SubscriptionManager.annualId;
            default:
                return SubscriptionManager.weeklyId;
        }
    }

    public void UpdateUI()
    {
        if (SubscriptionManager.I == null)
        {
            ShowUnsubscribedState("Subscription service not available");
            return;
        }

        var status = SubscriptionManager.I.currentStatus;
        var isActive = SubscriptionManager.I.HasActiveSubscription();
        var expiryDate = SubscriptionManager.I.GetActiveSubscriptionExpiry();

        Debug.Log($"[NoAdsPopUp] UpdateUI - Status: {status}, Active: {isActive}, Expiry: {expiryDate}");

        switch (status)
        {
            case SubscriptionStatus.Active:
                ShowSubscribedState(expiryDate);
                break;

            case SubscriptionStatus.Expired:
                ShowExpiredState();
                break;

            case SubscriptionStatus.Cancelled:
                ShowCancelledState(expiryDate);
                break;

            default:
                ShowUnsubscribedState();
                break;
        }

        // Update Data to match current state
        Data.purchased = isActive;
        if (isActive)
        {
            Data.packId = SubscriptionManager.I.GetActiveSubscriptionType();
        }
        else
        {
            Data.packId = string.Empty;
        }
    }

    void ShowSubscribedState(DateTime? expiryDate)
    {
        unsubscribed.SetActive(false);
        subscribed.SetActive(true);
        subscribedObjects.ForEach(s => s.gameObject.SetActive(true));

        // if (expiredPanel != null)
        //     expiredPanel.SetActive(false);

        if (expiryDate.HasValue)
        {
            string renewalText = $"Your subscription will renew on {expiryDate.Value:MMM dd, yyyy}";
            expiredDate.text = renewalText;

            // if (subscriptionTypeText != null)
            // {
            //     string activeType = SubscriptionManager.I.GetActiveSubscriptionType();
            //     subscriptionTypeText.text = GetSubscriptionDisplayName(activeType);
            // }
        }
        else
        {
            expiredDate.text = "Active subscription";
        }

        ShowStatusMessage("Subscription Active", Color.green);

        // Update subscribe button text for active subscription
        if (subscribe != null)
        {
            var buttonText = subscribe.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "Manage Subscription";
            }
        }
    }

    void ShowExpiredState()
    {
        unsubscribed.SetActive(true);
        subscribed.SetActive(false);
        subscribedObjects.ForEach(s => s.gameObject.SetActive(false));

        // if (expiredPanel != null)
        //     expiredPanel.SetActive(true);

        expiredDate.text = "Your subscription has expired. Subscribe again to continue enjoying premium features!";
        ShowStatusMessage("Subscription Expired - Rebuy Available", Color.red);

        // Update subscribe button text for rebuy
        if (subscribe != null)
        {
            var buttonText = subscribe.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "Subscribe Again";
            }
        }
    }

    void ShowCancelledState(DateTime? expiryDate)
    {
        // Still show as subscribed since it's active until expiry
        unsubscribed.SetActive(false);
        subscribed.SetActive(true);
        subscribedObjects.ForEach(s => s.gameObject.SetActive(true));

        // if (expiredPanel != null)
            // expiredPanel.SetActive(false);

        if (expiryDate.HasValue)
        {
            string cancelText = $"Subscription cancelled. Access until {expiryDate.Value:MMM dd, yyyy}";
            expiredDate.text = cancelText;
        }
        else
        {
            expiredDate.text = "Subscription cancelled but still active";
        }

        ShowStatusMessage("Subscription Cancelled", Color.yellow);

        // Update subscribe button text for resubscribe
        if (subscribe != null)
        {
            var buttonText = subscribe.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "Resubscribe";
            }
        }
    }

    void ShowUnsubscribedState(string customMessage = null)
    {
        unsubscribed.SetActive(true);
        subscribed.SetActive(false);
        subscribedObjects.ForEach(s => s.gameObject.SetActive(false));

        // if (expiredPanel != null)
        //     expiredPanel.SetActive(false);

        expiredDate.text = customMessage ?? "No active subscription";
        ShowStatusMessage(customMessage ?? "Ready to Subscribe", Color.white);

        if (subscribe != null)
        {
            var buttonText = subscribe.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = "Subscribe";
            }
        }
    }

    void ShowStatusMessage(string message, Color? color = null)
    {
        // if (statusMessage != null)
        // {
        //     statusMessage.text = message;
        //     if (color.HasValue)
        //     {
        //         statusMessage.color = color.Value;
        //     }
        // }

        Debug.Log($"[NoAdsPopUp] Status: {message}");
    }

    string GetSubscriptionDisplayName(string productId)
    {
        switch (productId)
        {
            case SubscriptionManager.weeklyId:
                return "Weekly Premium";
            case SubscriptionManager.monthlyId:
                return "Monthly Premium";
            case SubscriptionManager.annualId:
                return "Annual Premium";
            default:
                return "Premium Subscription";
        }
    }

    // Method to handle subscription button click based on current status
    void HandleSubscriptionAction()
    {
        if (SubscriptionManager.I == null) return;

        var status = SubscriptionManager.I.currentStatus;

        switch (status)
        {
            case SubscriptionStatus.Active:
                // Open manage subscription page
                SubscriptionManager.I.OpenManageSubscription();
                break;

            case SubscriptionStatus.Expired:
                // Clear expired data and allow rebuy
                SubscriptionManager.I.ClearExpiredSubscriptions();
                Subscribe();
                break;

            case SubscriptionStatus.Cancelled:
            case SubscriptionStatus.None:
                // Normal subscription flow
                Subscribe();
                break;
        }
    }

    // Public method for external calls to refresh UI
    public void RefreshUI()
    {
        if (SubscriptionManager.I != null)
        {
            SubscriptionManager.I.CheckSubscriptionStatus();
        }
        UpdateUI();
    }

    // Method to show loading state during purchase
    public void ShowPurchaseLoading(bool show)
    {
        if (subscribe != null)
        {
            subscribe.interactable = !show;
            var buttonText = subscribe.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = show ? "Processing..." : GetButtonTextForCurrentStatus();
            }
        }
    }

    string GetButtonTextForCurrentStatus()
    {
        if (SubscriptionManager.I == null) return "Subscribe Now";

        switch (SubscriptionManager.I.currentStatus)
        {
            case SubscriptionStatus.Active:
                return "Manage Subscription";
            case SubscriptionStatus.Expired:
                return "Subscribe Again";
            case SubscriptionStatus.Cancelled:
                return "Resubscribe";
            default:
                return "Subscribe Now";
        }
    }

    public void OnWindowOpen()
    {

        // Close settings window if open
        // var settingsPopup = GameObject.FindObjectsByType<PopUpsController>(FindObjectsSortMode.None)
            // .Where(s => s.description.ToLower() == "settings").FirstOrDefault();

        // if (settingsPopup != null)
        // {
        //     settingsPopup.CloseWindow();
        // }

        // Refresh subscription status when popup opens
        RefreshUI();
    }

    // Method to handle app coming back from background (useful for checking subscription changes)
    void OnApplicationFocus(bool hasFocus)
    {
        // if (hasFocus && isInitialized && gameObject.activeInHierarchy)
        // {
        //     // Small delay to ensure system is ready
        //     Invoke(nameof(RefreshUI), 0.5f);
        // }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        // if (!pauseStatus && isInitialized && gameObject.activeInHierarchy)
        // {
        //     // App resumed, check for subscription changes
        //     Invoke(nameof(RefreshUI), 0.5f);
        // }
    }
}