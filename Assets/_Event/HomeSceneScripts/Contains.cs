using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class have any static global variables.
/// </summary>
public static class Contains
{
    public static string ConvertMonth(int month)
    {
        string strMonth = "";
        switch (month)
        {
            case 1:
                strMonth += "January";
                break;
            case 2:
                strMonth += "February";
                break;
            case 3:
                strMonth += "March";
                break;
            case 4:
                strMonth += "April";
                break;
            case 5:
                strMonth += "May";
                break;
            case 6:
                strMonth += "June";
                break;
            case 7:
                strMonth += "July";
                break;
            case 8:
                strMonth += "August";
                break;
            case 9:
                strMonth += "September";
                break;
            case 10:
                strMonth += "October";
                break;
            case 11:
                strMonth += "November";
                break;
            case 12:
                strMonth += "December";
                break;
        }

        return strMonth;
    }

    public static DateTime StartOfWeek(DateTime dt)
    {
        int diff = (7 + (dt.DayOfWeek - DayOfWeek.Monday)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }


    public static List<string> ListEvent = new List<string>()
    {
        "Event 1", "Event 2", "Event 3", "Event 4", "Event 5", "Event 6", "Event 7", "Event 8", "Event 9", "Event 10",
        "Event 11", "Event 12", "Event 13", "Event 14", "Event 15", "Event 16", "Event 17", "Event 18", "Event 19",
        "Event 20",
        "Event 21", "Event 22", "Event 23", "Event 24", "Event 25", "Event 26", "Event 27", "Event 28", "Event 29",
        "Event 30",
        "Event 31", "Event 32", "Event 33", "Event 34", "Event 35", "Event 36", "Event 37", "Event 38", "Event 39",
        "Event 40",
        "Event 41", "Event 42", "Event 43", "Event 44", "Event 45", "Event 46", "Event 47", "Event 48", "Event 49",
        "Event 50",
        "Event 51", "Event 52", "Event 53", "Event 54", "Event 55", "Event 56", "Event 57", "Event 58", "Event 59",
        "Event 60"
    };

    private static string currentEvent = "";

    public static string CurrentEvent
    {
        get
        {
            if (string.IsNullOrEmpty(currentEvent)) currentEvent = GetCurrentEvent();
            return currentEvent;
        }
    }

    public static string GetCurrentEvent()
    {
        DateTime now = DateTime.Now;
        int difDay = (now - new DateTime(2024, 1, 1)).Days;
        var d = difDay / 7;
        d = d % ListEvent.Count;
        return ListEvent[d];
    }

    public static int GetCurrentYear
    {
        get { return DateTime.Now.Year; }
    }

    public static bool GetPostCardUnlock(string _event, int _year, int _postCardId)
    {
        return PlayerPrefs.GetInt(GetKeySavePostCard(_event, _year, _postCardId), 0) == 1;
    }

    public static void SetPostCardUnlock(string _event, int _year, int _postCardId)
    {
        PlayerPrefs.SetInt(GetKeySavePostCard(_event, _year, _postCardId), 1);
    }

    public static bool GetChallengeComplete(string _event, int _year, int _postCardId, int _challenge)
    {
        return PlayerPrefs.GetInt(GetKeySaveChallenge(_event, _year, _postCardId, _challenge), 0) == 1;
    }

    public static void SetChallengeComplete(string _event, int _year, int _postCardId, int _challenge)
    {
        PlayerPrefs.SetInt(GetKeySaveChallenge(_event, _year, _postCardId, _challenge), 1);
    }

    public static string GetKeySavePostCard(string _event, int _year, int _postCardId)
    {
        return string.Format("{0}_{1}_{2}", _event, _year, _postCardId);
    }

    public static string GetKeySaveChallenge(string _event, int _year, int _postCardId, int _challenge)
    {
        return string.Format("{0}_{1}_{2}_{3}", _event, _year, _postCardId, _challenge);
    }

    public static int m_currentPostCard;
    public static int m_currentChallenge;

    public static void SetPlayingChallenge(int _postCard, int _challenge)
    {
        m_currentPostCard = _postCard;
        m_currentChallenge = _challenge;
    }

    //Call when play challenge mode and win.
    public static void OnPlayChallengeWin()
    {
        SetChallengeComplete(CurrentEvent, GetCurrentYear, m_currentPostCard, m_currentChallenge);
    }
}