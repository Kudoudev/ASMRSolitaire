using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpManager : SingletonMonoBehaviour<HelpManager>
{
    [Header("Undo")] public List<Image> listBtnUndo;
    public List<GameObject> listUndoAdsIcon;
    public List<Text> listTextUndoAmmount;

    [Header("Hint")] public List<Image> listBtnHint;
    public List<GameObject> listHintAdsIcon;
    public List<Text> listTextHintAmmount;

    private void Start()
    {
        foreach (var g in listUndoAdsIcon)
        {
            g.SetActive(Contains.UndoAmmount <= 0);
        }
        foreach (var t in listTextUndoAmmount)
        {
            t.text = Contains.UndoAmmount.ToString();
        }
        
        foreach (var g in listHintAdsIcon)
        {
            g.SetActive(Contains.HintAmmount <= 0);
        }
        foreach (var t in listTextHintAmmount)
        {
            t.text = Contains.HintAmmount.ToString();
        }
    }

    public void UpdateUndo(int change)
    {
        Contains.UndoAmmount += change;
        foreach (var g in listUndoAdsIcon)
        {
            g.SetActive(Contains.UndoAmmount <= 0);
        }
        foreach (var t in listTextUndoAmmount)
        {
            t.text = Contains.UndoAmmount.ToString();
        }
    }

    public void UpdateHint(int change)
    {
        Contains.HintAmmount += change;
        foreach (var g in listHintAdsIcon)
        {
            g.SetActive(Contains.HintAmmount <= 0);
        }
        foreach (var t in listTextHintAmmount)
        {
            t.text = Contains.HintAmmount.ToString();
        }
    }
}
