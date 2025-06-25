using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class YearItem : MonoBehaviour
{
    public GameObject monthItem, eventItem;
    public TMP_Text txtYear;

    private int year;
    private List<MonthItem> listMonthItem;
    private List<EventAchiItem> listEventAchi;
    public void SetYearDailyChallenge(int year)
    {
        listMonthItem = new List<MonthItem>();
        GetComponent<GridLayoutGroup>().cellSize = new Vector2(400, 690);
        this.year = year;
        txtYear.text = year.ToString();

        for (int i = 12; i >= 1; i--)
        {
            if (year < DateTime.Today.Year || (year == DateTime.Today.Year && i <= DateTime.Today.Month))
            {
                MonthItem month = Instantiate(monthItem, transform).GetComponent<MonthItem>();
                month.SetMonth(year, i);
                listMonthItem.Add(month);
            }
        }
    }

    public void ReloadDailyChallenge()
    {
        foreach (MonthItem month in listMonthItem)
        {
            month.Reload();
        }
    }

    public void SetYearEvent(int year)
    {
        listEventAchi = new List<EventAchiItem>();
        GetComponent<GridLayoutGroup>().cellSize = new Vector2(400, 475);
        this.year = year;
        txtYear.text = year.ToString();

        DateTime startDate = new DateTime(2024, 1, 1);
        int startEvent = (new DateTime(year, 1, 1) - startDate).Days / 7;
        if (Contains.StartOfWeek(new DateTime(year, 1, 1)).Year != year) startEvent++;

        int endEvent = (new DateTime(year, 12, 31) - startDate).Days / 7;


        int eventNumber = endEvent;
        Debug.Log("list event in year " + this.year + ": " + startEvent + " - " + endEvent);

        for (int i = startEvent; i <= endEvent; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int eventId = i % Contains.ListEvent.Count;
                string eventName = Contains.ListEvent[eventId];
                if (Contains.GetPostCardUnlock(eventName, year, j))
                {
                    EventAchiItem eventAchi = Instantiate(eventItem, transform).GetComponent<EventAchiItem>();
                    eventAchi.SetPostcard(eventName, j);
                    listEventAchi.Add(eventAchi);
                }
            }
        }
    }
    
    public void ReloadEvent()
    {
        for (int i = 0; i < 3; i++)
        {
            if(Contains.GetPostCardUnlock(Contains.CurrentEvent, this.year, i) == false)
            {
                continue;
            }
            bool added = false;
            foreach (EventAchiItem eventAchi in listEventAchi)
            {
                if (eventAchi.CheckPostcard(Contains.CurrentEvent, i))
                {
                    added = true;
                    break;
                }
            }

            if (!added)
            {
                EventAchiItem eventAchi = Instantiate(eventItem, transform).GetComponent<EventAchiItem>();
                eventAchi.SetPostcard(Contains.CurrentEvent, i);
                listEventAchi.Add(eventAchi);
            }
        }
    }
}