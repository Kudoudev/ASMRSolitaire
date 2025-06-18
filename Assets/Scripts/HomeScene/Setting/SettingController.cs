using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingController : MonoBehaviour
{
    public TMP_Text txtDrawMode;
    public Toggle tglRightHandMode, tglMusic, tglSound;

    private void OnEnable()
    {
        txtDrawMode.text = GameManager.Instance.ModeGame == Enums.ModeGame.Easy ? "Draw 1" : "Draw 3";
        tglRightHandMode.isOn = Contains.IsRightHanded;
        tglMusic.isOn = Contains.IsMusicOn;
        tglSound.isOn = Contains.IsSoundOn;
    }

    public void OnRightHandModeChange(bool val)
    {
        Contains.IsRightHanded = val;
    }

    public void OnMusicChange(bool value)
    {
        if (value) {

            Contains.IsMusicOn = true;

            SoundSystems.Instance.EnableMusic ();
        } else {

            Contains.IsMusicOn = false;

            SoundSystems.Instance.DisableMusic ();
        }
    }

    public void OnSoundChange(bool value)
    {
        if (value) {

            Contains.IsSoundOn = true;

            SoundSystems.Instance.EnableSound();
        } else {

            Contains.IsSoundOn = false;

            SoundSystems.Instance.DisableSound();
        }
    }
}
