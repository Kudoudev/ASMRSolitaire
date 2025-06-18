using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HidenCardsManager : SingletonMonoBehaviour<HidenCardsManager>
{
    [System.Serializable]
    public struct TransformContents
    {
        /// <summary>
        /// The transform cards hidden.
        /// </summary>
        public RectTransform TransformCardsHidden;

        /// <summary>
        /// The transform cards shower.
        /// </summary>
        public RectTransform TransformCardsShower;
    }

    // ========================== References ======================= //

    #region References

    /// <summary>
    /// The holder hints.
    /// </summary>
    private Transform TransformCardsHidden;

    /// <summary>
    /// The holder shower.
    /// </summary>
    private Transform TransformCardsShower;

    [Header("Portrait")]
    /// <summary>
    /// The portrait transform on left.
    /// </summary>
    public TransformContents PortraitTransformOnLeft;

    /// <summary>
    /// The portrait transform on right.
    /// </summary>
    public TransformContents PortraitTransformOnRight;

    [Header("Landscape")]
    /// <summary>
    /// The landscape transform on left.
    /// </summary>
    public TransformContents LandscapeTransformOnLeft;

    /// <summary>
    /// The landscape transform on right.
    /// </summary>
    public TransformContents LandscapeTransformOnRight;

    /// <summary>
    /// The animation hint.
    /// </summary>
    public Animator[] AnimationHint;

    #endregion


    // ========================= Variables ========================= //

    #region Variables

    /// <summary>
    /// The cards on holder.
    /// </summary>
    protected List<CardBehaviour> cardsUsingLock = new List<CardBehaviour>();

    /// <summary>
    /// The cards on shower.
    /// </summary>
    protected List<CardBehaviour> cardUsingUnlock = new List<CardBehaviour>();

    #endregion

    // ========================= Cache ============================ //

    #region Cache

    /// <summary>
    /// The transform.
    /// </summary>
    protected new Transform transform;

    /// <summary>
    /// The card animation.
    /// </summary>
    protected CardBehaviour cardAnimation;

    /// <summary>
    /// The is waiting animation.
    /// </summary>
    protected bool IsWaitingAnimation;

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

    /// <summary>
    /// Start this instance.
    /// </summary>
    public void Reset()
    {
        for (int i = 0; i < cardsUsingLock.Count; i++)
        {
            cardsUsingLock[i].UpdateStateCard(Enums.CardBoard.CardHint);
        }
    }

    /// <summary>
    /// Updates the new cards to holder.
    /// </summary>
    /// <param name="card">Card.</param>
    /// <param name="IsCardOnShower">If set to <c>true</c> is card on shower.</param>
    public void AddTheNewCardsToHidenCards(CardBehaviour card, bool IsCardOnShower)
    {
        if (cardUsingUnlock.Contains(card))
        {
            cardUsingUnlock.Remove(card);
        }

        if (cardsUsingLock.Contains(card))
        {
            cardsUsingLock.Remove(card);
        }

        if (IsCardOnShower)
        {
            cardUsingUnlock.Add(card);
        }
        else
        {
            cardsUsingLock.Add(card);
        }

        RefreshCards();
    }

    /// <summary>
    /// Ises the have cards.
    /// </summary>
    /// <returns><c>true</c>, if have cards was ised, <c>false</c> otherwise.</returns>
    public bool IsExistAnyCards()
    {
        return cardsUsingLock.Count > 0 || cardUsingUnlock.Count > 0;
    }

    /// <summary>
    /// Shows the hint cards.
    /// </summary>
    public void DoShowingLockedCards()
    {
        if (IsWaitingAnimation)
            return;

        HudSystem.Instance.UpdateMove(++Contains.Moves);

        GamePlay.Instance.DisableHintGame();

        if (cardsUsingLock.Count > 0)
        {
            List<CardBehaviour> cardShow = new List<CardBehaviour>();

            int numberCardsWillShow = GameManager.Instance.ModeGame == Enums.ModeGame.Easy ? 1 : 3;

            int limitChecking = Mathf.Clamp(cardsUsingLock.Count - numberCardsWillShow, 0, int.MaxValue);

            int step = 0;

            for (int i = limitChecking; i < cardsUsingLock.Count; i++)
            {
                CardBehaviour cardFound = cardsUsingLock[i];

                if (step == 0)
                {
                    UndoSystem.Instance.RecordState(cardFound, cardFound.targetPositionCards, true,
                        Enums.CardPlayingEnums.None, Enums.ResultManager.None, cardFound.IsUnlocked(), false,
                        Contains.Score, false);
                }
                else
                {
                    UndoSystem.Instance.RecordState(cardFound, cardFound.targetPositionCards, true,
                        Enums.CardPlayingEnums.None, Enums.ResultManager.None, cardFound.IsUnlocked(), false,
                        Contains.Score, true);
                }

                step++;
            }

            for (int i = cardsUsingLock.Count - 1; i >= limitChecking; i--)
            {
                CardBehaviour cardFound = cardsUsingLock[i];

                cardFound.UnlockCard(true);

                cardUsingUnlock.Add(cardFound);

                cardShow.Add(cardFound);
            }

            for (int i = 0; i < cardShow.Count; i++)
            {
                if (cardsUsingLock.Contains(cardShow[i]))
                {
                    cardsUsingLock.Remove(cardShow[i]);
                }
            }


            RefreshCards();

            if (cardsUsingLock.Count == 0)
            {
                GamePlay.Instance.CheckIsConditionLose();
            }
        }
        else
        {
            if (cardUsingUnlock.Count > 0)
            {
                IsWaitingAnimation = true;

                int step = 0;

                for (int i = cardUsingUnlock.Count - 1; i > -1; i--)
                {
                    if (step == 0)
                    {
                        UndoSystem.Instance.RecordState(cardUsingUnlock[i], cardUsingUnlock[i].targetPositionCards,
                            true, Enums.CardPlayingEnums.None, Enums.ResultManager.None,
                            cardUsingUnlock[i].IsUnlocked(), true, Contains.Score, false);
                    }
                    else
                    {
                        UndoSystem.Instance.RecordState(cardUsingUnlock[i], cardUsingUnlock[i].targetPositionCards,
                            true, Enums.CardPlayingEnums.None, Enums.ResultManager.None,
                            cardUsingUnlock[i].IsUnlocked(), true, Contains.Score, true);
                    }

                    step++;

                    cardUsingUnlock[i].UnlockCard(false);

                    cardsUsingLock.Add(cardUsingUnlock[i]);
                }

                cardUsingUnlock.Clear();

                RefreshCards();

                Invoke(nameof(InvokeCompletedAnimation), 0.1f);
            }
        }
    }

    /// <summary>
    /// Determines whether this instance is lasted card the specified card.
    /// </summary>
    /// <returns><c>true</c> if this instance is lasted card the specified card; otherwise, <c>false</c>.</returns>
    /// <param name="card">Card.</param>
    public bool IsLastedCard(CardBehaviour card)
    {
        if (cardUsingUnlock.Count == 0)
            return false;

        if (cardUsingUnlock[cardUsingUnlock.Count - 1] == card)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Invokes the completed animation.
    /// </summary>
    void InvokeCompletedAnimation()
    {
        IsWaitingAnimation = false;
    }

    /// <summary>
    /// Refreshs the cards.
    /// </summary>
    /// <param name="direction">Direction.</param>
    public void RefreshCards(Enums.Direction direction)
    {
        // Sort the list of cards from the transform of cards hidden.
        Helper.SortCards(cardsUsingLock, TransformCardsHidden, 3, direction, Contains.DistanceSortReview, true);

        // Sort the list of cards from the transform of cards shower.
        Helper.SortCardsUnlock(cardUsingUnlock, TransformCardsShower, 3, direction, Contains.DistanceSortHintCards,
            true);
    }

    /// <summary>
    /// Refreshs the cards.
    /// </summary>
    public void RefreshCards()
    {
        if (MutilResolution.Instance.IsPortrait)
        {
            RefreshCards(Contains.IsRightHanded ? Enums.Direction.Right : Enums.Direction.Left);
        }
        else
        {
            RefreshCards(Contains.IsRightHanded ? Enums.Direction.Left : Enums.Direction.Right);
        }
    }

    /// <summary>
    /// Updates the portrait on left.
    /// </summary>
    public void UpdatePortraitOnLeft()
    {
        // Update the transform of list cards hidden.
        TransformCardsHidden = PortraitTransformOnLeft.TransformCardsHidden;

        // Update the transform of list cards shower.
        TransformCardsShower = PortraitTransformOnLeft.TransformCardsShower;
    }

    /// <summary>
    /// Updates the portrait on right.
    /// </summary>
    public void UpdatePortraitOnRight()
    {
        // Update the transform of list cards hidden.
        TransformCardsHidden = PortraitTransformOnRight.TransformCardsHidden;

        // Update the transform of list cards shower.
        TransformCardsShower = PortraitTransformOnRight.TransformCardsShower;
    }

    /// <summary>
    /// Updates the landscape on left.
    /// </summary>
    public void UpdateLandscapeOnLeft()
    {
        // Update the transform of list cards hidden.
        TransformCardsHidden = LandscapeTransformOnLeft.TransformCardsHidden;

        // Update the transform of list cards shower.
        TransformCardsShower = LandscapeTransformOnLeft.TransformCardsShower;
    }

    /// <summary>
    /// Updates the landscape on right.
    /// </summary>
    public void UpdateLandscapeOnRight()
    {
        // Update the transform of list cards hidden.
        TransformCardsHidden = LandscapeTransformOnRight.TransformCardsHidden;

        // Update the transform of list cards shower.
        TransformCardsShower = LandscapeTransformOnRight.TransformCardsShower;
    }

    /// <summary>
    /// Updates the cards.
    /// </summary>
    public void UpdateCards(List<CardBehaviour> cards)
    {
        // Adding the card to the Locking cards.
        cardsUsingLock.AddRange(cards);

        // Refresh the cards with direction.
        RefreshCards();
    }

    /// <summary>
    /// Gets the last card holder.
    /// </summary>
    /// <returns>The last card holder.</returns>
    public CardBehaviour GetLastCardHolder()
    {
        CardBehaviour card = null;

        if (cardsUsingLock.Count > 0)
        {
            card = cardsUsingLock[cardsUsingLock.Count - 1];

            cardsUsingLock.RemoveAt(cardsUsingLock.Count - 1);
        }

        return card;
    }

    /// <summary>
    /// Gets the last card shower.
    /// </summary>
    /// <returns>The last card shower.</returns>
    public CardBehaviour GetLastCardShower()
    {
        CardBehaviour card = null;

        if (cardUsingUnlock.Count > 0)
        {
            card = cardUsingUnlock[cardUsingUnlock.Count - 1];

            cardUsingUnlock.RemoveAt(cardUsingUnlock.Count - 1);
        }

        return card;
    }

    /// <summary>
    /// Determines whether this instance is have cards on shower the specified card.
    /// </summary>
    /// <returns><c>true</c> if this instance is have cards on shower the specified card; otherwise, <c>false</c>.</returns>
    /// <param name="card">Card.</param>
    public bool IsHaveCardsOnShower(CardBehaviour card)
    {
        return cardUsingUnlock.Contains(card);
    }

    /// <summary>
    /// Outs the of holder.
    /// </summary>
    /// <param name="card">Card.</param>
    public void OutOfHolder(CardBehaviour card)
    {
        if (cardUsingUnlock.Contains(card))
        {
            cardUsingUnlock.Remove(card);

            return;
        }
    }

    #endregion

    #region Helper

    /// <summary>
    /// Determines whether this instance is that last cards showing the specified card.
    /// </summary>
    /// <returns><c>true</c> if this instance is that last cards showing the specified card; otherwise, <c>false</c>.</returns>
    /// <param name="card">Card.</param>
    public bool IsThatLastCardsShowing(CardBehaviour card)
    {
        if (cardUsingUnlock.Count == 0)
        {
            return false;
        }

        if (cardUsingUnlock[cardUsingUnlock.Count - 1] == card)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Gets the hint cards.
    /// </summary>
    /// <returns>The hint cards.</returns>
    public List<CardBehaviour> GetHintCards()
    {
        List<CardBehaviour> valueReturn = new List<CardBehaviour>();

        int stepcard = GameManager.Instance.ModeGame == Enums.ModeGame.Easy ? 1 : 3;

        CardBehaviour cardCheck = null;

        CardBehaviour cardResult = null;

        for (int i = cardUsingUnlock.Count;
             i > 0;
             i = Mathf.Clamp(i % stepcard > 0 ? i - (i % stepcard) : i - stepcard, -1, int.MaxValue))
        {
            cardCheck = cardUsingUnlock[i - 1];

            cardResult = ResultCardsManager.Instance.GetCardResult(Enums.ResultManager.None, cardCheck);

            if (cardResult != null ||
                cardCheck != null && cardCheck.GetDataCard().GetEnumCardValue() == Enums.CardVariables.One)
            {
                break;
            }

            cardResult = PlayingCardsManager.Instance.GetCardResult(Enums.CardPlayingEnums.None, cardCheck);

            if (cardResult != null)
            {
                break;
            }
        }

        if (cardResult != null && cardCheck != null)
        {
            valueReturn.Add(cardCheck);

            valueReturn.Add(cardResult);
        }

        if (cardResult == null && cardCheck != null &&
            cardCheck.GetDataCard().GetEnumCardValue() == Enums.CardVariables.One)
        {
            valueReturn.Clear();

            valueReturn.Add(cardCheck);
        }

        return valueReturn;
    }


    /// <summary>
    /// Determines whether this instance is have card usefull.
    /// </summary>
    /// <returns><c>true</c> if this instance is have card usefull; otherwise, <c>false</c>.</returns>
    public bool IsHaveLockedCards()
    {
        if (cardsUsingLock.Count > 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// The is active animation.
    /// </summary>
    protected int IsActiveAnimation = Animator.StringToHash("IsActiveAnimation");

    /// <summary>
    /// Enables the hint animation.
    /// </summary>
    public void EnableHintAnimation()
    {
        for (int i = 0; i < AnimationHint.Length; i++)
        {
            if (AnimationHint[i].gameObject.activeInHierarchy)
                AnimationHint[i].SetBool(IsActiveAnimation, true);
        }
    }

    /// <summary>
    /// Disables the hint animation.
    /// </summary>
    public void DisableHintAnimation()
    {
        for (int i = 0; i < AnimationHint.Length; i++)
        {
            if (AnimationHint[i].gameObject.activeInHierarchy)
                AnimationHint[i].SetBool(IsActiveAnimation, false);
        }

        HintCardsManager.Instance.DisableHint();
    }

    #endregion
}