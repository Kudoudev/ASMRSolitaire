using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CardStyle
{
    [Header ("Cards")]

	/// <summary>
	/// The identifier of cards.
	/// </summary>
    public int Id;

	/// <summary>
	/// The cards data.
	/// </summary>
    public CardDataProperties[] cardsData;

    /// <summary>
    /// The default cards.
    /// </summary>
    public Sprite DefaultCards;
}

public class DataSystem : SingletonMonoBehaviour < DataSystem > {

    // =========================== References ==================== //

    [Header("Database")]
	/// <summary>
	/// The card style.
	/// </summary>
	[SerializeField]  private CardStyle[] cardStyle;


    [Header ("Prefabs")]
	/// <summary>
	/// The card prefabs.
	/// </summary>
	[SerializeField]  private CardBehaviour cardPrefabs;

	// =========================== Helper ======================= //

	#region Helper

	/// <summary>
	/// Gets the cards data.
	/// </summary>
	/// <returns>The cards data.</returns>
	public CardDataProperties[] GetCardsData(int id = 0)
	{
        for ( int  i = 0; i < cardStyle.Length; i++)
        {
            if (cardStyle[i].Id == id)
            {
                return cardStyle[i].cardsData;
            }
        }

		return null;
	}

	/// <summary>
	/// Gets the card prefab.
	/// </summary>
	/// <returns>The card prefab.</returns>
	public CardBehaviour GetCardPrefab()
	{
		return cardPrefabs;
	}

	/// <summary>
	/// Gets the default card.
	/// </summary>
	/// <returns>The default card.</returns>
	public Sprite GetDefaultCard(int id = 0)
	{
        for (int i = 0; i < cardStyle.Length; i++)
        {
            if (cardStyle[i].Id == id)
            {
                return cardStyle[i].DefaultCards;
            }
        }

        return null;
	}

	#endregion
}
