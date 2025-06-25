using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class EventChallengeButton : MonoBehaviour
{
    public GameObject background;

    public void Reset()
    {
        GetComponent<Toggle>().interactable = true;
        background.SetActive(true);
    }

    public void Hide()
    {
        GetComponent<Toggle>().interactable = false;
        background.SetActive(false);
    }
}