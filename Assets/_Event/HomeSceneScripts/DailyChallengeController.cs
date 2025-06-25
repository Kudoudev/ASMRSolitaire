using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DailyChallengeController : MonoBehaviour
{
    public TMP_Text txtMonth, txtComplete;
    public List<DailyChallengeButton> listDayButton;
    public GameObject btnPreMonth, btnNextMonth, btnPlay, txtCompleteThisMonth;
    public Image imgCup;

    public DateTime SelectingDay;

    private void OnEnable()
    {
        //todo select last day can play
        SelectingDay = DateTime.Today;
        FindNotFinishDay();

        ResetCalendar();
    }

    public void OnChangeMonthClick(int change)
    {
        int year = SelectingDay.Year;
        int month = SelectingDay.Month;
        month += change;
        if (month > 12)
        {
            month = 1;
            year++;
        }
        else if (month < 1)
        {
            month = 12;
            year--;
        }

        for (int i = DateTime.DaysInMonth(year, month); i > 0; i--)
        {
            SelectingDay = new DateTime(year, month, i);
            if (SelectingDay > DateTime.Today) continue;
            if (PlayerPrefs.GetInt(SelectingDay.ToShortDateString(), 0) == 1) continue;
            break;
        }

        ResetCalendar();
    }

    private void ResetCalendar()
    {
        int year = SelectingDay.Year, month = SelectingDay.Month;
        //show btnNext if not current month
        btnNextMonth.SetActive(year < DateTime.Today.Year ||
                               (year == DateTime.Today.Year &&
                                month < DateTime.Today.Month));
        btnPreMonth.SetActive(year > 2024 || (year == 2024 && month > 1));
        string m = Contains.ConvertMonth(month) + " " + year;
        txtMonth.text = m;
        imgCup.sprite = Resources.Load<Sprite>("Textures/Cup/" + m);

        //start day is sunday = 0
        int startDay = ((int)new DateTime(year, month, 1).DayOfWeek + 6) % 7;

        int daysInMonth = DateTime.DaysInMonth(year, month);

        for (int i = 0; i < listDayButton.Count; i++)
        {
            DailyChallengeButton btnDay = listDayButton[i];
            if (i < startDay || i >= startDay + daysInMonth)
            {
                btnDay.Reset();
            }
            else
            {
                btnDay.SetDate(new DateTime(year, month, i + 1 - startDay));
            }
        }

        int complete = 0;
        for (int i = 1; i <= daysInMonth; i++)
        {
            if (PlayerPrefs.GetInt(new DateTime(year, month, i).ToShortDateString(), 0) == 1) complete++;
        }
        
        txtCompleteThisMonth.SetActive((year == DateTime.Today.Year && month == DateTime.Today.Month && complete == DateTime.Today.Day) || (complete == daysInMonth));
        btnPlay.SetActive(!txtCompleteThisMonth.activeSelf);

        txtComplete.text = complete + "/" + daysInMonth;
        imgCup.color = complete == daysInMonth ? Color.white : Color.black;
    }

    public void OnDayClick(DateTime date)
    {
        SelectingDay = date;
    }

    public void OnPlayClick()
    {
        Debug.Log("============TODO implement code play daily challenge");

        //Debug
        StartCoroutine(OnShowWin());
    }

    #region Show Win

    public Transform ball;

    public IEnumerator OnShowWin()
    {
        if (PlayerPrefs.GetInt(SelectingDay.ToShortDateString(), 0) == 1)
        {
            HomeSceneController.Instance.GetComponent<CanvasGroup>().blocksRaycasts = true;
            yield break;
        }

        yield return new WaitUntil(() => SceneManager.sceneCount == 1);
        PlayerPrefs.SetInt(SelectingDay.ToShortDateString(), 1);
        ball.DOKill();
        ball.DOScale(3, 0f);
        ball.position = Vector3.zero;
        ball.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        foreach (DailyChallengeButton dailyChallengeButton in listDayButton)
        {
            if (dailyChallengeButton.Date == SelectingDay)
            {
                ball.DOScale(1, 1f);
                ball.DOMove(dailyChallengeButton.transform.position, 1).OnComplete(() =>
                {
                    ball.gameObject.SetActive(false);
                    FindNotFinishDay();
                    ResetCalendar();
                    HomeSceneController.Instance.GetComponent<CanvasGroup>().blocksRaycasts = true;
                });
            }
        }
    }

    private void FindNotFinishDay()
    {
        while (PlayerPrefs.GetInt(SelectingDay.ToShortDateString(), 0) == 1 && SelectingDay >= new DateTime(2024, 1, 1))
        {
            SelectingDay = SelectingDay.AddDays(-1);
            return;
        }
        if(SelectingDay.Year == 2023) SelectingDay = DateTime.Today;
    }

    #endregion
}