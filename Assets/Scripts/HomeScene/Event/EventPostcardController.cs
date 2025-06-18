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

    private void Awake()
    {
        bool unlockedPostcard = PlayerPrefs.GetInt(Contains.CurrentEvent + "_" + postcardId, 0) == 1;
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
        PlayerPrefs.SetInt(Contains.CurrentEvent + "_" + postcardId, 1);
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
            if (PlayerPrefs.GetInt(Contains.CurrentEvent + "_" + postcardId + "_" + i, 0) == 1)
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
}