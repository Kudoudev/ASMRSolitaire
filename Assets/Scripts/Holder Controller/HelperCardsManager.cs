using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

/// <summary>
/// Helper cards manager.
/// </summary>
public class HelperCardsManager : SingletonMonoBehaviour < HelperCardsManager > {

	/// <summary>
	/// The holder default.
	/// </summary>
    [SerializeField]
    private RectTransform holderDefault;
	[SerializeField]
	private Transform txtStage;

	/// <summary>
	/// The cards in holder.
	/// </summary>
	[HideInInspector]
	public List < CardBehaviour > cardsMemoryTemp = new List<CardBehaviour>();


	/// <summary>
	/// Gets the holder.
	/// </summary>
	/// <returns>The holder.</returns>
    public Transform GetTheTransformOfHolder()
    {
        return holderDefault.transform;
    }

	/// <summary>
	/// Determines whether this instance is have cards in holder.
	/// </summary>
	/// <returns><c>true</c> if this instance is have cards in holder; otherwise, <c>false</c>.</returns>
    public bool IsExistsAnyCardsInTheHolder()
    {
        return cardsMemoryTemp.Count > 0;
    }

	/// <summary>
	/// Clears the holder.
	/// </summary>
    public void RefreshTheMemory()
    {
        cardsMemoryTemp.Clear();
    }

	public Vector3 GetWorldPosition ( Vector3 localPosition)
	{
		return holderDefault.transform.TransformPoint (localPosition);
	}

	public Vector3 GetLocalPosition ( Vector3 worldPosition)
	{
		return holderDefault.transform.InverseTransformPoint (worldPosition);
	}

	public void ShowStage(int stage)
	{
		txtStage.GetComponent<TMP_Text>().text = "Stage: " + stage;
		txtStage.DOScale(1f, 0f);
		txtStage.gameObject.SetActive(true);
		txtStage.DOScale(2f, 1f).OnComplete(() =>
		{
			txtStage.gameObject.SetActive(false);
		});
	}
}
