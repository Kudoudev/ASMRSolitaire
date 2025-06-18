using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawModeController : MonoBehaviour
{
    public Toggle tglDraw1;
    public TMP_Text txtDrawMode;

    private void OnEnable()
    {
        tglDraw1.isOn = GameManager.Instance.ModeGame == Enums.ModeGame.Easy;
    }

    public void OnDraw1SettingChange(bool val)
    {
        if (val)
        {
            GameManager.Instance.ModeGame = Enums.ModeGame.Easy;
            txtDrawMode.text = "Draw 1";
            PlayerPrefs.SetString("ModeGame", "Easy");
        }
        else
        {
            GameManager.Instance.ModeGame = Enums.ModeGame.Hard;
            txtDrawMode.text = "Draw 3";
            PlayerPrefs.SetString("ModeGame", "Hard");
        }
    }
}
