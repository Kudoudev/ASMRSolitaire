using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    public TMP_Text txtBestScore;
    private void Start()
    {
        ReloadDatetimeAndEvent();
    }

    public void ReloadDatetimeAndEvent()
    {
        LoadDatetime();
        LoadEvent();
    }

    #region event

    public TMP_Text txtDateTime, txtDailyChallengeButton;
    public Toggle tglDailyChallenge;

    public DailyChallengeController dailyChallengeController;

    public void LoadDatetime()
    {
        DateTime today = DateTime.Today;
        txtDateTime.text = Contains.ConvertMonth(today.Month) + " " + today.Day;
        txtDailyChallengeButton.text = PlayerPrefs.GetInt(today.ToShortDateString(), 0) == 0 ? "Play" : "Complete";
    }

    public void OnPlayDailyChallengeClick()
    {
        if (txtDailyChallengeButton.text == "Play")
        {
            Debug.LogError("==========TODO Implement play daily challenge");
        }
        else
        {
            tglDailyChallenge.isOn = true;
        }
    }

    #endregion

    #region Event

    public TMP_Text txtEventName, txtRemainTime, txtRemainTimeInEventPopup, txtEventButton;

    private void LoadEvent()
    {
        txtEventName.text = Contains.GetCurrentEvent();

        DateTime now = DateTime.Now;
        int difDay = (now - new DateTime(2024, 1, 1)).Days;
        double difHour = (now - new DateTime(2024, 1, 1)).TotalHours;
        int remainHour = (int)((difDay / 7 + 1) * 7 * 24 - difHour);
        txtRemainTime.text = remainHour / 24 + "d " + remainHour % 24 + "h";
        txtRemainTimeInEventPopup.text = remainHour / 24 + "d " + remainHour % 24 + "h";

        bool eventComplete = PlayerPrefs.GetInt(Contains.CurrentEvent + "_" + 0 + "_Complete", 0) == 1 &&
                             PlayerPrefs.GetInt(Contains.CurrentEvent + "_" + 1 + "_Complete", 0) == 1 &&
                             PlayerPrefs.GetInt(Contains.CurrentEvent + "_" + 2 + "_Complete", 0) == 1;

        txtEventButton.text = eventComplete ? "Complete" : "Play";
    }

    #endregion

    public void OnPlayClick()
    {
        Debug.LogError("==========TODO Implement play");
    }
}