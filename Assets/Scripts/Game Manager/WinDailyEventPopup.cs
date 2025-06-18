using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinDailyEventPopup : MonoBehaviour
{
    public Image imgIcon;
    public TMP_Text txtTile, txtDailyChallenge, txtScore, txtMove, txtTime, txtBestTime;
    public GameObject newBest;

    private void OnEnable()
    {
        if (GameManager.Instance.PlayMode == Enums.PlayMode.Daily)
        {
            txtTile.text = "Congratulations!";
            txtDailyChallenge.gameObject.SetActive(true);

            DateTime playDate = GameManager.Instance.playDate;
            string day = Contains.ConvertMonth(playDate.Month) + " " + playDate.Day;

            txtDailyChallenge.text = "You have completed the daily challenge\nfor " + day + "!";
        }
        else
        {
            txtTile.text = "Event Level\nCompleted!";
            txtDailyChallenge.gameObject.SetActive(false);
        }

        txtScore.text = Contains.Score.ToString();
        txtMove.text = Contains.Moves.ToString();
        txtTime.text = TimeSystem.Instance.GetTimeDisplay();
        newBest.gameObject.SetActive(TimeSystem.Instance.GetTimeInSecond() < Contains.BestTimeSolitaire);
        Contains.BestTimeSolitaire = Mathf.Min(TimeSystem.Instance.GetTimeInSecond(), Contains.BestTimeSolitaire);
        txtBestTime.text = TimeSystem.Instance.ConvertSecondToFormat(Contains.BestTimeSolitaire);
    }
}