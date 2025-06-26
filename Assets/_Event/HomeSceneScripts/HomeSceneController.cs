using System;
using UnityEngine;
using UnityEngine.UI;

public class HomeSceneController : SingletonMonoBehaviour<HomeSceneController>
{
    public Toggle tglDailyChallenge;
    public MainSceneController mainSceneController;
    public DailyChallengeController dailyChallengeController;
    public EventController eventController;

    void Start()
    {
        if (DailyChallengeController.winEvent)
        {
            dailyChallengeController.gameObject.SetActive(true);
            OnDailyChallengeWin();
        }
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