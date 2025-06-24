using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif

[System.Serializable]
public class NotificationData
{
    public string title, description;
}

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    private const string CHANNEL_ID = "daily_notifications";
    private const string CHANNEL_NAME = "Daily Reminders";

    public List<NotificationData> dailyMessages = new List<NotificationData>
    {
        new NotificationData { title = "Time to Unwind 💤", description = "Clear your mind with soft card sounds and smooth swipes 🃏." },
        new NotificationData { title = "Quiet Challenge Awaits 🧘", description = "Minimalist design, maximum calm. Return to Zen Solitaire 🕊️." },
        new NotificationData { title = "Evening Focus 🌙", description = "End your day with gentle gameplay and peaceful rhythm." },
        new NotificationData { title = "Match, Breathe, Repeat 🌿", description = "Every move is a breath. Every puzzle, a pause." },
        new NotificationData { title = "ASMR Session Ready 🎧", description = "Let the soft shuffle of cards reset your mind." },
        new NotificationData { title = "Tap In to Tune Out 🔇", description = "Quiet logic meets quiet moments. Press play for peace." },
        new NotificationData { title = "Your Zen Deck Is Waiting 🕊️", description = "Cards aligned. Vibes calm. You in?" },
        new NotificationData { title = "Soothe Your Scroll 📱", description = "Swap doomscrolling for something serene." },
        new NotificationData { title = "Recenter With Solitaire 🧩", description = "Minimal movement. Maximum clarity." },
        new NotificationData { title = "Peace, One Card at a Time ✨", description = "Slow play for fast calm." },
        new NotificationData { title = "Soft Stack Time 🧺", description = "Stack cards. Clear thoughts. Breathe easy." },
        new NotificationData { title = "Zen Focus Mode 🔲", description = "No ads, no noise, just flow." },
        new NotificationData { title = "Puzzle Meets Stillness 🔕", description = "Solitaire like a whisper—quiet, focused, healing." },
        new NotificationData { title = "Tap for Clarity 🎴", description = "Each swipe is a step toward peace." },
        new NotificationData { title = "Card Flow Activated 🌊", description = "Play at your own pace. No pressure. Just presence." },
        new NotificationData { title = "Unplug & Play 🔌", description = "Mindful minutes in a minimalist world." },
        new NotificationData { title = "Night Stack 🌘", description = "Low light. Low stress. High relaxation." },
        new NotificationData { title = "Minimal Moves, Major Calm 🎯", description = "Let the layout guide your focus." },
        new NotificationData { title = "Still Awake? 🌙", description = "Quietly wind down with a few thoughtful matches." },
        new NotificationData { title = "Solitaire for the Soul 💜", description = "A soft place to land after a long day." },
        new NotificationData { title = "Pause, Then Play ⏸️", description = "Breathe deep. Shuffle gently. Begin." },
        new NotificationData { title = "Tap. Flow. Exhale. 🫧", description = "Simple actions with soothing outcomes." },
        new NotificationData { title = "Soft Challenge Ahead ☁️", description = "No rush. Just rhythm and reason." },
        new NotificationData { title = "Play in Silence 🔇", description = "Silence isn’t empty. It’s full of Solitaire." },
        new NotificationData { title = "Low Light Mode 🔦", description = "For cozy evenings and calm minds." },
        new NotificationData { title = "Drift Into Focus 🌬", description = "Nothing loud. Just logic, breath, and space." },
        new NotificationData { title = "Zen O’Clock 🕓", description = "It’s always the right time to slow down." },
        new NotificationData { title = "Stillness Through Strategy 🧠", description = "Every layout a meditative journey." },
        new NotificationData { title = "Minimal Game, Max Peace 🧊", description = "Clean UI. Pure ASMR. Zero clutter." },
        new NotificationData { title = "Stacked in Harmony 🪷", description = "Solitaire in its calmest form." },
        new NotificationData { title = "Clear the Board, Clear the Mind 🧽", description = "Let logic and tranquility meet." },
        new NotificationData { title = "Flow State, Initiated 🌀", description = "No timers. No distractions. Just cards and clarity." },
        new NotificationData { title = "Your Daily Decompress 🚿", description = "A short game to rinse off the day." },
        new NotificationData { title = "Mindful Minutes Start Now 🧘", description = "Return to your quiet space." },
        new NotificationData { title = "Recharge in Silence 🔋", description = "Soft puzzles. Pure reset." },
        new NotificationData { title = "Still Time to Play 🌠", description = "Late night calm begins with a single swipe." },
        new NotificationData { title = "Let Go With Every Move 🌬️", description = "Drop stress with each match." },
        new NotificationData { title = "Card Therapy Begins 🛠️", description = "Subtle sounds, smooth logic, real peace." },
        new NotificationData { title = "You + Cards = Balance ⚖️", description = "A little challenge, a lot of calm." },
        new NotificationData { title = "Just You and the Deck 🌌", description = "Minimalist focus. Meditative flow." }
    };


    void RequestNotificationPermission()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
    try
    {
        using (var activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
            .GetStatic<AndroidJavaObject>("currentActivity"))
        {
            using (var permissionClass = new AndroidJavaClass("androidx.core.app.ActivityCompat"))
            {
                string permission = "android.permission.POST_NOTIFICATIONS";
                int permissionResult = activity.Call<int>("checkSelfPermission", permission);
                if (permissionResult != 0)
                {
                    permissionClass.CallStatic("requestPermissions", activity, new string[] { permission }, 0);
                    Debug.Log("Requested Android POST_NOTIFICATIONS permission");
                }
                else
                {
                    Debug.Log("Android POST_NOTIFICATIONS permission already granted");
                }
            }
        }
    }
    catch (Exception ex)
    {
        Debug.LogWarning("Could not request Android notification permission: " + ex.Message);
    }
#elif UNITY_IOS
        StartCoroutine(RequestIOSNotificationPermission());
#endif
    }


    System.Collections.IEnumerator RequestIOSNotificationPermission()
    {
#if UNITY_IPHONE && !UNITY_EDITOR
            var option = AuthorizationOption.Alert | AuthorizationOption.Badge | AuthorizationOption.Sound;
            var request = new AuthorizationRequest(option, true);

            // while (!request.IsFinished)
            // {
            //     yield return null;
            // }
            Debug.Log($"iOS notification permission granted: {request.Granted}");
#endif

        yield return null;

    }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeNotifications();
            RequestNotificationPermission(); // ← ADD THIS
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Schedule test notification 2 minutes after game starts
        // ScheduleTestNotification();

        // Schedule daily notifications
        ScheduleDailyNotifications();
    }

    void InitializeNotifications()
    {
#if UNITY_ANDROID
        // Android setup
        var channel = new AndroidNotificationChannel()
        {
            Id = CHANNEL_ID,
            Name = CHANNEL_NAME,
            Importance = Importance.High,
            Description = "Daily game reminders and updates",
            EnableLights = true,
            EnableVibration = true
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
        AndroidNotificationCenter.CancelAllNotifications();

        Debug.Log("Android notifications initialized");

#elif UNITY_IOS && !UNITY_EDITOR
        // iOS setup - request permission
        var authorizationOption = AuthorizationOption.Alert | AuthorizationOption.Badge | AuthorizationOption.Sound;
        using (var req = new AuthorizationRequest(authorizationOption, true))
        {
            // while (!req.IsFinished)
            // {
            //     System.Threading.Thread.Sleep(10);
            // }
            
            Debug.Log($"iOS notification permission: {req.Granted}");
        }
        
        iOSNotificationCenter.RemoveAllDeliveredNotifications();
        iOSNotificationCenter.RemoveAllScheduledNotifications();
        
        Debug.Log("iOS notifications initialized");
#endif
    }

    void ScheduleTestNotification()
    {
        ScheduleNotification(
            "Test Notification 🧪",
            "This is a test notification fired 2 minutes after game start!",
            DateTime.Now.AddMinutes(2)
        );

        Debug.Log("Test notification scheduled for 2 minutes from now");
    }

    void ScheduleDailyNotifications()
    {
        CancelDailyNotifications();

        DateTime start = DateTime.Now.AddHours(UnityEngine.Random.Range(3f, 6f));
        DateTime end = DateTime.Now.AddDays(7);
        int idCounter = 1;

        while (start < end)
        {
            var message = dailyMessages[UnityEngine.Random.Range(0, dailyMessages.Count)];
            ScheduleNotification(message.title, message.description, start, idCounter++);

            // Next interval: 3 to 6 hours later
            float nextHours = UnityEngine.Random.Range(3f, 6f);
            start = start.AddHours(nextHours);
        }

        Debug.Log($"Scheduled notifications every 3–6 hours for 7 days. Total: {idCounter - 1}");
    }

    void ScheduleNotification(string title, string message, DateTime fireTime, int identifier = -1)
    {
#if UNITY_ANDROID
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = message;
        notification.FireTime = fireTime;
        notification.SmallIcon = "icon_0";
        notification.LargeIcon = "icon_1";
        notification.ShouldAutoCancel = true;

        int notificationId = AndroidNotificationCenter.SendNotification(notification, CHANNEL_ID);

        if (identifier > 0)
        {
            PlayerPrefs.SetInt($"notification_id_{identifier}", notificationId);
        }

#elif UNITY_IOS && !UNITY_EDITOR
        var notification = new iOSNotification()
        {
            Identifier = identifier > 0 ? $"daily_notification_{identifier}" : $"test_notification_{DateTime.Now.Ticks}",
            Title = title,
            Body = message,
            Subtitle = "",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "game_reminder",
            ThreadIdentifier = "game_thread",
            Trigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan(fireTime.Ticks - DateTime.Now.Ticks),
                Repeats = false
            }
        };
        
        iOSNotificationCenter.ScheduleNotification(notification);
#endif

        Debug.Log($"Notification scheduled: '{title}' for {fireTime}");
    }

    public void CancelDailyNotifications()
    {
#if UNITY_ANDROID
        // Cancel by stored IDs
        for (int day = 1; day <= 30; day++)
        {
            int notificationId = PlayerPrefs.GetInt($"notification_id_{day}", -1);
            if (notificationId != -1)
            {
                AndroidNotificationCenter.CancelNotification(notificationId);
                PlayerPrefs.DeleteKey($"notification_id_{day}");
            }
        }

#elif UNITY_IOS && !UNITY_EDITOR
        // Cancel by identifier pattern
        for (int day = 1; day <= 30; day++)
        {
            iOSNotificationCenter.RemoveScheduledNotification($"daily_notification_{day}");
        }
#endif

        Debug.Log("Daily notifications cancelled");
    }

    public void CancelAllNotifications()
    {
#if UNITY_ANDROID
        AndroidNotificationCenter.CancelAllNotifications();
#elif UNITY_IOS 
        iOSNotificationCenter.RemoveAllScheduledNotifications();
        iOSNotificationCenter.RemoveAllDeliveredNotifications();
#endif

        Debug.Log("All notifications cancelled");
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            OnAppPaused();
        }
        else
        {
            OnAppResumed();
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
        {
            OnAppPaused();
        }
        else
        {
            OnAppResumed();
        }
    }

    void OnAppPaused()
    {
        // Reschedule notifications when app goes to background
        ScheduleDailyNotifications();
        Debug.Log("App paused - notifications rescheduled");
    }

    void OnAppResumed()
    {
        // Check if app was opened via notification
        CheckNotificationIntent();
        Debug.Log("App resumed - checking notification intent");
    }

    void CheckNotificationIntent()
    {
#if UNITY_ANDROID
        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();
        if (notificationIntentData != null)
        {
            HandleNotificationClick(notificationIntentData.Notification.Title, notificationIntentData.Notification.Text);
        }

#elif UNITY_IOS && !UNITY_EDITOR
        var notification = iOSNotificationCenter.GetLastRespondedNotification();
        if (notification != null)
        {
            HandleNotificationClick(notification.Title, notification.Body);
            iOSNotificationCenter.RemoveDeliveredNotification(notification.Identifier);
        }
#endif
    }

    void HandleNotificationClick(string title, string message)
    {
        Debug.Log($"User opened app via notification: {title}");

        // Handle notification click - give rewards, open specific screen, etc.
        if (title.Contains("Test Notification"))
        {
            Debug.Log("Test notification clicked!");
            // Handle test notification
        }
        else if (title.Contains("Come Back and Play"))
        {
            Debug.Log("Daily notification clicked!");
            // Give daily login bonus
            GiveDailyBonus();
        }
    }

    void GiveDailyBonus()
    {
        // Example: Give player rewards for returning
        Debug.Log("Daily bonus granted!");
        // PlayerData.AddCoins(100);
        // PlayerData.AddGems(10);
        // ShowWelcomeBackUI();
    }

    // Public methods for manual testing
    [ContextMenu("Test Notification Now")]
    public void TestNotificationNow()
    {
        ScheduleNotification("Instant Test 🚀", "This notification should appear immediately!", DateTime.Now.AddSeconds(5));
    }

    [ContextMenu("Reschedule Daily Notifications")]
    public void RescheduleDailyNotifications()
    {
        ScheduleDailyNotifications();
    }

    [ContextMenu("Cancel All Notifications")]
    public void TestCancelAll()
    {
        CancelAllNotifications();
    }

    // Show current scheduled notifications count (Android only)
    [ContextMenu("Show Scheduled Count")]
    public void ShowScheduledCount()
    {
#if UNITY_ANDROID
        Debug.Log($"Scheduled notifications: ");
#elif UNITY_IOS
        Debug.Log("iOS notification count not available at runtime");
#endif
    }

    void OnDestroy()
    {
        // Clean up when object is destroyed
        if (Instance == this)
        {
            Instance = null;
        }
    }
}