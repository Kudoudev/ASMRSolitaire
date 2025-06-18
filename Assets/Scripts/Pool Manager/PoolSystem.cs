using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pool system.
/// </summary>
public class PoolSystem : SingletonMonoBehaviour < PoolSystem > {

	// ============================= References =========================== //

	#region References

	/// <summary>
	/// The item properties.
	/// </summary>
	[SerializeField]
	protected List < CardBehaviour > ItemProperties;

	#endregion

	#region Functional

	/// <summary>
	/// Gets all cards.
	/// </summary>
	/// <returns>The all cards.</returns>
	public List < CardBehaviour > GetAllCards()
	{
		return ItemProperties;
	}

	/// <summary>
	/// Clears the cards.
	/// </summary>
	public void ClearCards()
	{
		for (int i = 0; i < ItemProperties.Count; i++) {
			if (ItemProperties [i] != null) {
				Destroy (ItemProperties [i].gameObject);
			}
		}

		ItemProperties.Clear ();
	}

	/// <summary>
	/// Clear this instance.
	/// </summary>
    public void Clear()
    {
        ItemProperties.Clear();
    }

	/// <summary>
	/// Returns to pool.
	/// </summary>
	/// <param name="param">Parameter.</param>
	public void ReturnToPool(CardBehaviour param)
	{
		param.gameObject.SetActive (false);

		if ( ItemProperties.Contains ( param ))
		{
			LogGame.DebugLog (string.Format ( "[Pool System] Card Was Found {0}" , param.name));

            ItemProperties.Remove(param);
        }

		param.transform.SetParent (transform);

		ItemProperties.Add (param);
	}

	/// <summary>
	/// Gets the number of cards.
	/// </summary>
	public int GetNumberOfCards()
	{
        for (int i = 0; i < ItemProperties.Count; i++)
        {
            if (ItemProperties[i] == null)
            {
                ItemProperties.RemoveAt(i);
            }
        }

        return ItemProperties.Count;
	}

	#endregion
}
