using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Time system.
/// </summary>
public class TimeSystem : SingletonMonoBehaviour<TimeSystem>
{
    // =============================== Variables ========================= //

    /// <summary>
    /// The hour.
    /// </summary>
    public int Hour;

    /// <summary>
    /// The minute.
    /// </summary>
    public int Minute;

    /// <summary>
    /// The second.
    /// </summary>
    public int Second;


    // ========================= Functional ======================= //

    /// <summary>
    /// Awake this instance.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        InvokeRepeating("UpdateTime", 0, 1);
    }

    /// <summary>
    /// Updates the time.
    /// </summary>
    protected void UpdateTime()
    {
        if (GameManager.Instance.IsGameEnd() || !GameManager.Instance.IsGameReady())
        {
            return;
        }

        Second = Mathf.Clamp(Second + 1, 0, 60);

        Minute = Mathf.Clamp(Second - 60 >= 0 ? Minute + 1 : Minute, 0, 60);

        Hour = Mathf.Clamp(Minute - 60 >= 0 ? Hour + 1 : Hour, 0, 999);

        if (Second >= 60)
        {
            Second = 0;
        }

        if (Minute >= 60)
        {
            Minute = 0;
        }
    }

    /// <summary>
    /// Gets the time display.
    /// </summary>
    /// <returns>The time display.</returns>
    public string GetTimeDisplay()
    {
        return Hour == 0
            ? string.Format("{0}:{1}", Minute.ToString("00"), Second.ToString("00"))
            : string.Format("{0}:{1}:{2}", Hour.ToString("00"), Minute.ToString("00"), Second.ToString("00"));
    }

    public string ConvertSecondToFormat(int _second)
    {
        int hour = _second / 3600;
        int minute = (_second % 3600) / 60;
        int second = _second % 60;
        return hour == 0
            ? string.Format("{0}:{1}", minute.ToString("00"), second.ToString("00"))
            : string.Format("{0}:{1}:{2}", hour.ToString("00"), minute.ToString("00"), second.ToString("00"));
    }

    public int GetTimeInSecond()
    {
        return Hour * 3600 + Minute * 60 + Second;
    }

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    protected override void OnDestroy()
    {
        base.OnDestroy();

        CancelInvoke();
    }
}