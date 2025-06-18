using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDataProperties : ScriptableObject {

	// ============================ Variables ======================= //

	#region Variables 

	/// <summary>
	/// The type of the card.
	/// </summary>
	[SerializeField]
	private Enums.CardType cardType;

	[SerializeField]
	private Enums.CardVariables cardValue;

	[SerializeField]
	private Sprite CardSprite;

	#endregion

	// ======================== Properties ====================== //

	#region Properties

	/// <summary>
	/// Gets the type of the card.
	/// </summary>
	/// <returns>The card type.</returns>
	public Enums.CardType GetCardType()
	{
		return cardType;
	}

	/// <summary>
	/// Gets the card value.
	/// </summary>
	/// <returns>The card value.</returns>
	public int GetCardValue()
	{
		return (int)cardValue;
	}

	/// <summary>
	/// Gets the card value.
	/// </summary>
	/// <returns>The card value.</returns>
	public Enums.CardVariables GetEnumCardValue()
	{
		return cardValue;
	}

	/// <summary>
	/// Gets the card sprite.
	/// </summary>
	/// <returns>The card sprite.</returns>
	public Sprite GetCardSprite()
	{
		return CardSprite;
	}
	#endregion
}
