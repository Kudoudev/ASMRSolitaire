using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogInterface : MonoBehaviour {

    [Header("Animator")]
    [SerializeField]
    private Animator controller;

    protected int AnimtionClose = Animator.StringToHash("IsClose");

    protected int AnimationOpen = Animator.StringToHash("IsOpen");

    public virtual void OnTouchEscape()
    {
        //TODO: Close some things with key escape.

    }

    public virtual void Show()
    {
		gameObject.SetActive (true);

        controller.SetBool(AnimationOpen, true);

        controller.SetBool(AnimtionClose, false);

        GamePlay.Instance.EnableBlur();
    }

    public virtual void Close(System.Action OnClose = null)
    {
        if ( OnClose != null)
        {
            OnClose();
        }
    }

    public virtual void Close()
    {

    }

    public virtual void Hide()
    {
        controller.SetBool(AnimationOpen, false);

        controller.SetBool(AnimtionClose, true);

        if ( DialogSystem.Instance.CurrentDialog == this)
        {
            GamePlay.Instance.DisableBlur();

            DialogSystem.Instance.CurrentDialog = null;
        }
    }


	public void OnCompletedHide()
	{
		gameObject.SetActive (false);
	}

}
