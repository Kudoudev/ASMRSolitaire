using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventAchiItem : MonoBehaviour
{
    public Image imgPostcard;
    public TMP_Text txtEventName;
    private string _eventName;
    private int _postcardId;

    public void SetPostcard(string eventName, int postcardId)
    {
        _eventName = eventName;
        _postcardId = postcardId;
        
        imgPostcard.sprite = Resources.Load<Sprite>("Textures/Event/" + eventName + "/" + postcardId);
        txtEventName.text = eventName;
    }

    public bool CheckPostcard(string eventName, int id)
    {
        return _eventName == eventName && _postcardId == id;
    }
}
