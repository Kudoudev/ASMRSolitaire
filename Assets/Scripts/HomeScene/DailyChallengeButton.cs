using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DailyChallengeButton : MonoBehaviour
{
    public DailyChallengeController dailyChallengeController;
    public GameObject complete;

    private TMP_Text _txtDay;
    private DateTime _date;

    public DateTime Date => _date;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
        Signals.Get<OnDayClick>().AddListener(OnDayClickSignals);
    }

    private void OnDestroy()
    {
        Signals.Get<OnDayClick>().RemoveListener(OnDayClickSignals);
    }

    public void Reset()
    {
        if (_txtDay == null) _txtDay = transform.GetChild(0).GetComponent<TMP_Text>();
        _txtDay.text = "";
        GetComponent<Button>().interactable = false;
        GetComponent<Image>().DOFade(0, 0);
        complete.SetActive(false);
    }

    public void SetDate(DateTime date)
    {
        _date = date;
        if (_txtDay == null) _txtDay = transform.GetChild(0).GetComponent<TMP_Text>();
        _txtDay.text = date.Day.ToString();
        GetComponent<Button>().interactable = _date <= DateTime.Today;
        GetComponent<Image>().DOFade(_date != dailyChallengeController.SelectingDay ? 0 : 1, 0);

        complete.SetActive(PlayerPrefs.GetInt(_date.ToShortDateString(), 0) == 1);
    }

    public void OnClick()
    {
        dailyChallengeController.OnDayClick(_date);
        Signals.Get<OnDayClick>().Dispatch(_date);
    }

    public void OnDayClickSignals(DateTime clickDate)
    {
        GetComponent<Image>().DOFade(_date != clickDate ? 0 : 1, 0);
    }
}