using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DialogYesNo : DialogInterface {

    [Header("UI")]
    [SerializeField]
    private Text[] UITitle;

    [SerializeField]
    private Text[] UIMessage;

	[Header ("Contents")]
	public Transform Content_Default;

	public Transform Content_Candy;

    // ======================== Variables ====================== //

    protected string title;

    protected string message;

    // ============================ Action ====================== //
    protected System.Action OnClose;

    protected System.Action OnYes;

    protected System.Action OnNo;

    public void Init(string title, string message, System.Action OnYes = null, System.Action OnNo = null, System.Action OnClose = null)
    {


        this.title = title;

        this.message = message;

        this.OnNo = OnNo;

        this.OnYes = OnYes;

        this.OnClose = OnClose;
    }

    public override void Show()
    {
		if (GamePlay.Instance != null) {
			if (GamePlay.Instance.GameThemes == Enums.Themes.Default) {
				Content_Default.gameObject.SetActive (true);

				Content_Candy.gameObject.SetActive (false);

				Content_Default.SetAsFirstSibling ();
			} else {
				Content_Default.gameObject.SetActive (false);

				Content_Candy.gameObject.SetActive (true);

				Content_Candy.SetAsFirstSibling ();
			}
		}

		for (int i = 0; i < UITitle.Length; i++) {


			UITitle[i].text = title;
		}

		for (int i = 0; i < UIMessage.Length; i++) {


			UIMessage[i].text = message;
		}


       

        base.Show();  
    }

    public override void Close(Action OnClose = null)
    {
        Hide();  

        base.Close(OnClose);

        if (this.OnClose != null)
        {
            this.OnClose();
        }
    }

    public override void Close()
    {
		SoundSystems.Instance.PlaySound (Enums.SoundIndex.Press);

        Hide();

        base.Close();

        if (this.OnClose != null)
        {
            this.OnClose();
        }
    }

    public void No()
    {
		SoundSystems.Instance.PlaySound (Enums.SoundIndex.Press);

        Hide();

        if (OnNo != null)
        {
            OnNo();
        }       
    }

    public void Yes()
    {
		//AdsManager.instance.ShowInterstitial ();

		SoundSystems.Instance.PlaySound (Enums.SoundIndex.Press);

        Hide();

        if ( OnYes  != null)
        {
            OnYes();
        }     
    }
}
