using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MovementEffects;
using DG.Tweening;
using UnityEngine.UI;

public class HintCardsManager : SingletonMonoBehaviour < HintCardsManager > {

	/// <summary>
	/// The holder hint cards.
	/// </summary>
	public Transform HolderHintCards;

	/// <summary>
	/// The cards.
	/// </summary>
	public Image Cards;

	/// <summary>
	/// The handle.
	/// </summary>
	protected CoroutineHandle handle;

	/// <summary>
	/// The is break.
	/// </summary>
	protected bool IsBreak;

	public void ShowHint(Vector3 startPosition, Vector3 endPosition , Sprite image)
	{
		if (handle != null) {
			
			IsBreak = true;

			Timing.KillCoroutines (handle);
		}

		Cards.sprite = image;

		handle = Timing.RunCoroutine (StartHint(startPosition, endPosition ));
	}

	public void DisableHint()
	{
		IsBreak = true;

		if (handle != null) {
			Timing.KillCoroutines (handle);
		}

		HolderHintCards.gameObject.SetActive (false);
	}

	IEnumerator < float > StartHint(Vector3 startPosition, Vector3 endPosition)
	{
		HolderHintCards.gameObject.SetActive (true);

		bool IsCompletedMoving = true;

		IsBreak = false;

		bool IsCompletedFirstTime = false;

		while (!IsBreak) {
			if (IsCompletedMoving == false)
				yield return 0f;
			else {

				if (IsCompletedFirstTime) {

					HolderHintCards.gameObject.SetActive (false);

					yield return Timing.WaitForSeconds (Contains.DurationFade);

					HolderHintCards.gameObject.SetActive (true);
				}

				Cards.DOComplete (true);

				IsCompletedMoving = false;

				Cards.transform.position = startPosition;

				Cards.color = Color.white;

				Cards.transform.DOMove (endPosition, Contains.DurationPreview).OnComplete ( ()=>
					{
						IsCompletedMoving = true;
					});

				Cards.DOFade (0.5f, Contains.DurationPreview);

				IsCompletedFirstTime = true;
			}
		}

		yield return 0f;
	}
}
