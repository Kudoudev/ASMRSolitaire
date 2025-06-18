using System;
using UnityEngine;
using UnityEngine.UI;

public class HomeSceneController : SingletonMonoBehaviour<HomeSceneController>
{
    public Toggle tglDailyChallenge;
    public MainSceneController mainSceneController;
    public DailyChallengeController dailyChallengeController;
    public EventController eventController;

    public void OnDailyChallengeWin()
    {
        tglDailyChallenge.isOn = true;
    }

    public void OnShowDailyChallengeComplete()
    {
        // PlayerPrefs.SetInt(dailyChallengeController.SelectingDay.ToShortDateString(), 1);
        StartCoroutine(dailyChallengeController.OnShowWin());
    }

    public void OnEventChallengeWin()
    {
        eventController.OnWin();
    }

    public void OnShowEventChallengeComplete()
    {
        eventController.OnShowWin();
    }

    public void ReloadDailyEvent()
    {
        mainSceneController.ReloadDatetimeAndEvent();
    }
}