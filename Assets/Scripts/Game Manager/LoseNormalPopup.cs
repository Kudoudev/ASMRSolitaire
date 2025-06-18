using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoseNormalPopup : MonoBehaviour
{
    public TMP_Text txtScore;

    private void OnEnable()
    {
        txtScore.text = Contains.Score.ToString();
    }
}
