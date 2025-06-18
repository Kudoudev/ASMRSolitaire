using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutilResolution : RegisterSystem {

	public static MutilResolution Instance;

	public bool IsPortraitView {
		get { return Screen.width < Screen.height; }
	}

	public bool IsPortrait;


	void Awake()
	{
		Instance = this;
	}

	void OnDestroy()
	{
		Instance = null;
	}

	/// <summary>
	/// Awake this instance.
	/// </summary>
	public override void OnEnable()
	{
		base.OnEnable ();

		IsPortrait = IsPortraitView;
	}

	/// <summary>
	/// Raises the update event.
	/// </summary>
	public override void OnUpdate ()
	{
		base.OnUpdate ();

		if (!IsPortrait && IsPortraitView) {
			
			IsPortrait = true;

			UpdatePortraitContents ();
		
		} else if ( IsPortrait && !IsPortraitView ) {
		
			IsPortrait = false;

			UpdateLandscapeContents ();
		}	
	}

	public void UpdatePortraitContents()
	{
		if (GamePlay.Instance != null) {
			GamePlay.Instance.UpdatePortrait ();
		}
	}

	public void UpdateLandscapeContents()
	{
		if (GamePlay.Instance != null) {
			GamePlay.Instance.UpdateLandscape ();
		}
	}
}
