using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EventCompletePopup : MonoBehaviour
{
    public Image imgPostcard;
    public Button btnOk;

    public void Show(int postcardId, Action callback)
    {
        imgPostcard.sprite = Resources.Load<Sprite>("Textures/Event/" + Contains.CurrentEvent + "/" + postcardId);
        btnOk.onClick.RemoveAllListeners();
        btnOk.onClick.AddListener(()=>
        {
            callback.Invoke();
            gameObject.SetActive(false);
        });
        gameObject.SetActive(true);
    }
}
