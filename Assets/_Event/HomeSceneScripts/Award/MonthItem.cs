using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonthItem : MonoBehaviour
{
    public Image imgCup;
    public TMP_Text txtMonth, txtProgress;
    public Slider sldProgress;
    private int year, month;

    public void SetMonth(int year, int month)
    {
        this.month = month;
        this.year = year;
        imgCup.sprite = Resources.Load<Sprite>("Textures/Cup/" + Contains.ConvertMonth(month) + " " + year);
        txtMonth.text = Contains.ConvertMonth(month);
        
        int daysInMonth = DateTime.DaysInMonth(year, month);
        sldProgress.maxValue = daysInMonth;
        int complete = 0;
        for (int i = 1; i <= daysInMonth; i++)
        {
            if (PlayerPrefs.GetInt(new DateTime(year, month, i).ToShortDateString(), 0) == 1) complete++;
        }

        sldProgress.value = complete;
        txtProgress.text = complete + " of " + daysInMonth;
        imgCup.color = complete == daysInMonth ? Color.white : Color.black;
    }

    public void Reload()
    {
        int daysInMonth = DateTime.DaysInMonth(year, month);
        sldProgress.maxValue = daysInMonth;
        int complete = 0;
        for (int i = 1; i <= daysInMonth; i++)
        {
            if (PlayerPrefs.GetInt(new DateTime(year, month, i).ToShortDateString(), 0) == 1) complete++;
        }

        sldProgress.value = complete;
        txtProgress.text = complete + " of " + daysInMonth;
        imgCup.color = complete == daysInMonth ? Color.white : Color.black;
    }
}
