using MovementEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSystem : SingletonMonoBehaviour < DialogSystem > {

    // ============================= References ======================== //

#region Refenreces

	/// <summary>
	/// The current dialog.
	/// </summary>
    [HideInInspector]
    public DialogInterface CurrentDialog;


    [Header("Prefabs")]

	/// <summary>
	/// The prefab dialog yes no.
	/// </summary>
    [SerializeField]
    private DialogYesNo prefabDialogYesNo;

	/// <summary>
	/// The prefab dialog themes.
	/// </summary>
	[SerializeField]
	private DialogThemes prefabDialogThemes;

	/// <summary>
	/// The prefab dialog options.
	/// </summary>
	[SerializeField] 
	private DialogOptions prefabDialogOptions;

    // ========================== Variables ================================ //
	/// <summary>
	/// The dialog yes no.
	/// </summary>
    private DialogYesNo dialogYesNo;

	/// <summary>
	/// The dialog themes.
	/// </summary>
	private DialogThemes dialogThemes;

	/// <summary>
	/// The dialog options.
	/// </summary>
	private DialogOptions dialogOptions;
    #endregion


    // ============================ Properties =========================== //

	/// <summary>
	/// Dos the show.
	/// </summary>
	/// <param name="dialog">Dialog.</param>
    protected void DoShow(DialogInterface dialog)
    {
        Timing.RunCoroutine(ShowDialog(dialog));
    }

	/// <summary>
	/// Shows the dialog.
	/// </summary>
	/// <returns>The dialog.</returns>
	/// <param name="dialog">Dialog.</param>
    IEnumerator < float > ShowDialog(DialogInterface dialog)
    {
        bool IsComplete = false;

        if (CurrentDialog != null)
        {
            CurrentDialog.Close( ()=>
            {
                IsComplete = true;
            });
        }else
        {
            IsComplete = true;
        }

        while (IsComplete == false)
        {
            yield return Timing.WaitForOneFrame;
        }

        CurrentDialog = dialog;

        CurrentDialog.Show();
    }

	/// <summary>
	/// Instances the dialog.
	/// </summary>
	/// <returns>The dialog.</returns>
	/// <param name="dialog">Dialog.</param>
    protected GameObject InstanceDialog(DialogInterface dialog)
    {
        if ( ReferenceEquals (dialog, null ) )
        {
            LogGame.DebugLog("[Dialog System] Can't find the dialog.");

            return null;
        }

        var param = Instantiate(dialog.gameObject, this.transform) as GameObject;

		param.gameObject.SetActive (false);

        return param;
    }

#region Properties

	/// <summary>
	/// Determines whether this instance is have dialog using.
	/// </summary>
	/// <returns><c>true</c> if this instance is have dialog using; otherwise, <c>false</c>.</returns>
    public bool IsHaveDialogUsing()
    {
        return CurrentDialog != null;
    }

#endregion

	/// <summary>
	/// Shows the yes no.
	/// </summary>
	/// <param name="title">Title.</param>
	/// <param name="message">Message.</param>
	/// <param name="OnYes">On yes.</param>
	/// <param name="OnNo">On no.</param>
	/// <param name="OnClose">On close.</param>
    public void ShowYesNo(string title, string message, System.Action OnYes = null , System.Action OnNo = null, System.Action OnClose = null)
    {
        if ( dialogYesNo == null )
        {
            var paramGet = InstanceDialog(prefabDialogYesNo);

            if ( ReferenceEquals (paramGet, null))
            {
                return;
            }

            dialogYesNo = paramGet.GetComponent<DialogYesNo>();
        }

        dialogYesNo.Init(title, message, OnYes, OnNo, OnClose);

        DoShow(dialogYesNo);
    }    

	/// <summary>
	/// Shows the dialog themes.
	/// </summary>
	public void ShowDialogThemes()
	{
		if (dialogThemes == null) {
			
			var paramGet = InstanceDialog (prefabDialogThemes);

			if (ReferenceEquals (paramGet, null)) {
				return;
			}

			dialogThemes = paramGet.GetComponent < DialogThemes > ();
		}

		DoShow (dialogThemes);
	}

	/// <summary>
	/// Shows the dialog options.
	/// </summary>
	public void ShowDialogOptions()
	{
		if (dialogOptions == null) {

			var paramGet = InstanceDialog (prefabDialogOptions);

			if (ReferenceEquals (paramGet, null)) {
				return;
			}

			dialogOptions = paramGet.GetComponent < DialogOptions > ();

		}

		DoShow (dialogOptions);
	}
}
