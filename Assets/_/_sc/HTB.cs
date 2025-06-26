using ImpulseVibrations;
using SimpleSolitaire.Model.Config;
using UnityEngine;
using UnityEngine.UI;


public class HTB : MonoBehaviour
{
    public ImpactTypeFeedback hapticType = ImpactTypeFeedback.IMPACT_LIGHT;
    public const long LIGHT = 25;
    public const long MED = 40;
    public const long HEAVY = 60;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            long sensitive = hapticType switch
            {
                ImpactTypeFeedback.IMPACT_LIGHT => LIGHT,
                ImpactTypeFeedback.IMPACT_MEDIUM => MED,
                ImpactTypeFeedback.IMPACT_HEAVY => HEAVY,
                _ => 0
            };
            DoHaptic(sensitive, hapticType);
        });
    }

    public static void DoHaptic(long sensitive, ImpactTypeFeedback hapticType = ImpactTypeFeedback.IMPACT_LIGHT)
    {
        if (Data.haptic)
        {
            Vibrator.AndroidVibrate(sensitive);
            Vibrator.iOSVibrate(hapticType);
            // Debug.LogError("Do haptic");
        }
    }



}
