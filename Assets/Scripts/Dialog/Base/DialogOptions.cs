using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogOptions : DialogInterface {

	[Header ("Contents")]
	/// <summary>
	/// The content default.
	/// </summary>
	public Transform Content_Default;

	/// <summary>
	/// The content candy.
	/// </summary>
	public Transform Content_Candy;

	[Header ("Toggle")]
	/// <summary>
	/// The handle.
	/// </summary>
	public Toggle[] Handle;

	/// <summary>
	/// The sound.
	/// </summary>
	public Toggle[] Sound;

	/// <summary>
	/// The music.
	/// </summary>
	public Toggle[] Music;

	/// <summary>
	/// The remove ads.
	/// </summary>
	public Button[] RemoveAds;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start()
	{
		for (int i = 0; i < Handle.Length; i++) {
			Handle [i].onValueChanged.AddListener ((bool arg0) => {
				
				if (arg0 == true) {
					Contains.IsRightHanded = true;

				} else {
					Contains.IsRightHanded = false;
				}		

				if (GamePlay.Instance != null && MutilResolution.Instance != null) {
					if (MutilResolution.Instance.IsPortrait) {
						GamePlay.Instance.UpdatePortrait ();	
					} else {
						GamePlay.Instance.UpdateLandscape ();
					}
				}
			});
		}

		for (int i = 0; i < Sound.Length; i++) {

			if (Sound != null) {
				Sound[i].onValueChanged.AddListener ((bool arg0) => {
					if (arg0 == true) {

						Contains.IsSoundOn = true;

						SoundSystems.Instance.EnableSound ();
					} else {
						Contains.IsSoundOn = false;

						SoundSystems.Instance.DisableSound ();
					}			

				});
			}
		}

		for (int j = 0; j < Music.Length; j++) {

			if (Music != null) {
				Music[j].onValueChanged.AddListener ((bool arg0) => {
		
					if (arg0 == true) {

						Contains.IsMusicOn = true;

						SoundSystems.Instance.EnableMusic ();
					} else {

						Contains.IsMusicOn = false;

						SoundSystems.Instance.DisableMusic ();
					}
			
				});
			}
		}
	}

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		for (int i = 0; i < Handle.Length; i++) {

			if (Contains.IsRightHanded) {
				if (Handle != null) {
					Handle[i].isOn = true;
				}
			} else {
				if (Handle != null) {
					Handle[i].isOn = false;
				}
			}
		}

		for (int i = 0; i < RemoveAds.Length; i++) {

			if (Contains.IsHavingRemoveAd) {
				RemoveAds[i].interactable = false;
			}
		}

		for (int i = 0; i < Sound.Length; i++) {
			if (Contains.IsSoundOn) {
				Sound [i].isOn = true;
			}else{
				Sound [i].isOn = false;
			}
		}

		for (int i = 0; i < Music.Length; i++) {
			if (Contains.IsMusicOn) {
				Music [i].isOn = true;
			}else{
				Music [i].isOn = false;
			}
		}
	}

	/// <summary>
	/// Show this instance.
	/// </summary>
	public override void Show ()
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

		base.Show ();	
	}

	/// <summary>
	/// Shows the themes dialog.
	/// </summary>
	public void ShowThemesDialog()
	{
		DialogSystem.Instance.ShowDialogThemes ();
	}

	/// <summary>
	/// Shows the remove ads.
	/// </summary>
	public void ShowRemoveAds(){
		
		SoundSystems.Instance.PlaySound (Enums.SoundIndex.Press);

		if ( SceneManager.GetActiveScene ().name == Contains.GamePlayScene )
		{
			// DialogSystem.Instance.ShowYesNo ("Confirmation", string.Format ("Do you want to remove the ads with {0}", IapManager.Instance.ReturnThePrice ()), () => {
			// 	IapManager.Instance.BuyNonConsumable();
			// }	
			// );
		}
	}

	public override void Close (System.Action OnClose)
	{
		Hide ();

		base.Close (OnClose);
	}
}
