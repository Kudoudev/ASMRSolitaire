using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogThemes : DialogInterface {

	[Header ("Contents")]
	public Transform Content_Default;

	public Transform Content_Candy;

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
	/// Changes the theme.
	/// </summary>
	/// <param name="id">Identifier.</param>
	public void ChangeTheme(int id)
	{
		SoundSystems.Instance.PlaySound (Enums.SoundIndex.Press);

		switch (id) {
		case 0:

			GamePlay.Instance.ChangeStyleCards (Enums.Themes.Default);

			break;
		case 1:

			GamePlay.Instance.ChangeStyleCards (Enums.Themes.Candy);

			break;
		default:

			GamePlay.Instance.ChangeStyleCards (Enums.Themes.Default);

			break;
		}

		Hide ();
	}

	public override void Close (System.Action OnClose)
	{
		Hide ();

		base.Close (OnClose);
	}


}
