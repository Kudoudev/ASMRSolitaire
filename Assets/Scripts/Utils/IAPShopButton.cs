using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPShopButton : MonoBehaviour
{
    private void Start()
    {
        if (IronSourceManager.Instance.IsRemovedInterstitialAds())
        {
            gameObject.SetActive(false);
        }
    }
}
