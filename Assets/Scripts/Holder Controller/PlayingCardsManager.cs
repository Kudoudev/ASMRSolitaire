using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCardsManager : SingletonMonoBehaviour < PlayingCardsManager > {

	[System.Serializable]
	public struct TransformContent
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

		/// <summary>
		/// The transform cards 5.
		/// </summary>
		public RectTransform TransformCards_5;

		/// <summary>
		/// The transform cards 6.
		/// </summary>
		public RectTransform TransformCards_6;

		/// <summary>
		/// The transform cards 7.
		/// </summary>
		public RectTransform TransformCards_7;
	}

	// ========================== References ======================= //

	#region References

	/// <summary>
	/// The holder cards 1.
	/// </summary>
	private RectTransform TransformCards_1;

	/// <summary>
	/// The holder cards 2.
	/// </summary>
	private RectTransform TransformCards_2;

	/// <summary>
	/// The holder cards 3.
	/// </summary>
	private RectTransform TransformCards_3;

	/// <summary>
	/// The holder cards 4.
	/// </summary>
	private RectTransform TransformCards_4;

	/// <summary>
	/// The holder cards 5.
	/// </summary>
	private RectTransform TransformCards_5;

	/// <summary>
	/// The holder cards 6.
	/// </summary>
	private RectTransform TransformCards_6;

	/// <summary>
	/// The holder cards 7.
	/// </summary>
	private RectTransform TransformCards_7;
	#endregion


	// ========================= Variables ========================= //

	#region Variables

	/// <summary>
	/// The cards on holder.
	/// </summary>
	protected List < CardBehaviour > ArrayOfCards_A = new List<CardBehaviour>();

	/// <summary>
	/// The cards on holder I.
	/// </summary>
	protected List < CardBehaviour > ArrayOfCards_B = new List<CardBehaviour> ();

	/// <summary>
	/// The cards on holder II.
	/// </summary>
	protected List < CardBehaviour > ArrayOfCards_C = new List<CardBehaviour> ();

	/// <summary>
	/// The cards on holder I.
	/// </summary>
	protected List < CardBehaviour > ArrayOfCards_D = new List<CardBehaviour> ();

	/// <summary>
	/// The cards on holder I.
	/// </summary>
	protected List < CardBehaviour > ArrayOfCards_E = new List<CardBehaviour> ();

	/// <summary>
	/// The cards on holder II.
	/// </summary>
	protected List < CardBehaviour > ArrayOfCards_G = new List<CardBehaviour> ();

	/// <summary>
	/// The cards on holder I.
	/// </summary>
	protected List < CardBehaviour > ArrayOfCards_H  = new List<CardBehaviour> ();
    #endregion

	[Header ("Portrait")]

	/// <summary>
	/// The content on left.
	/// </summary>
	public TransformContent PortraitContentOnLeft;

	/// <summary>
	/// The content on right.
	/// </summary>
	public TransformContent PortraitContentOnRight;

	[Header ("Landscape")]

	/// <summary>
	/// The landscape content on left.
	/// </summary>
	public TransformContent LandscapeContentOnLeft;

	/// <summary>
	/// The landscape content on right.
	/// </summary>
	public TransformContent LandscapeContentOnRight;

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
		base.Awake ();

		// Update the caches.
		InitCache ();
	}

	/// <summary>
	/// Inits the cache.
	/// </summary>
	void InitCache()
	{
		// Update the cache for new transform.
		transform = gameObject.transform;
	}

	/// <summary>
	/// Sorts the cards.
	/// </summary>
	public void SortCards(){
	
		// Sort the cards on Array of Cards.
		Helper.SortUnlockedCards (ArrayOfCards_A, TransformCards_1);
	
		// Sort the cards on Array of Cards.
		Helper.SortUnlockedCards (ArrayOfCards_B, TransformCards_2);

		// Sort the cards on Array of Cards.
		Helper.SortUnlockedCards (ArrayOfCards_C, TransformCards_3);

		// Sort the cards on Array of Cards.
		Helper.SortUnlockedCards (ArrayOfCards_D, TransformCards_4);

		// Sort the cards on Array of Cards.
		Helper.SortUnlockedCards (ArrayOfCards_E, TransformCards_5);

		// Sort the cards on Array of Cards.
		Helper.SortUnlockedCards (ArrayOfCards_G, TransformCards_6);

		// Sort the cards on Array of Cards.
		Helper.SortUnlockedCards (ArrayOfCards_H, TransformCards_7);
	}


	/// <summary>
	/// Updates the portrait view.
	/// </summary>
	public void UpdatePortraitLeft()
	{
		// Add Transform Portrait for current holder.
		TransformCards_1 = PortraitContentOnLeft.TransformCards_1;

		// Add Transform Portrait for current holder.
		TransformCards_2 = PortraitContentOnLeft.TransformCards_2;

		// Add Transform Portrait for current holder.
		TransformCards_3 = PortraitContentOnLeft.TransformCards_3;

		// Add Transform Portrait for current holder.
		TransformCards_4 = PortraitContentOnLeft.TransformCards_4;

		// Add Transform Portrait for current holder.
		TransformCards_5 = PortraitContentOnLeft.TransformCards_5;

		// Add Transform Portrait for current holder.
		TransformCards_6 = PortraitContentOnLeft.TransformCards_6;

		// Add Transform Portrait for current holder.
		TransformCards_7 = PortraitContentOnLeft.TransformCards_7;
	}

	/// <summary>
	/// Updates the portrait right.
	/// </summary>
	public void UpdatePortraitRight()
	{
		// Add Transform Portrait for current holder.
		TransformCards_1 = PortraitContentOnRight.TransformCards_1;

		// Add Transform Portrait for current holder.
		TransformCards_2 = PortraitContentOnRight.TransformCards_2;

		// Add Transform Portrait for current holder.
		TransformCards_3 = PortraitContentOnRight.TransformCards_3;

		// Add Transform Portrait for current holder.
		TransformCards_4 = PortraitContentOnRight.TransformCards_4;

		// Add Transform Portrait for current holder.
		TransformCards_5 = PortraitContentOnRight.TransformCards_5;

		// Add Transform Portrait for current holder.
		TransformCards_6 = PortraitContentOnRight.TransformCards_6;

		// Add Transform Portrait for current holder.
		TransformCards_7 = PortraitContentOnRight.TransformCards_7;
	}

	/// <summary>
	/// Updates the landscape left.
	/// </summary>
	public void UpdateLandscapeLeft()
	{
		// Add Transform Landscape for current holder.
		TransformCards_1 = LandscapeContentOnLeft.TransformCards_1;

		// Add Transform Landscape for current holder.
		TransformCards_2 = LandscapeContentOnLeft.TransformCards_2;

		// Add Transform Landscape for current holder.
		TransformCards_3 = LandscapeContentOnLeft.TransformCards_3;

		// Add Transform Landscape for current holder.
		TransformCards_4 = LandscapeContentOnLeft.TransformCards_4;

		// Add Transform Landscape for current holder.
		TransformCards_5 = LandscapeContentOnLeft.TransformCards_5;

		// Add Transform Landscape for current holder.
		TransformCards_6 = LandscapeContentOnLeft.TransformCards_6;

		// Add Transform Landscape for current holder.
		TransformCards_7 = LandscapeContentOnLeft.TransformCards_7;
	}

	/// <summary>
	/// Updates the landscape right.
	/// </summary>
	public void UpdateLandscapeRight()
	{
		// Add Transform Landscape for current holder.
		TransformCards_1 = LandscapeContentOnRight.TransformCards_1;

		// Add Transform Landscape for current holder.
		TransformCards_2 = LandscapeContentOnRight.TransformCards_2;

		// Add Transform Landscape for current holder.
		TransformCards_3 = LandscapeContentOnRight.TransformCards_3;

		// Add Transform Landscape for current holder.
		TransformCards_4 = LandscapeContentOnRight.TransformCards_4;

		// Add Transform Landscape for current holder.
		TransformCards_5 = LandscapeContentOnRight.TransformCards_5;

		// Add Transform Landscape for current holder.
		TransformCards_6 = LandscapeContentOnRight.TransformCards_6;

		// Add Transform Landscape for current holder.
		TransformCards_7 = LandscapeContentOnRight.TransformCards_7;
	}

	/// <summary>
	/// Unlocks the last cards.
	/// </summary>
    public void UnlockLastCards()
	{
		// Create the new card cache.
		CardBehaviour card = null;

		// Check the condition to unlock this cards.
		if (ArrayOfCards_A.Count > 0 && ArrayOfCards_A [ArrayOfCards_A.Count - 1].IsUnlocked () == false) {
			card = ArrayOfCards_A [ArrayOfCards_A.Count - 1];

			// record the state of cards to using in the Undo System.
			UndoSystem.Instance.RecordState (card, card.targetPositionCards, false, Enums.CardPlayingEnums.None, Enums.ResultManager.None, card.IsUnlocked (), false, Contains.Score, true);

			// Unlock this card.
			card.UnlockCard (true);

			// Update new score when unlocked the cards.
			Helper.UpdateScore (Contains.ScoreMoveCards);

			// Update the score on UI.
			HudSystem.Instance.UpdateScore (Contains.Score);
		}

		// Check the condition to unlock this cards.
		if (ArrayOfCards_B.Count > 0 && ArrayOfCards_B [ArrayOfCards_B.Count - 1].IsUnlocked () == false) {
			card = ArrayOfCards_B [ArrayOfCards_B.Count - 1];

			// record the state of cards to using in the Undo System.
			UndoSystem.Instance.RecordState (card, card.targetPositionCards, false, Enums.CardPlayingEnums.None, Enums.ResultManager.None, card.IsUnlocked (), false, Contains.Score, true);

			// Unlock this card.
			card.UnlockCard (true);

			// Update new score when unlocked the cards.
			Helper.UpdateScore (Contains.ScoreMoveCards);

			// Update the score on UI.
			HudSystem.Instance.UpdateScore (Contains.Score);
		}

		// Check the condition to unlock this cards.
		if (ArrayOfCards_C.Count > 0 && ArrayOfCards_C [ArrayOfCards_C.Count - 1].IsUnlocked () == false) {
			card = ArrayOfCards_C [ArrayOfCards_C.Count - 1];

			// record the state of cards to using in the Undo System.
			UndoSystem.Instance.RecordState (card, card.targetPositionCards, false, Enums.CardPlayingEnums.None, Enums.ResultManager.None, card.IsUnlocked (), false, Contains.Score, true);

			// Unlock this card.
			card.UnlockCard (true);

			// Update new score when unlocked the cards.
			Helper.UpdateScore (Contains.ScoreMoveCards);

			// Update the score on UI.
			HudSystem.Instance.UpdateScore (Contains.Score);
		}

		// Check the condition to unlock this cards.
		if (ArrayOfCards_D.Count > 0 && ArrayOfCards_D [ArrayOfCards_D.Count - 1].IsUnlocked () == false) {
			card = ArrayOfCards_D [ArrayOfCards_D.Count - 1];

			// record the state of cards to using in the Undo System.
			UndoSystem.Instance.RecordState (card, card.targetPositionCards, false, Enums.CardPlayingEnums.None, Enums.ResultManager.None, card.IsUnlocked (), false, Contains.Score, true);

			// Unlock this card.
			card.UnlockCard (true);

			// Update new score when unlocked the cards.
			Helper.UpdateScore (Contains.ScoreMoveCards);

			// Update the score on UI.
			HudSystem.Instance.UpdateScore (Contains.Score);
		}

		// Check the condition to unlock this cards.
		if (ArrayOfCards_E.Count > 0 && ArrayOfCards_E [ArrayOfCards_E.Count - 1].IsUnlocked () == false) {
			card = ArrayOfCards_E [ArrayOfCards_E.Count - 1];

			// record the state of cards to using in the Undo System.
			UndoSystem.Instance.RecordState (card, card.targetPositionCards, false, Enums.CardPlayingEnums.None, Enums.ResultManager.None, card.IsUnlocked (), false, Contains.Score, true);

			// Unlock this card.
			card.UnlockCard (true);

			// Update new score when unlocked the cards.
			Helper.UpdateScore (Contains.ScoreMoveCards);

			// Update the score on UI.
			HudSystem.Instance.UpdateScore (Contains.Score);
		}

		// Check the condition to unlock this cards.
		if (ArrayOfCards_G.Count > 0 && ArrayOfCards_G [ArrayOfCards_G.Count - 1].IsUnlocked () == false) {
			card = ArrayOfCards_G [ArrayOfCards_G.Count - 1];

			// record the state of cards to using in the Undo System.
			UndoSystem.Instance.RecordState (card, card.targetPositionCards, false, Enums.CardPlayingEnums.None, Enums.ResultManager.None, card.IsUnlocked (), false, Contains.Score, true);

			// Unlock this card.
			card.UnlockCard (true);

			// Update new score when unlocked the cards.
			Helper.UpdateScore (Contains.ScoreMoveCards);

			// Update the score on UI.
			HudSystem.Instance.UpdateScore (Contains.Score);
		}

		// Check the condition to unlock this cards.
		if (ArrayOfCards_H.Count > 0 && ArrayOfCards_H [ArrayOfCards_H.Count - 1].IsUnlocked () == false) {
			card = ArrayOfCards_H [ArrayOfCards_H.Count - 1];

			// record the state of cards to using in the Undo System.
			UndoSystem.Instance.RecordState (card, card.targetPositionCards, false, Enums.CardPlayingEnums.None, Enums.ResultManager.None, card.IsUnlocked (), false, Contains.Score, true);

			// Unlock this card.
			card.UnlockCard (true);

			// Update new score when unlocked the cards.
			Helper.UpdateScore (Contains.ScoreMoveCards);

			// Update the score on UI.
			HudSystem.Instance.UpdateScore (Contains.Score);
		}
	}

	/// <summary>
	/// Outs the of holder.
	/// </summary>
	/// <param name="card">Card.</param>
    public void RemoveThisCard(CardBehaviour card)
    {
		// Check if this card is exists in the array of cards.
        if (ArrayOfCards_A.Contains(card))
        {

			// Remove this card in the array of cards.
            ArrayOfCards_A.Remove(card);

            return;
        }

		// Check if this card is exists in the array of cards.
        if (ArrayOfCards_B.Contains(card))
        {

			// Remove this card in the array of cards.
            ArrayOfCards_B.Remove(card);

            return;
        }

		// Check if this card is exists in the array of cards.
        if (ArrayOfCards_C.Contains(card))
        {

			// Remove this card in the array of cards.
            ArrayOfCards_C.Remove(card);

            return;
        }

		// Check if this card is exists in the array of cards.
        if (ArrayOfCards_D.Contains(card))
        {

			// Remove this card in the array of cards.
            ArrayOfCards_D.Remove(card);

            return;
        }

		// Check if this card is exists in the array of cards.
        if (ArrayOfCards_E.Contains(card))
        {

			// Remove this card in the array of cards.
            ArrayOfCards_E.Remove(card);

            return;
        }

		// Check if this card is exists in the array of cards.
        if (ArrayOfCards_G.Contains(card))
        {

			// Remove this card in the array of cards.
            ArrayOfCards_G.Remove(card);

            return;
        }

		// Check if this card is exists in the array of cards.
        if (ArrayOfCards_H.Contains(card))
        {

			// Remove this card in the array of cards.
            ArrayOfCards_H.Remove(card);

            return;
        }
    }

	/// <summary>
	/// Determines whether this instance is exists this card the specified card holder.
	/// </summary>
	/// <returns><c>true</c> if this instance is exists this card the specified card holder; otherwise, <c>false</c>.</returns>
	/// <param name="card">Card.</param>
	/// <param name="holder">Holder.</param>
    public bool IsExistsThisCard(CardBehaviour card , Enums.CardPlayingEnums holder)
	{
		// Create the value need to return.
		bool valueReturn = false;

		// Checking the holder.
		switch (holder) {
		case Enums.CardPlayingEnums.holderOne:

				// If this exists in the array of cards. return true else return false.
			valueReturn = ArrayOfCards_A.Contains (card);

			break;
		case Enums.CardPlayingEnums.holderTwo:

			// If this exists in the array of cards. return true else return false.
			valueReturn = ArrayOfCards_B.Contains (card);
			break;
		case Enums.CardPlayingEnums.holderThree:

			// If this exists in the array of cards. return true else return false.
			valueReturn = ArrayOfCards_C.Contains (card);

			break;
		case Enums.CardPlayingEnums.holderFour:

			// If this exists in the array of cards. return true else return false.
			valueReturn = ArrayOfCards_D.Contains (card);
			break;
		case Enums.CardPlayingEnums.holderFive:

			// If this exists in the array of cards. return true else return false.
			valueReturn = ArrayOfCards_E.Contains (card);
			break;
		case Enums.CardPlayingEnums.holderSix:

			// If this exists in the array of cards. return true else return false.
			valueReturn = ArrayOfCards_G.Contains (card);
			break;
		case Enums.CardPlayingEnums.holderSeven:

			// If this exists in the array of cards. return true else return false.
			valueReturn = ArrayOfCards_H.Contains (card);
			break;
		}

		// return the value of condition.
		return valueReturn;
	}

	/// <summary>
	/// Gets the holder cards.
	/// </summary>
	/// <returns>The holder cards.</returns>
	/// <param name="holder">Holder.</param>
    public Transform GetParrentOfCards(Enums.CardPlayingEnums holder)
	{
		// Creating the Transform of new holder.
		Transform holderReturn = null;

		switch (holder) {
		case Enums.CardPlayingEnums.holderOne:

				// Get transform from the holder of cards.
			holderReturn = TransformCards_1;
                
			break;
		case Enums.CardPlayingEnums.holderTwo:

			// Get transform from the holder of cards.
			holderReturn = TransformCards_2;
			break;
		case Enums.CardPlayingEnums.holderThree:

			// Get transform from the holder of cards.
			holderReturn = TransformCards_3;
			break;
		case Enums.CardPlayingEnums.holderFour:

			// Get transform from the holder of cards.
			holderReturn = TransformCards_4;
			break;
		case Enums.CardPlayingEnums.holderFive:

			// Get transform from the holder of cards.
			holderReturn = TransformCards_5;
			break;
		case Enums.CardPlayingEnums.holderSix:

			// Get transform from the holder of cards.
			holderReturn = TransformCards_6;
			break;
		case Enums.CardPlayingEnums.holderSeven:

			// Get transform from the holder of cards.
			holderReturn = TransformCards_7;
			break;
		}

		// return the current of holder after getting.
		return holderReturn;
	}

	/// <summary>
	/// Returns the enum holder.
	/// </summary>
	/// <returns>The enum holder.</returns>
	/// <param name="param">Parameter.</param>
    public Enums.CardPlayingEnums ReturnEnumHolder(CardBehaviour param)
    {
        if ( ArrayOfCards_A.Contains (param))
        {
            return Enums.CardPlayingEnums.holderOne;
        }

        if (ArrayOfCards_B.Contains(param))
        {
            return Enums.CardPlayingEnums.holderTwo;
        }

        if (ArrayOfCards_C.Contains(param))
        {
            return Enums.CardPlayingEnums.holderThree;
        }

        if (ArrayOfCards_D.Contains(param))
        {
            return Enums.CardPlayingEnums.holderFour;
        }

        if (ArrayOfCards_E.Contains(param))
        {
            return Enums.CardPlayingEnums.holderFive;
        }

        if (ArrayOfCards_G.Contains(param))
        {
            return Enums.CardPlayingEnums.holderSix;
        }

        if (ArrayOfCards_H.Contains(param))
        {
            return Enums.CardPlayingEnums.holderSeven;
        }

        return Enums.CardPlayingEnums.None;
    }

	/// <summary>
	/// Updates the new card to holder.
	/// </summary>
	/// <param name="card">Card.</param>
	/// <param name="holder">Holder.</param>
    public void UpdateNewCardToHolder(CardBehaviour card ,Enums.CardPlayingEnums holder)
    {
        switch (holder)
        {
            case Enums.CardPlayingEnums.holderOne:

                ArrayOfCards_A.Add(card);

                break;
            case Enums.CardPlayingEnums.holderTwo:

                ArrayOfCards_B.Add(card);

                break;
            case Enums.CardPlayingEnums.holderThree:

                ArrayOfCards_C.Add(card);

                break;
            case Enums.CardPlayingEnums.holderFour:

                ArrayOfCards_D.Add(card);

                break;
            case Enums.CardPlayingEnums.holderFive:

                ArrayOfCards_E.Add(card);

                break;
            case Enums.CardPlayingEnums.holderSix:

                ArrayOfCards_G.Add(card);

                break;
            case Enums.CardPlayingEnums.holderSeven:

                ArrayOfCards_H.Add(card);

                break;
        }
    }

    /// <summary>
    /// Get the postion - where card can move to.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetLastPositionInHolder(Enums.CardPlayingEnums holder)
    {
        Vector3 position = Vector3.zero;

       switch ( holder )
        {
            case Enums.CardPlayingEnums.holderOne:

                position = Helper.GetPositionInTheHolderCards(ArrayOfCards_A, Enums.Direction.Down);

                break;
            case Enums.CardPlayingEnums.holderTwo:

                position = Helper.GetPositionInTheHolderCards(ArrayOfCards_B, Enums.Direction.Down);

                break;
            case Enums.CardPlayingEnums.holderThree:

                position = Helper.GetPositionInTheHolderCards(ArrayOfCards_C, Enums.Direction.Down);

                break;
            case Enums.CardPlayingEnums.holderFour:

                position = Helper.GetPositionInTheHolderCards(ArrayOfCards_D, Enums.Direction.Down);

                break;
            case Enums.CardPlayingEnums.holderFive:

                position = Helper.GetPositionInTheHolderCards(ArrayOfCards_E, Enums.Direction.Down);

                break;
            case Enums.CardPlayingEnums.holderSix:

                position = Helper.GetPositionInTheHolderCards(ArrayOfCards_G, Enums.Direction.Down);

                break;
            case Enums.CardPlayingEnums.holderSeven:

                position = Helper.GetPositionInTheHolderCards(ArrayOfCards_H, Enums.Direction.Down);

                break;
        }

        if (position == Vector3.zero)
        {

            switch (holder)
            {
                case Enums.CardPlayingEnums.holderOne:

                    position = TransformCards_1.position;

                    break;
                case Enums.CardPlayingEnums.holderTwo:

                    position = TransformCards_2.position;

                    break;
                case Enums.CardPlayingEnums.holderThree:

                    position = TransformCards_3.position;

                    break;
                case Enums.CardPlayingEnums.holderFour:

                    position = TransformCards_4.position;

                    break;
                case Enums.CardPlayingEnums.holderFive:

                    position = TransformCards_5.position;

                    break;
                case Enums.CardPlayingEnums.holderSix:

                    position = TransformCards_6.position;

                    break;
                case Enums.CardPlayingEnums.holderSeven:

                    position = TransformCards_7.position;

                    break;
            }
        }
        return position;
    }

	/// <summary>
	/// Gets the last card.
	/// </summary>
	/// <returns>The last card.</returns>
	/// <param name="holder">Holder.</param>
    public CardBehaviour GetLastCard(Enums.CardPlayingEnums holder)
    {
        CardBehaviour valueReturn = null;

        switch (holder)
        {
            case Enums.CardPlayingEnums.holderOne:

                if ( ArrayOfCards_A.Count > 0 && ArrayOfCards_A[ArrayOfCards_A.Count - 1].IsUnlocked())
                {
                    valueReturn = ArrayOfCards_A[ArrayOfCards_A.Count - 1];
                }

                break;
            case Enums.CardPlayingEnums.holderTwo:

                if (ArrayOfCards_B.Count > 0 && ArrayOfCards_B[ArrayOfCards_B.Count - 1].IsUnlocked())
                {
                    valueReturn = ArrayOfCards_B[ArrayOfCards_B.Count - 1];
                }

                break;
            case Enums.CardPlayingEnums.holderThree:

                if (ArrayOfCards_C.Count > 0 && ArrayOfCards_C[ArrayOfCards_C.Count - 1].IsUnlocked())
                {
                    valueReturn = ArrayOfCards_C[ArrayOfCards_C.Count - 1];
                }

                break;
            case Enums.CardPlayingEnums.holderFour:

                if (ArrayOfCards_D.Count > 0 && ArrayOfCards_D[ArrayOfCards_D.Count - 1].IsUnlocked())
                {
                    valueReturn = ArrayOfCards_D[ArrayOfCards_D.Count - 1];
                }

                break;
            case Enums.CardPlayingEnums.holderFive:

                if (ArrayOfCards_E.Count > 0 && ArrayOfCards_E[ArrayOfCards_E.Count - 1].IsUnlocked())
                {
                    valueReturn = ArrayOfCards_E[ArrayOfCards_E.Count - 1];
                }

                break;
            case Enums.CardPlayingEnums.holderSix:

                if (ArrayOfCards_G.Count > 0 && ArrayOfCards_G[ArrayOfCards_G.Count - 1].IsUnlocked())
                {
                    valueReturn = ArrayOfCards_G[ArrayOfCards_G.Count - 1];
                }

                break;
            case Enums.CardPlayingEnums.holderSeven:

                if (ArrayOfCards_H.Count > 0 && ArrayOfCards_H[ArrayOfCards_H.Count - 1].IsUnlocked())
                {
                    valueReturn = ArrayOfCards_H[ArrayOfCards_H.Count - 1];
                }

                break;
        }

        return valueReturn;
    }

	/// <summary>
	/// Gets the card after this.
	/// </summary>
	/// <returns>The card after this.</returns>
	/// <param name="holder">Holder.</param>
	/// <param name="card">Card.</param>
    public List < CardBehaviour > GetCardAfterThis(Enums.CardPlayingEnums holder , CardBehaviour card)
    {
        List <  CardBehaviour > valueReturn = new List<CardBehaviour>();

        int indexGet = -1;

        switch (holder)
        {
            case Enums.CardPlayingEnums.holderOne:

                indexGet = ArrayOfCards_A.FindIndex(x => x == card); 

                if ( indexGet > -1)
                {
                    for ( int i = indexGet + 1; i < ArrayOfCards_A.Count; i++ )
                    {
                        valueReturn.Add(ArrayOfCards_A[i]);
                    }
                }

                break;
            case Enums.CardPlayingEnums.holderTwo:

                indexGet = ArrayOfCards_B.FindIndex(x => x == card);

                if (indexGet > -1)
                {
                    for (int i = indexGet + 1; i < ArrayOfCards_B.Count; i++)
                    {
                        valueReturn.Add(ArrayOfCards_B[i]);
                    }
                }

                break;
            case Enums.CardPlayingEnums.holderThree:

                indexGet = ArrayOfCards_C.FindIndex(x => x == card);

                if (indexGet > -1)
                {
                    for (int i = indexGet + 1; i < ArrayOfCards_C.Count; i++)
                    {
                        valueReturn.Add(ArrayOfCards_C[i]);
                    }
                }

                break;
            case Enums.CardPlayingEnums.holderFour:

                indexGet = ArrayOfCards_D.FindIndex(x => x == card);

                if (indexGet > -1)
                {
                    for (int i = indexGet + 1; i < ArrayOfCards_D.Count; i++)
                    {
                        valueReturn.Add(ArrayOfCards_D[i]);
                    }
                }

                break;
            case Enums.CardPlayingEnums.holderFive:

                indexGet = ArrayOfCards_E.FindIndex(x => x == card);

                if (indexGet > -1)
                {
                    for (int i = indexGet + 1; i < ArrayOfCards_E.Count; i++)
                    {
                        valueReturn.Add(ArrayOfCards_E[i]);
                    }
                }

                break;
            case Enums.CardPlayingEnums.holderSix:

                indexGet = ArrayOfCards_G.FindIndex(x => x == card);

                if (indexGet > -1)
                {
                    for (int i = indexGet + 1; i < ArrayOfCards_G.Count; i++)
                    {
                        valueReturn.Add(ArrayOfCards_G[i]);
                    }
                }

                break;
            case Enums.CardPlayingEnums.holderSeven:

                indexGet = ArrayOfCards_H.FindIndex(x => x == card);

                if (indexGet > -1)
                {
                    for (int i = indexGet + 1; i < ArrayOfCards_H.Count; i++)
                    {
                        valueReturn.Add(ArrayOfCards_H[i]);
                    }
                }

                break;
        }

        return valueReturn;
    }
		
	/// <summary>
	/// Determines whether this instance is this last card the specified card.
	/// </summary>
	/// <returns><c>true</c> if this instance is this last card the specified card; otherwise, <c>false</c>.</returns>
	/// <param name="card">Card.</param>
    public bool IsThisLastCard(CardBehaviour card)
    {
        if ( ArrayOfCards_A.Contains ( card ))
        {
            return ArrayOfCards_A[ArrayOfCards_A.Count - 1] == card;
        }

        if (ArrayOfCards_B.Contains(card))
        {
            return ArrayOfCards_B[ArrayOfCards_B.Count - 1] == card;
        }

        if (ArrayOfCards_C.Contains(card))
        {
            return ArrayOfCards_C[ArrayOfCards_C.Count - 1] == card;
        }

        if (ArrayOfCards_D.Contains(card))
        {
            return ArrayOfCards_D[ArrayOfCards_D.Count - 1] == card;
        }

        if (ArrayOfCards_E.Contains(card))
        {
            return ArrayOfCards_E[ArrayOfCards_E.Count - 1] == card;
        }

        if (ArrayOfCards_G.Contains(card))
        {
            return ArrayOfCards_G[ArrayOfCards_G.Count - 1] == card;
        }

        if (ArrayOfCards_H.Contains(card))
        {
            return ArrayOfCards_H[ArrayOfCards_H.Count - 1] == card;
        }

        return false;
    }
	#endregion

	#region Hint

	/// <summary>
	/// Gets the hint cards. Use For hint.
	/// </summary>
	/// <returns>The hint cards.</returns>
	public List < CardBehaviour > GetHintCards()
	{
		List < CardBehaviour > valueReturn = new List<CardBehaviour> ();

		List < CardBehaviour > cards = new List<CardBehaviour> ();

		CardBehaviour cardCheck = null;

		CardBehaviour cardResult = null;

		for (int j = 0; j < 7; j++) {

			Enums.CardPlayingEnums holder = (Enums.CardPlayingEnums)j;

			switch (holder) {
			case Enums.CardPlayingEnums.holderOne:

				cards = ArrayOfCards_A;

				break;
			case Enums.CardPlayingEnums.holderTwo:

				cards = ArrayOfCards_B;

				break;
			case Enums.CardPlayingEnums.holderThree:

				cards = ArrayOfCards_C;

				break;
			case Enums.CardPlayingEnums.holderFour:

				cards = ArrayOfCards_D;

				break;
			case Enums.CardPlayingEnums.holderFive:

				cards = ArrayOfCards_E;

				break;
			case Enums.CardPlayingEnums.holderSix:

				cards = ArrayOfCards_G;

				break;
			case Enums.CardPlayingEnums.holderSeven:

				cards = ArrayOfCards_H;

				break;
			}


			if (cards.Count > 0 && cards [cards.Count - 1].IsUnlocked ()) {

				cardCheck = cards [cards.Count - 1];

				cardResult = ResultCardsManager.Instance.GetCardResult (Enums.ResultManager.None, cardCheck);

				if (cardResult != null && cardCheck != null || cardCheck != null && cardCheck.GetDataCard().GetEnumCardValue() == Enums.CardVariables.One ) {

					valueReturn.Add (cardCheck);

					if (cardResult != null && cardCheck.GetDataCard().GetEnumCardValue() != Enums.CardVariables.One) {

						valueReturn.Add (cardResult);
					}

					break;
				}
			}


			if (cards.Count > 0 && cardResult == null) {

				int index = -1;

				for (int i = cards.Count - 1; i > -1; i--) {
					if (i - 1 > -1) {
						if (cards [i - 1].IsUnlocked() && cards [i].GetProperties ().IsSmallerThan (cards [i - 1].GetDataCard ())  && !cards [i].GetProperties ().IsSameColorCard (cards [i - 1].GetDataCard ())) {
							index = i - 1;
						} else
							break;
					}
				}

				if (index == -1) {
					index = cards.Count - 1;
				}

				for (int i = index; i < cards.Count; i++) {

					cardCheck = cards [i];

					cardResult = GetCardResult (holder, cardCheck , i == 0 ? null : cards[i - 1]);

					if (cardResult != null) {

						valueReturn.Add (cardCheck);

						valueReturn.Add (cardResult);

						break;
					} 		
				}

			}

			if (valueReturn.Count == 2)
				break;		
		}

		return valueReturn;
	}

	/// <summary>
	/// Determines whether this instance is have cards.
	/// </summary>
	/// <returns><c>true</c> if this instance is have cards; otherwise, <c>false</c>.</returns>
    public bool IsHaveCards()
    {
        return ArrayOfCards_A.Count > 0 ||
            ArrayOfCards_B.Count > 0 ||
            ArrayOfCards_C.Count > 0 ||
            ArrayOfCards_D.Count > 0 ||
            ArrayOfCards_E.Count > 0 ||
            ArrayOfCards_G.Count > 0 ||
            ArrayOfCards_H.Count > 0;
    }

	/// <summary>
	/// Collects the cards to result holder.
	/// </summary>
	/// <returns><c>true</c>, if cards to result holder was collected, <c>false</c> otherwise.</returns>
    public bool CollectCardsToResultHolder()
    {
        CardBehaviour card;

        if ( ArrayOfCards_A.Count > 0)
        {
            card = ArrayOfCards_A[ArrayOfCards_A.Count - 1];

            if ( card.IsUnlocked () && card.DoUpdateResultRegions())
            {
                ArrayOfCards_A.Remove(card);

                return true;
            }            
        }

        if (ArrayOfCards_B.Count > 0)
        {
            card = ArrayOfCards_B[ArrayOfCards_B.Count - 1];

            if (card.IsUnlocked() && card.DoUpdateResultRegions())
            {
                ArrayOfCards_B.Remove(card);

                return true;
            }
        }

        if (ArrayOfCards_C.Count > 0)
        {
            card = ArrayOfCards_C[ArrayOfCards_C.Count - 1];

            if (card.IsUnlocked() && card.DoUpdateResultRegions())
            {
                ArrayOfCards_C.Remove(card);

                return true;
            }
        }

        if (ArrayOfCards_D.Count > 0)
        {
            card = ArrayOfCards_D[ArrayOfCards_D.Count - 1];

            if (card.IsUnlocked() && card.DoUpdateResultRegions())
            {
                ArrayOfCards_D.Remove(card);

                return true;
            }
        }

        if (ArrayOfCards_E.Count > 0)
        {
            card = ArrayOfCards_E[ArrayOfCards_E.Count - 1];

            if (card.IsUnlocked() && card.DoUpdateResultRegions())
            {
                ArrayOfCards_E.Remove(card);

                return true;
            }
        }

        if (ArrayOfCards_G.Count > 0)
        {
            card = ArrayOfCards_G[ArrayOfCards_G.Count - 1];

            if (card.IsUnlocked() && card.DoUpdateResultRegions())
            {
                ArrayOfCards_G.Remove(card);

                return true;
            }
        }

        if (ArrayOfCards_H.Count > 0)
        {
            card = ArrayOfCards_H[ArrayOfCards_H.Count - 1];

            if (card.IsUnlocked() && card.DoUpdateResultRegions())
            {
                ArrayOfCards_H.Remove(card);

                return true;
            }
        }

        return false;
    }

	/// <summary>
	/// Gets the card result. Use For hint
	/// </summary>
	/// <returns>The card result.</returns>
	/// <param name="except">Except.</param>
	/// <param name="cardCompare">Card compare.</param>
	public CardBehaviour GetCardResult(Enums.CardPlayingEnums except , CardBehaviour cardCompare , CardBehaviour cardBefore = null)
	{
		CardBehaviour cardResult = null;

		bool IsFoundTheCard = false;

		for (int i = 0; i < 7; i++) {

			if (IsFoundTheCard)
				break;

			if ((int)except != i) {
				switch (i) {
				case 0:

					cardResult = GetLastCard (Enums.CardPlayingEnums.holderOne);

					if (cardResult != null && cardCompare.GetProperties ().IsSmallerThan (cardResult.GetDataCard()) && !cardCompare.GetProperties().IsSameColorCard(cardResult.GetDataCard())) {

						if (cardBefore == null || cardBefore.GetDataCard().GetCardValue () != cardResult.GetDataCard().GetCardValue() || !cardBefore.IsUnlocked()) {

							IsFoundTheCard = true;

						} 
					}

					break;
				case 1:

					cardResult = GetLastCard (Enums.CardPlayingEnums.holderTwo);

					if (cardResult != null && cardCompare.GetProperties ().IsSmallerThan (cardResult.GetDataCard()) && !cardCompare.GetProperties().IsSameColorCard(cardResult.GetDataCard())) {
						if (cardBefore == null || cardBefore.GetDataCard().GetCardValue () != cardResult.GetDataCard().GetCardValue() || !cardBefore.IsUnlocked()) {

							IsFoundTheCard = true;

						} 
					}

					break;
				case 2:

					cardResult = GetLastCard (Enums.CardPlayingEnums.holderThree);

					if (cardResult != null && cardCompare.GetProperties ().IsSmallerThan (cardResult.GetDataCard())&& !cardCompare.GetProperties().IsSameColorCard(cardResult.GetDataCard())) {
						if (cardBefore == null || cardBefore.GetDataCard().GetCardValue () != cardResult.GetDataCard().GetCardValue() || !cardBefore.IsUnlocked()) {

							IsFoundTheCard = true;

						} 
					}

					break;
				case 3:

					cardResult = GetLastCard (Enums.CardPlayingEnums.holderFour);

					if (cardResult != null && cardCompare.GetProperties ().IsSmallerThan (cardResult.GetDataCard())&& !cardCompare.GetProperties().IsSameColorCard(cardResult.GetDataCard())) {
						if (cardBefore == null || cardBefore.GetDataCard().GetCardValue () != cardResult.GetDataCard().GetCardValue() || !cardBefore.IsUnlocked()) {

							IsFoundTheCard = true;

						} 
					}

					break;
				case 4:

					cardResult = GetLastCard (Enums.CardPlayingEnums.holderFive);

					if (cardResult != null && cardCompare.GetProperties ().IsSmallerThan (cardResult.GetDataCard())&& !cardCompare.GetProperties().IsSameColorCard(cardResult.GetDataCard())) {
						if (cardBefore == null || cardBefore.GetDataCard().GetCardValue () != cardResult.GetDataCard().GetCardValue() || !cardBefore.IsUnlocked()) {

							IsFoundTheCard = true;

						} 
					}

					break;
				case 5:

					cardResult = GetLastCard (Enums.CardPlayingEnums.holderSix);

					if (cardResult != null && cardCompare.GetProperties ().IsSmallerThan (cardResult.GetDataCard())&& !cardCompare.GetProperties().IsSameColorCard(cardResult.GetDataCard())) {
						if (cardBefore == null || cardBefore.GetDataCard().GetCardValue () != cardResult.GetDataCard().GetCardValue() || !cardBefore.IsUnlocked()) {

							IsFoundTheCard = true;

						} 
					}

					break;
				case 6:

					cardResult = GetLastCard (Enums.CardPlayingEnums.holderSeven);

					if (cardResult != null && cardCompare.GetProperties ().IsSmallerThan (cardResult.GetDataCard())&& !cardCompare.GetProperties().IsSameColorCard(cardResult.GetDataCard())) {
						if (cardBefore == null || cardBefore.GetDataCard().GetCardValue () != cardResult.GetDataCard().GetCardValue() || !cardBefore.IsUnlocked()) {

							IsFoundTheCard = true;

						} 
					}

					break;
				}
			}
		}

		if (IsFoundTheCard == false)
			cardResult = null;

		return cardResult;
	}

	/// <summary>
	/// Determines whether this instance is have empty space.
	/// </summary>
	/// <returns><c>true</c> if this instance is have empty space; otherwise, <c>false</c>.</returns>
	public bool IsHaveEmptySpace()
	{
        //TODO: Check the empty space.
        bool IsEmpty = ArrayOfCards_A.Count == 0 || ArrayOfCards_B.Count == 0 || ArrayOfCards_C.Count == 0 || ArrayOfCards_D.Count == 0 || ArrayOfCards_E.Count == 0 || ArrayOfCards_G.Count == 0 || ArrayOfCards_H.Count == 0;

        bool IsHaveKingCard = false;

        if ( ArrayOfCards_A.Count > 0 && IsHaveKingCard == false)
        {
            int value = -1;

            for ( int  i = 0; i < ArrayOfCards_A.Count; i++)
            {
                if ( ArrayOfCards_A[i].IsUnlocked() == true)
                {
                    if ( ArrayOfCards_A [i].GetDataCard().GetEnumCardValue () == Enums.CardVariables.King && i > 0)
                    {
                        IsHaveKingCard = true;

                        value = ArrayOfCards_A[i].GetDataCard().GetCardValue();
                    } else if ( value > -1 && ArrayOfCards_A[i].GetDataCard().GetCardValue() + 1 == value)
                    {
                        value = ArrayOfCards_A[i].GetDataCard().GetCardValue();
                    } else if ( value > - 1)
                    {
                        IsHaveKingCard = false;

                        break;
                    }
                }
            }
        }

        if (ArrayOfCards_B.Count > 0 && IsHaveKingCard == false)
        {
            int value = -1;

            for (int i = 0; i < ArrayOfCards_B.Count; i++)
            {
                if (ArrayOfCards_B[i].IsUnlocked() == true)
                {
                    if (ArrayOfCards_B[i].GetDataCard().GetEnumCardValue() == Enums.CardVariables.King && i > 0)
                    {
                        IsHaveKingCard = true;

                        value = ArrayOfCards_B[i].GetDataCard().GetCardValue();
                    }
                    else if (value > -1 && ArrayOfCards_B[i].GetDataCard().GetCardValue() + 1 == value)
                    {
                        value = ArrayOfCards_B[i].GetDataCard().GetCardValue();
                    }
                    else if (value > -1)
                    {
                        IsHaveKingCard = false;

                        break;
                    }
                }
            }
        }

        if (ArrayOfCards_C.Count > 0 && IsHaveKingCard == false)
        {
            int value = -1;

            for (int i = 0; i < ArrayOfCards_C.Count; i++)
            {
                if (ArrayOfCards_C[i].IsUnlocked() == true)
                {
                    if (ArrayOfCards_C[i].GetDataCard().GetEnumCardValue() == Enums.CardVariables.King && i > 0)
                    {
                        IsHaveKingCard = true;

                        value = ArrayOfCards_C[i].GetDataCard().GetCardValue();
                    }
                    else if (value > -1 && ArrayOfCards_C[i].GetDataCard().GetCardValue() + 1 == value)
                    {
                        value = ArrayOfCards_C[i].GetDataCard().GetCardValue();
                    }
                    else if (value > -1)
                    {
                        IsHaveKingCard = false;

                        break;
                    }
                }
            }
        }

        if (ArrayOfCards_D.Count > 0 && IsHaveKingCard == false)
        {
            int value = -1;

            for (int i = 0; i < ArrayOfCards_D.Count; i++)
            {
                if (ArrayOfCards_D[i].IsUnlocked() == true)
                {
                    if (ArrayOfCards_D[i].GetDataCard().GetEnumCardValue() == Enums.CardVariables.King && i > 0)
                    {
                        IsHaveKingCard = true;

                        value = ArrayOfCards_D[i].GetDataCard().GetCardValue();
                    }
                    else if (value > -1 && ArrayOfCards_D[i].GetDataCard().GetCardValue() + 1 == value)
                    {
                        value = ArrayOfCards_D[i].GetDataCard().GetCardValue();
                    }
                    else if (value > -1)
                    {
                        IsHaveKingCard = false;

                        break;
                    }
                }
            }
        }

        if (ArrayOfCards_E.Count > 0 && IsHaveKingCard == false)
        {
            int value = -1;

            for (int i = 0; i < ArrayOfCards_E.Count; i++)
            {
                if (ArrayOfCards_E[i].IsUnlocked() == true)
                {
                    if (ArrayOfCards_E[i].GetDataCard().GetEnumCardValue() == Enums.CardVariables.King && i > 0)
                    {
                        IsHaveKingCard = true;

                        value = ArrayOfCards_E[i].GetDataCard().GetCardValue();
                    }
                    else if (value > -1 && ArrayOfCards_E[i].GetDataCard().GetCardValue() + 1 == value)
                    {
                        value = ArrayOfCards_E[i].GetDataCard().GetCardValue();
                    }
                    else if (value > -1)
                    {
                        IsHaveKingCard = false;

                        break;
                    }
                }
            }           
        }

        if (ArrayOfCards_G.Count > 0 && IsHaveKingCard == false)
        {
            int value = -1;

            for (int i = 0; i < ArrayOfCards_G.Count; i++)
            {
                if (ArrayOfCards_G[i].IsUnlocked() == true)
                {
                    if (ArrayOfCards_G[i].GetDataCard().GetEnumCardValue() == Enums.CardVariables.King && i > 0)
                    {
                        IsHaveKingCard = true;

                        value = ArrayOfCards_G[i].GetDataCard().GetCardValue();
                    }
                    else if (value > -1 && ArrayOfCards_G[i].GetDataCard().GetCardValue() + 1 == value)
                    {
                        value = ArrayOfCards_G[i].GetDataCard().GetCardValue();
                    }
                    else if (value > -1)
                    {
                        IsHaveKingCard = false;

                        break;
                    }
                }
            }
        }

        if (ArrayOfCards_H.Count > 0 && IsHaveKingCard == false)
        {
            int value = -1;

            for (int i = 0; i < ArrayOfCards_H.Count; i++)
            {
                if (ArrayOfCards_H[i].IsUnlocked() == true)
                {
                    if (ArrayOfCards_H[i].GetDataCard().GetEnumCardValue() == Enums.CardVariables.King && i > 0)
                    {
                        IsHaveKingCard = true;

                        value = ArrayOfCards_H[i].GetDataCard().GetCardValue();
                    }
                    else if (value > -1 && ArrayOfCards_H[i].GetDataCard().GetCardValue() + 1 == value)
                    {
                        value = ArrayOfCards_H[i].GetDataCard().GetCardValue();
                    }
                    else if (value > -1)
                    {
                        IsHaveKingCard = false;

                        break;
                    }
                }
            }
        }


        return IsEmpty && IsHaveKingCard;
	}
	#endregion
}
