using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultCardsManager : SingletonMonoBehaviour < ResultCardsManager > {

	[System.Serializable]
	public struct TransformContents
	{
		/// <summary>
		/// The transform cards 1.
		/// </summary>
		public RectTransform TransformCards_1;

		/// <summary>
		/// The transform cards 2.
		/// </summary>
		public RectTransform TransformCards_2;

		/// <summary>
		/// The transform cards 3.
		/// </summary>
		public RectTransform TransformCards_3;

		/// <summary>
		/// The transform cards 4.
		/// </summary>
		public RectTransform TransformCards_4;
	}

	// ========================== References ======================= //

	#region References

	/// <summary>
	/// The holder result 1.
	/// </summary>
	private RectTransform TransformCards_1;

	/// <summary>
	/// The holder result 2.
	/// </summary>
	private RectTransform TransformCards_2;

	/// <summary>
	/// The holder result 3.
	/// </summary>
	private RectTransform TransformCards_3;

	/// <summary>
	/// The holder result 4.
	/// </summary>
	private RectTransform TransformCards_4;


	[Header ("Portrait")]

	/// <summary>
	/// The portrait transform on the left.
	/// </summary>
	public TransformContents PortraitTransformOnLeft;

	/// <summary>
	/// The portrait transform on the right.
	/// </summary>
	public TransformContents PortraitTransformOnRight;

	[Header ("Landscape")]

	/// <summary>
	/// The landscape transform on the left.
	/// </summary>
	public TransformContents LandscapeTransformOnLeft;

	/// <summary>
	/// The landscape transform on the right.
	/// </summary>
	public TransformContents LandscapeTransformOnRight;


	#endregion


	// ========================= Variables ========================= //

	#region Variables

	/// <summary>
	/// The cards on holder.
	/// </summary>
	protected List < CardBehaviour > holdTheCardsOne = new List<CardBehaviour>();

	/// <summary>
	/// The cards on holder I.
	/// </summary>
	protected List < CardBehaviour > holdTheCardsTwo = new List<CardBehaviour> ();

	/// <summary>
	/// The cards on holder II.
	/// </summary>
	protected List < CardBehaviour > holdTheCardsThree = new List<CardBehaviour> ();

	/// <summary>
	/// The cards on holder I.
	/// </summary>
	protected List < CardBehaviour > holdTheCardsFour = new List<CardBehaviour> ();

	#endregion

	// ========================= Cache ============================ //

	#region Cache
	/// <summary>
	/// The transform.
	/// </summary>
	protected new Transform transform;
	#endregion


	// ========================= Functional ======================== //
	#region Functional

	/// <summary>
	/// Awake this instance.
	/// </summary>
    protected override void Awake()
    {
        base.Awake();

        InitCache();
    }

    /// <summary>
    /// Inits the cache.
    /// </summary>
    void InitCache()
	{
		transform = gameObject.transform;
	}

    public bool IsExistsThisCard(CardBehaviour card, Enums.ResultManager holder)
    {
        bool valueReturn = false;

        switch (holder)
        {
            case Enums.ResultManager.holderOne:

                valueReturn = holdTheCardsOne.Contains(card);

                break;
            case Enums.ResultManager.holderTwo:

                valueReturn = holdTheCardsTwo.Contains(card);
                break;
            case Enums.ResultManager.holderThree:

                valueReturn = holdTheCardsThree.Contains(card);

                break;
            case Enums.ResultManager.holderFour:

                valueReturn = holdTheCardsFour.Contains(card);
                break;
        }

        return valueReturn;
    }


    /// <summary>
    /// Get the postion - where card can move to.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetLastPositionInHolder(Enums.ResultManager holder)
    {
        Vector3 position = Vector3.zero;

        switch (holder)
        {
            case Enums.ResultManager.holderOne:

                position = Helper.GetPositionInTheHolderCards(holdTheCardsOne, Enums.Direction.None);

                break;
            case Enums.ResultManager.holderTwo:

                position = Helper.GetPositionInTheHolderCards(holdTheCardsTwo, Enums.Direction.None);

                break;
            case Enums.ResultManager.holderThree:

                position = Helper.GetPositionInTheHolderCards(holdTheCardsThree, Enums.Direction.None);

                break;
            case Enums.ResultManager.holderFour:

                position = Helper.GetPositionInTheHolderCards(holdTheCardsFour, Enums.Direction.None);

                break; 
        }

        if (position == Vector3.zero)
        {

            switch (holder)
            {
                case Enums.ResultManager.holderOne:

                    position = TransformCards_1.position;

                    break;
                case Enums.ResultManager.holderTwo:

                    position = TransformCards_2.position;

                    break;
                case Enums.ResultManager.holderThree:

                    position = TransformCards_3.position;

                    break;
                case Enums.ResultManager.holderFour:

                    position = TransformCards_4.position;

                    break;
            }
        }
        return position;
    }

	/// <summary>
	/// Sorts the cards.
	/// </summary>
	public void SortCards()
	{
		// Sort the cards from the Transform of Cards.
		Helper.SortCards (holdTheCardsOne, TransformCards_1, 0 , Enums.Direction.None , 0 );

		// Sort the cards from the Transform of Cards.
		Helper.SortCards (holdTheCardsTwo, TransformCards_2, 0 , Enums.Direction.None , 0 );

		// Sort the cards from the Transform of Cards.
		Helper.SortCards (holdTheCardsThree, TransformCards_3, 0 , Enums.Direction.None , 0 );

		// Sort the cards from the Transform of Cards.
		Helper.SortCards (holdTheCardsFour, TransformCards_4, 0 , Enums.Direction.None , 0 );
	}

	/// <summary>
	/// Updates the portrait on left.
	/// </summary>
	public void UpdatePortraitOnLeft()
	{
		// Update the transform of cards.
		TransformCards_1 = PortraitTransformOnLeft.TransformCards_1;

		// Update the transform of cards.
		TransformCards_2 = PortraitTransformOnLeft.TransformCards_2;

		// Update the transform of cards.
		TransformCards_3 = PortraitTransformOnLeft.TransformCards_3;

		// Update the transform of cards.
		TransformCards_4 = PortraitTransformOnLeft.TransformCards_4;
	}

	/// <summary>
	/// Updates the portrait on right.
	/// </summary>
	public void UpdatePortraitOnRight()
	{
		// Update the transform of cards.
		TransformCards_1 = PortraitTransformOnRight.TransformCards_1;

		// Update the transform of cards.
		TransformCards_2 = PortraitTransformOnRight.TransformCards_2;

		// Update the transform of cards.
		TransformCards_3 = PortraitTransformOnRight.TransformCards_3;

		// Update the transform of cards.
		TransformCards_4 = PortraitTransformOnRight.TransformCards_4;
	}

	/// <summary>
	/// Updates the landscape on left.
	/// </summary>
	public void UpdateLandscapeOnLeft()
	{
		// Update the transform of cards.
		TransformCards_1 = LandscapeTransformOnLeft.TransformCards_1;

		// Update the transform of cards.
		TransformCards_2 = LandscapeTransformOnLeft.TransformCards_2;

		// Update the transform of cards.
		TransformCards_3 = LandscapeTransformOnLeft.TransformCards_3;

		// Update the transform of cards.
		TransformCards_4 = LandscapeTransformOnLeft.TransformCards_4;
	}

	/// <summary>
	/// Updates the landscape on right.
	/// </summary>
	public void UpdateLandscapeOnRight(){
		// Update the transform of cards.
		TransformCards_1 = LandscapeTransformOnRight.TransformCards_1;

		// Update the transform of cards.
		TransformCards_2 = LandscapeTransformOnRight.TransformCards_2;

		// Update the transform of cards.
		TransformCards_3 = LandscapeTransformOnRight.TransformCards_3;

		// Update the transform of cards.
		TransformCards_4 = LandscapeTransformOnRight.TransformCards_4;
	}

	/// <summary>
	/// Gets the last card.
	/// </summary>
	/// <returns>The last card.</returns>
	/// <param name="holder">Holder.</param>
    public CardBehaviour GetLastCard(Enums.ResultManager holder)
    {
        CardBehaviour valueReturn = null;

        switch (holder)
        {
            case Enums.ResultManager.holderOne:

                if (holdTheCardsOne.Count > 0 && holdTheCardsOne[holdTheCardsOne.Count - 1].IsUnlocked())
                {
                    valueReturn = holdTheCardsOne[holdTheCardsOne.Count - 1];
                }

                break;
            case Enums.ResultManager.holderTwo:

                if (holdTheCardsTwo.Count > 0 && holdTheCardsTwo[holdTheCardsTwo.Count - 1].IsUnlocked())
                {
                    valueReturn = holdTheCardsTwo[holdTheCardsTwo.Count - 1];
                }

                break;
            case Enums.ResultManager.holderThree:

                if (holdTheCardsThree.Count > 0 && holdTheCardsThree[holdTheCardsThree.Count - 1].IsUnlocked())
                {
                    valueReturn = holdTheCardsThree[holdTheCardsThree.Count - 1];
                }

                break;
            case Enums.ResultManager.holderFour:

                if (holdTheCardsFour.Count > 0 && holdTheCardsFour[holdTheCardsFour.Count - 1].IsUnlocked())
                {
                    valueReturn = holdTheCardsFour[holdTheCardsFour.Count - 1];
                }

                break;
        }

        return valueReturn;
    }

    public Transform GetHolderCards(Enums.ResultManager holder)
    {
        Transform holderReturn = null;

        switch (holder)
        {
            case Enums.ResultManager.holderOne:

                holderReturn = TransformCards_1;

                break;
            case Enums.ResultManager.holderTwo:

                holderReturn = TransformCards_2;
                break;
            case Enums.ResultManager.holderThree:

                holderReturn = TransformCards_3;
                break;
            case Enums.ResultManager.holderFour:

                holderReturn = TransformCards_4;
                break;
        }

        return holderReturn;
    }

    public void UpdateNewCardToHolder(CardBehaviour card, Enums.ResultManager holder)
    {
        switch (holder)
        {
            case Enums.ResultManager.holderOne:

                holdTheCardsOne.Add(card);

                break;
            case Enums.ResultManager.holderTwo:

                holdTheCardsTwo.Add(card);

                break;
            case Enums.ResultManager.holderThree:

                holdTheCardsThree.Add(card);

                break;
            case Enums.ResultManager.holderFour:

                holdTheCardsFour.Add(card);

                break;
        }
    }

    public Enums.ResultManager ReturnEnumHolder(CardBehaviour param)
    {
        if (holdTheCardsOne.Contains(param))
        {
            return Enums.ResultManager.holderOne;
        }

        if (holdTheCardsTwo.Contains(param))
        {
            return Enums.ResultManager.holderTwo;
        }

        if (holdTheCardsThree.Contains(param))
        {
            return Enums.ResultManager.holderThree;
        }

        if (holdTheCardsFour.Contains(param))
        {
            return Enums.ResultManager.holderFour;
        }

        return Enums.ResultManager.None;
    }

    public void OutOfHolder(CardBehaviour card)
    {
        if (holdTheCardsOne.Contains(card))
        {
            holdTheCardsOne.Remove(card);

            return;
        }

        if (holdTheCardsTwo.Contains(card))
        {
            holdTheCardsTwo.Remove(card);

            return;
        }

        if (holdTheCardsThree.Contains(card))
        {
            holdTheCardsThree.Remove(card);

            return;
        }

        if (holdTheCardsFour.Contains(card))
        {
            holdTheCardsFour.Remove(card);

            return;
        }
    }
    #endregion

    #region helper
    public bool IsHaveSameTypeCard(Enums.ResultManager holder, CardBehaviour param)
    {

        bool valueReturn = false;

        switch (holder)
        {
            case Enums.ResultManager.holderOne:

                if (holdTheCardsOne.Count > 0 && param.GetDataCard().GetCardType() == holdTheCardsOne[holdTheCardsOne.Count - 1].GetDataCard().GetCardType())
                {
                    valueReturn = true;
                }

                break;
            case Enums.ResultManager.holderTwo:

                if (holdTheCardsTwo.Count > 0 && param.GetDataCard().GetCardType() == holdTheCardsTwo[holdTheCardsTwo.Count - 1].GetDataCard().GetCardType())
                {
                    valueReturn = true;
                }

                break;
            case Enums.ResultManager.holderThree:

                if (holdTheCardsThree.Count > 0 && param.GetDataCard().GetCardType() == holdTheCardsThree[holdTheCardsThree.Count - 1].GetDataCard().GetCardType())
                {
                    valueReturn = true;
                }

                break;
            case Enums.ResultManager.holderFour:

                if (holdTheCardsFour.Count > 0 && param.GetDataCard().GetCardType() == holdTheCardsFour[holdTheCardsFour.Count - 1].GetDataCard().GetCardType())
                {
                    valueReturn = true;
                }

                break;
        }
        return valueReturn;

    }

/// <summary>
/// Gets the card result. use For hint
/// </summary>
/// <returns>The card result.</returns>
/// <param name="except">Except.</param>
/// <param name="cardCompare">Card compare.</param>
public CardBehaviour GetCardResult(Enums.ResultManager except , CardBehaviour cardCompare)
	{
		CardBehaviour cardResult = null;

		bool IsFoundTheCard = false;

		for (int i = 0; i < 4; i++) {

			if (IsFoundTheCard)
				break;

			if ((int)except != i) {
				switch (i) {
				case 0:

					cardResult = GetLastCard (Enums.ResultManager.holderOne);

					if (cardResult != null && cardCompare.GetProperties ().IsBiggerThan (cardResult.GetDataCard ()) && cardCompare.GetDataCard().GetCardType() == cardResult.GetDataCard ().GetCardType()) {
						IsFoundTheCard = true;
					}

					break;
				case 1:

					cardResult = GetLastCard (Enums.ResultManager.holderTwo);

					if (cardResult != null && cardCompare.GetProperties ().IsBiggerThan (cardResult.GetDataCard ()) && cardCompare.GetDataCard().GetCardType() == cardResult.GetDataCard ().GetCardType()) {
						IsFoundTheCard = true;
					}

					break;
				case 2:

					cardResult = GetLastCard (Enums.ResultManager.holderThree);

					if (cardResult != null && cardCompare.GetProperties ().IsBiggerThan (cardResult.GetDataCard ()) && cardCompare.GetDataCard().GetCardType() == cardResult.GetDataCard ().GetCardType()) {
						IsFoundTheCard = true;
					}

					break;
				case 3:

					cardResult = GetLastCard (Enums.ResultManager.holderFour);

					if (cardResult != null && cardCompare.GetProperties ().IsBiggerThan (cardResult.GetDataCard ()) && cardCompare.GetDataCard().GetCardType() == cardResult.GetDataCard ().GetCardType()) {
						IsFoundTheCard = true;
					}

					break;
				}
			}

			if (IsFoundTheCard == false)
				cardResult = null;
		
		}

		return cardResult;
	
	}

	#endregion

}
