using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class have any static global variables.
/// </summary>
public static class Contains
{
    /// <summary>
    /// Ammount of saved starting hands.
    /// </summary>
    public const int StartingHandsCount = 7;

    /// <summary>
    /// The home scene.
    /// </summary>
    public const string HomeScene = "HomeScene";

    /// <summary>
    /// The game play scene.
    /// </summary>
    public const string GamePlayScene = "GamePlayScene";

    /// <summary>
    /// The distance sort unlocked cards.
    /// </summary>
    public const float DistanceSortUnlockedCards = 0.7f;

    /// <summary>
    /// The distance sort locked cards.
    /// </summary>
    public const float DistanceSortLockedCards = 0.3f;

    /// <summary>
    /// The distance sort review.
    /// </summary>
    public const float DistanceSortReview = 0.1f;

    /// <summary>
    /// The distance sort hint cards.
    /// </summary>
    public const float DistanceSortHintCards = 0.5f;

    /// <summary>
    /// The maximum holder cards.
    /// </summary>
    public const int MaximumHolderCards = 7;

    /// <summary>
    /// The off set width card.
    /// </summary>
    public const float OffSetWidthCard = 1.5f;

    /// <summary>
    /// The off set height card.
    /// </summary>
    public const float OffSetHeightCard = 2f;

    #region Animation

    /// <summary>
    /// The duration moving.
    /// </summary>
    public const float DurationMoving = 0.1f;

    /// /// <summary>
    /// The duration draw.
    /// </summary>
    public const float DurationDraw = 0.07f;

    /// <summary>
    /// The duration fade.
    /// </summary>
    public const float DurationFade = 0.4f;

    #endregion

    #region Wait Coroutine

    /// <summary>
    /// The duration preview.
    /// </summary>
    public const float DurationPreview = 1f;

    #endregion

    #region static

    /// <summary>
    /// The is sound on.
    /// </summary>
    public static bool IsSoundOn
    {
        get => PlayerPrefs.GetInt("IsSoundOn", 1) == 1;
        set => PlayerPrefs.SetInt("IsSoundOn", value ? 1 : 0);
    }

    /// <summary>
    /// The is music on.
    /// </summary>
    public static bool IsMusicOn
    {
        get => PlayerPrefs.GetInt("IsMusicOn", 1) == 1;
        set => PlayerPrefs.SetInt("IsMusicOn", value ? 1 : 0);
    }


    /// <summary>
    /// The moves.
    /// </summary>
    private static int moves = 0;

    /// <summary>
    /// Gets or sets the moves.
    /// </summary>
    /// <value>The moves.</value>
    public static int Moves
    {
        set { moves = Mathf.Clamp(value, 0, 99999); }

        get { return moves; }
    }

    #endregion

    /// <summary>
    /// The number cards.
    /// </summary>
    public const int NumberCards = 52;
    
    public static int UndoAmmount
    {
        get => PlayerPrefs.GetInt("UndoAmmount", 3);
        set => PlayerPrefs.SetInt("UndoAmmount", value);
    }
    
    public static int HintAmmount
    {
        get => PlayerPrefs.GetInt("HintAmmount", 3);
        set => PlayerPrefs.SetInt("HintAmmount", value);
    }

    /// <summary>
    /// The is having remove ad.
    /// </summary>
    public static bool IsHavingRemoveAd
    {
        get { return PlayerPrefs.GetInt("IsHaveRemoveAd", 0) == 1; }

        set
        {
            int param = value == true ? 1 : 0;

            PlayerPrefs.SetInt("IsHaveRemoveAd", param);
        }
    }

    /// <summary>
    /// Gets or sets the current style.
    /// </summary>
    /// <value>The current style.</value>
    public static Enums.Themes CurrentStyle
    {
        get
        {
            return PlayerPrefs.GetString("Style", "Default") == "Default" ? Enums.Themes.Default : Enums.Themes.Candy;
        }

        set
        {
            Enums.Themes param = value;

            switch (param)
            {
                case Enums.Themes.Candy:

                    PlayerPrefs.SetString("Style", "Candy");

                    break;
                case Enums.Themes.Default:

                    PlayerPrefs.SetString("Style", "Default");

                    break;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether this instance is right handed.
    /// </summary>
    /// <value><c>true</c> if this instance is right handed; otherwise, <c>false</c>.</value>
    public static bool IsRightHanded
    {
        get => PlayerPrefs.GetInt("RightHanded", 1) == 1;

        set
        {
            bool paramGet = value;

            if (paramGet == true)
            {
                PlayerPrefs.SetInt("RightHanded", 1);
            }
            else
            {
                PlayerPrefs.SetInt("RightHanded", 0);
            }
        }
    }

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
        "Event 60",
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
        return ListEvent[difDay / 7];
    }


    #region Score

    /// <summary>
    /// The score.
    /// </summary>
    public static int Score = 0;

    /// /// <summary>
    /// The score move cards.
    /// </summary>
    public const int ScoreMoveCards = 5;

    /// <summary>
    /// The score result cards.
    /// </summary>
    public const int ScoreResultCards = 10;

    #endregion

    #region Board Information

    public static int BestTimeSolitaire
    {
        get => PlayerPrefs.GetInt("BestTimeSolitaire", 86400);
        set => PlayerPrefs.SetInt("BestTimeSolitaire", value);
    }

    public static int BestScoreSolitaire
    {
        get => PlayerPrefs.GetInt("BestScoreSolitaire", 0);
        set => PlayerPrefs.SetInt("BestScoreSolitaire", value);
    }

    // /// <summary>
    // /// Gets or sets the best score on easy.
    // /// </summary>
    // /// <value>The best score on easy.</value>
    // public static int BestScoreOnEasy
    // {
    //     get { return PlayerPrefs.GetInt("BestScoreOnEasy", 0); }
    //
    //     set { PlayerPrefs.SetInt("BestScoreOnEasy", value); }
    // }
    //
    // /// <summary>
    // /// Gets or sets the best score on hard.
    // /// </summary>
    // /// <value>The best score on hard.</value>
    // public static int BestScoreOnHard
    // {
    //     get { return PlayerPrefs.GetInt("BestScoreOnHard", 0); }
    //
    //     set { PlayerPrefs.SetInt("BestScoreOnHard", value); }
    // }
    //
    // /// <summary>
    // /// Gets or sets the best moves on easy.
    // /// </summary>
    // /// <value>The best moves on easy.</value>
    // public static int BestMovesOnEasy
    // {
    //     get { return PlayerPrefs.GetInt("BestMovesOnEasy", 0); }
    //
    //     set { PlayerPrefs.SetInt("BestMovesOnEasy", value); }
    // }
    //
    // /// <summary>
    // /// Gets or sets the best move on hard.
    // /// </summary>
    // /// <value>The best move on hard.</value>
    // public static int BestMoveOnHard
    // {
    //     get { return PlayerPrefs.GetInt("BestMoveOnHard", 0); }
    //
    //     set { PlayerPrefs.SetInt("BestMoveOnHard", value); }
    // }
    //
    // /// <summary>
    // /// Gets or sets the best time on easy.
    // /// </summary>
    // /// <value>The best time on easy.</value>
    // public static float BestTimeOnEasy
    // {
    //     get { return PlayerPrefs.GetFloat("BestTimeOnEasy", 0f); }
    //     set { PlayerPrefs.SetFloat("BestTimeOnEasy", value); }
    // }
    //
    // /// <summary>
    // /// Gets or sets the best time on hard.
    // /// </summary>
    // /// <value>The best time on hard.</value>
    // public static float BestTimeOnHard
    // {
    //     get { return PlayerPrefs.GetFloat("BestTimeOnHard", 0f); }
    //     set { PlayerPrefs.SetFloat("BestTimeOnHard", value); }
    // }
    //
    // /// <summary>
    // /// Gets or sets the total time on easy.
    // /// </summary>
    // /// <value>The total time on easy.</value>
    // public static float TotalTimeOnEasy
    // {
    //     get { return PlayerPrefs.GetFloat("TotalTimeOnEasy", 0f); }
    //     set { PlayerPrefs.SetFloat("TotalTimeOnEasy", value); }
    // }
    //
    // /// <summary>
    // /// Gets or sets the total time on hard.
    // /// </summary>
    // /// <value>The total time on hard.</value>
    // public static float TotalTimeOnHard
    // {
    //     get { return PlayerPrefs.GetFloat("TotalTimeOnHard", 0f); }
    //     set { PlayerPrefs.SetFloat("TotalTimeOnHard", value); }
    // }

    #endregion
}