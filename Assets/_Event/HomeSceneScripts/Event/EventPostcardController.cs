using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EventPostcardController : MonoBehaviour
{
    public TMP_Text txtComplete;
    public GameObject imgLock, imgEvent;
    public int postcardId;
    public List<Toggle> listToggleChallenge;
    public bool IsProcessingPostcard { get; set; }
    private void Awake()
    {
        bool unlockedPostcard = Contains.GetPostCardUnlock(Contains.CurrentEvent, Contains.GetCurrentYear, postcardId);
        txtComplete.gameObject.SetActive(unlockedPostcard);
        imgLock.SetActive(!unlockedPostcard);
        imgEvent.GetComponent<Image>().sprite =
            Resources.Load<Sprite>("Textures/Event/" + Contains.CurrentEvent + "/" + postcardId);

        foreach (Toggle toggle in listToggleChallenge)
        {
            toggle.GetComponent<EventChallengeButton>().Reset();
        }
    }

    private void OnEnable()
    {
        NextChallenge();
    }

    public void UnlockPostcard()
    {
        Contains.SetPostCardUnlock(Contains.CurrentEvent, Contains.GetCurrentYear, postcardId);
        txtComplete.gameObject.SetActive(true);
        imgLock.SetActive(false);
    }

    public int GetSelectingChallenge()
    {
        for (int i = 0; i < listToggleChallenge.Count; i++)
        {
            if (listToggleChallenge[i].GetComponent<Toggle>().isOn) return i;
        }

        return 0;
    }

    public bool NextChallenge()
    {
        int curChallenge = -1, completed = 0;
        for (int i = listToggleChallenge.Count - 1; i >= 0; i--)
        {
            if (Contains.GetChallengeComplete(Contains.CurrentEvent, Contains.GetCurrentYear, postcardId, i))
            {
                listToggleChallenge[i].GetComponent<EventChallengeButton>().Hide();
                completed++;
            }
            else
            {
                curChallenge = i;
            }
        }

        txtComplete.text = completed + "/" + listToggleChallenge.Count;

        if (curChallenge != -1)
        {
            listToggleChallenge[curChallenge].isOn = true;
            return true;
        }

        return false;
    }

    public bool IsPostCardComplete()
    {
        for (int i = listToggleChallenge.Count - 1; i >= 0; i--)
        {
            if (Contains.GetChallengeComplete(Contains.CurrentEvent, Contains.GetCurrentYear, postcardId, i) == false)
            {
                return false;
            }
        }
        return true;
    }

    public void OnProcessing()
    {
        imgLock.SetActive(false);
    }
}