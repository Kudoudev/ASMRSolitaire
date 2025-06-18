using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPManager : MonoBehaviour
{
    public GameObject loading;

    public void OnClick()
    {
        loading.SetActive(true);
    }

    public void OnRemoveAdsPurchased()
    {
        IronSourceManager.Instance.SetRemoveInterstitialAds(1);
    }

    public void OnRestoreSuccess()
    {
        IronSourceManager.Instance.SetRemoveInterstitialAds(1);
    }
}