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
        while (!listPostcard[postcardId].NextChallenge())
        {
            if (postcardId >= 2)
            {
                btnPlay.gameObject.SetActive(false);
                return;
            }
            postcardId++;
            listPostcard[postcardId].UnlockPostcard();
            scroll.StartingPanel = postcardId;
        }
    }

    private void OnEnable()
    {
        if (scroll.CenteredPanel != postcardId) scroll.GoToPanel(postcardId);
    }

    public void OnChangePostcard()
    {
        btnPlay.transform.GetChild(0).GetComponent<TMP_Text>().text =
            scroll.CenteredPanel == postcardId ? "Play" : "Current Postcard";
    }

    private int postcardId = 0, challengeId = 0;

    public void OnPlayClick()
    {
        if (scroll.CenteredPanel == postcardId)
        {
            HomeSceneController.Instance.GetComponent<CanvasGroup>().blocksRaycasts = false;
            challengeId = listPostcard[postcardId].GetSelectingChallenge();
            GameManager.Instance.PlayMode = Enums.PlayMode.Event;
            GameManager.Instance.startingHandsToLoad =
                (int)Contains.StartOfWeek(DateTime.Today).Ticks / (postcardId + 2) + challengeId;
            SceneLoader.Instance.LoadScene(Contains.GamePlayScene, LoadSceneMode.Additive);
        }
        else
        {
            scroll.GoToPanel(postcardId);
        }
    }

    public void OnWin()
    {
        PlayerPrefs.SetInt(Contains.CurrentEvent + "_" + postcardId + "_" + challengeId, 1);
    }

    #region Show Win

    public void OnShowWin()
    {
        bool isNextPostcard = !listPostcard[postcardId].NextChallenge();
        if (isNextPostcard)
        {
            PlayerPrefs.SetInt(Contains.CurrentEvent + "_" + postcardId + "_Complete", 1);
            listPostcard[postcardId].imgEvent.transform.DOShakeScale(1f, 1f, 1).OnComplete(() =>
            {
                eventCompletePopup.Show(postcardId, () =>
                {
                    if (postcardId >= 2)
                    {
                        btnPlay.gameObject.SetActive(false);
                        return;
                    }
                    postcardId++;
                    listPostcard[postcardId].UnlockPostcard();
                    scroll.GoToPanel(postcardId);
                });
            });
        }

        HomeSceneController.Instance.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    #endregion
}