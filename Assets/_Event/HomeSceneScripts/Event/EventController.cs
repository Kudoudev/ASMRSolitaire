using System;
using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class EventController : MonoBehaviour
{
    public TMP_Text txtEventName;
    public List<EventPostcardController> listPostcard;
    public Button btnPlay;
    public SimpleScrollSnap scroll;
    public EventCompletePopup eventCompletePopup;

    private void Awake()
    {
        txtEventName.text = Contains.CurrentEvent;
    }

    private void OnEnable()
    {
        for (int i = 0; i < listPostcard.Count; i++)
        {
            var item = listPostcard[i];
            if (item.IsPostCardComplete())
            {
                item.UnlockPostcard();
            }
            else
            {
                item.OnProcessing();
                postcardId = i;
                scroll.StartingPanel = postcardId;
                break;
            }
        }

        if (scroll.CenteredPanel != postcardId) scroll.GoToPanel(postcardId);
        
    }

    public void OnChangePostcard()
    {
        btnPlay.transform.GetChild(0).GetComponent<TMP_Text>().text =
            scroll.CenteredPanel == postcardId ? "Play" : "Current Postcard";

        listPostcard[postcardId].OnProcessing();
    }

    private int postcardId = 0, challengeId = 0;

    public void OnPlayClick()
    {
        if (scroll.CenteredPanel == postcardId)
        {
            Contains.SetPlayingChallenge(postcardId, listPostcard[postcardId].GetSelectingChallenge());

            Debug.LogError("==========TODO Implement play event");
            //Debug===
            Contains.OnPlayChallengeWin();
            listPostcard[postcardId].NextChallenge();
            //Debug===
        }
        else
        {
            scroll.GoToPanel(postcardId);
        }
    }

    public void OnWin()
    {
        //set complete challenge
        Contains.SetChallengeComplete(Contains.CurrentEvent, Contains.GetCurrentYear, postcardId, challengeId);
    }
}