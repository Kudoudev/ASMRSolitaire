using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwardsController : MonoBehaviour
{
    #region Daily Challenge

    // public Toggle tglDailyChallenge;
    public GameObject yearItem;
    public Transform dailyChallangeContent;

    private void LoadDailyChallenge()
    {
        DateTime today = DateTime.Today;
        for (int i = today.Year; i >= 2024; i--)
        {
            YearItem year = Instantiate(yearItem, dailyChallangeContent).GetComponent<YearItem>();
            year.SetYearDailyChallenge(i);
            listYearDailyChallenge ??= new List<YearItem>();
            listYearDailyChallenge.Add(year);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(dailyChallangeContent.GetComponent<RectTransform>());
    }

    #endregion

    #region Daily Challenge

    public Transform eventContent;
    public GameObject scrollViewEvent;

    private void LoadEvent()
    {
        DateTime today = DateTime.Today;
        for (int i = today.Year; i >= 2024; i--)
        {
            YearItem year = Instantiate(yearItem, eventContent).GetComponent<YearItem>();
            year.SetYearEvent(i);
            listYearEvent ??= new List<YearItem>();
            listYearEvent.Add(year);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(eventContent.GetComponent<RectTransform>());
        scrollViewEvent.SetActive(false);
    }

    #endregion

    private List<YearItem> listYearDailyChallenge, listYearEvent;

    private void Start()
    {
        LoadDailyChallenge();
        LoadEvent();
    }

    private void OnEnable()
    {
        if (listYearDailyChallenge != null)
        {
            foreach (YearItem item in listYearDailyChallenge)
            {
                item.ReloadDailyChallenge();
            }
        }

        if (listYearEvent != null)
        {
            foreach (YearItem item in listYearEvent)
            {
                item.ReloadEvent();
            }
        }

        // tglDailyChallenge.isOn = true;
    }

    private void OnGUI()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(eventContent.GetComponent<RectTransform>());
    }
}