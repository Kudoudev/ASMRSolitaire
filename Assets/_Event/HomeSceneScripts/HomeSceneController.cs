using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeSceneController : SingletonMonoBehaviour<HomeSceneController>
{
    public Toggle tglDailyChallenge;
    public MainSceneController mainSceneController;
    public DailyChallengeController dailyChallengeController;
    public EventController eventController;
    public TMP_Text date;

    void Start()
    {
        if (DailyChallengeController.winEvent)
        {
            dailyChallengeController.gameObject.SetActive(true);
            OnDailyChallengeWin();
        }
        else
        {
            if (DailyChallengeController.SelectingDay == default(DateTime))
            {
                DailyChallengeController.SelectingDay = DateTime.Today;
                if (PlayerPrefs.GetInt(DailyChallengeController.SelectingDay.ToShortDateString(), 0) == 1)
                {
                    //find last day that is not finished
                    for (int i = 0; i < 365; i++)
                    {
                        DailyChallengeController.SelectingDay = DateTime.Today.AddDays(-i);
                        // Debug.LogError(SelectingDay);
                        if (PlayerPrefs.GetInt(DailyChallengeController.SelectingDay.ToShortDateString(), 0) == 0)
                            break;
                    }
                }
            }
            UpdateDate();
        }
    }
    public void UpdateDate()
    {
        date.text = DailyChallengeController.SelectingDay.ToLongDateString();
    }
    public void OnDailyChallengeWin()
    {
        tglDailyChallenge.isOn = true;
    }

    public void OnShowDailyChallengeComplete()
    {
        StartCoroutine(dailyChallengeController.OnShowWin());
    }

    public void ReloadDailyEvent()
    {
        mainSceneController.ReloadDatetimeAndEvent();
    }
}