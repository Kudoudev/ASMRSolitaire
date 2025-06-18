using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

/// <summary>
/// Card properties.
/// </summary>
[System.Serializable]
public class CardProperties
{
    // ========================= Variables ====================== //


    #region Variables

    /// <summary>
    /// The properties.
    /// </summary>
    private CardDataProperties properties;

    /// <summary>
    /// The card on board.
    /// </summary>
    protected Enums.CardBoard cardOnBoard;

    /// <summary>
    /// The is card unlock.
    /// </summary>
    public bool IsCardUnlocked;

    #endregion

    // ======================= Functional ======================== //

    #region Functional

    /// <summary>
    /// Inits the card.
    /// </summary>
    /// <param name="properties">Properties.</param>
    /// <param name="cardOnBoard">Card on board.</param>
    public void InitCard(CardDataProperties properties, Enums.CardBoard cardOnBoard)
    {
        this.properties = properties;

        this.cardOnBoard = cardOnBoard;

        this.IsCardUnlocked = false;
    }

    /// <summary>
    /// Updates the state card.
    /// </summary>
    /// <param name="cardOnBoard">Card on board.</param>
    public void UpdateStateCard(Enums.CardBoard cardOnBoard)
    {
        this.cardOnBoard = cardOnBoard;
    }

    /// <summary>
    /// Determines whether this instance is card on board.
    /// </summary>
    /// <returns><c>true</c> if this instance is card on board; otherwise, <c>false</c>.</returns>
    public bool IsCardOnBoard()
    {
        return cardOnBoard == Enums.CardBoard.CardUse;
    }


    public bool IsCardOnHidenHolder()
    {
        return cardOnBoard == Enums.CardBoard.CardHint;
    }

    /// <summary>
    /// Gets the data properties.
    /// </summary>
    /// <returns>The data properties.</returns>
    public CardDataProperties GetDataProperties()
    {
        return properties;
    }

    /// <summary>
    /// Determines whether this instance is same color card the specified data.
    /// </summary>
    /// <returns><c>true</c> if this instance is same color card the specified data; otherwise, <c>false</c>.</returns>
    /// <param name="data">Data.</param>
    public bool IsSameColorCard(CardDataProperties data)
    {
        /// Check same color between diamonds and Hearts.
        if (data.GetCardType() == Enums.CardType.Heart || data.GetCardType() == Enums.CardType.Diamond)
        {
            if (properties.GetCardType() == Enums.CardType.Heart || properties.GetCardType() == Enums.CardType.Diamond)
            {
                return true;
            }
        }

        /// Check same color between Spades and Clubs.
        if (data.GetCardType() == Enums.CardType.Spade || data.GetCardType() == Enums.CardType.Club)
        {
            if (properties.GetCardType() == Enums.CardType.Spade || properties.GetCardType() == Enums.CardType.Club)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Determines whether this instance is smaller than the specified data.
    /// </summary>
    /// <returns><c>true</c> if this instance is smaller than the specified data; otherwise, <c>false</c>.</returns>
    /// <param name="data">Data.</param>
    public bool IsSmallerThan(CardDataProperties data)
    {
        /// Calculate the value distance between two cards. maximum distance = 1. 
        if (properties.GetCardValue() + 1 == data.GetCardValue())
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Determines whether this instance is bigger than the specified data.
    /// </summary>
    /// <returns><c>true</c> if this instance is bigger than the specified data; otherwise, <c>false</c>.</returns>
    /// <param name="data">Data.</param>
    public bool IsBiggerThan(CardDataProperties data)
    {
        /// Calculate the value distance between two cards. maximum distance = 1.
        if (properties.GetCardValue() - 1 == data.GetCardValue())
        {
            return true;
        }

        return false;
    }

    #endregion
}

/// <summary>
/// Card UI.
/// </summary>
[System.Serializable]
public struct CardUI
{
    /// <summary>
    /// The user interface cards.
    /// </summary>
    [SerializeField] private Image UICards;

    [SerializeField] private Image UICardsParrent;

    // =========================== Functional ============================ //

    #region Functional

    /// <summary>
    /// Updates the image card.
    /// </summary>
    /// <param name="param">Parameter.</param>
    public void UpdateImageCard(Sprite param)
    {
        if (param == null)
            return;

        UICards.sprite = param;

        UICardsParrent.sprite = param;
    }

    #endregion
}

/// <summary>
/// Card behaviour. Manage all function from card.
/// </summary>
public class CardBehaviour : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler, IPointerClickHandler
{
    // ============================= References =========================== //

    #region References

    [Header("UI")]
    /// <summary>
    /// The card UI.
    /// </summary>
    [SerializeField]
    private CardUI cardUI;

    /// <summary>
    /// The animation.
    /// </summary>
    [SerializeField] private Animator animation;

    #endregion

    // ============================= Variables ============================ //

    #region Variables

    /// <summary>
    /// The properties.
    /// </summary>
    protected CardProperties properties = new CardProperties();

    /// <summary>
    /// The pointer.
    /// </summary>
    protected Vector3 pointer;

    /// <summary>
    /// The position update.
    /// </summary>
    Vector3 positionUpdate;

    /// <summary>
    /// The last holder.
    /// </summary>
    private Transform parentHolder;

    /// <summary>
    /// The last index of the sliding.
    /// </summary>
    private int LastSlidingIndex;

    /// <summary>
    /// The is drag.
    /// </summary>
    protected bool IsDrag;

    /// <summary>
    /// The is busy animation.
    /// </summary>
    protected bool IsBusyAnimation;

    /// <summary>
    /// The target position cards.
    /// </summary>
    public Vector3 targetPositionCards;

    /// <summary>
    /// The last target position.
    /// </summary>
    protected Vector3 lastTargetPosition;

    #endregion

    // =========================== Cache ================================= //

    #region Cache

    /// <summary>
    /// The transform.
    /// </summary>
    private new Transform transform;

    /// <summary>
    /// The is active animation.
    /// </summary>
    protected int IsActiveAnimation = Animator.StringToHash("IsActiveAnimation");

    /// <summary>
    /// The is rotation.
    /// </summary>
    protected int IsRotation = Animator.StringToHash("IsRotation");

    #endregion

    // ============================ Funcional ============================ //

    #region Functional

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake()
    {
        InitCache();
    }

    /// <summary>
    /// Refresh this instance.
    /// </summary>
    public void Refresh()
    {
        DisableOutline();
    }

    /// <summary>
    /// Inits the cache.
    /// </summary>
    public void InitCache()
    {
        transform = gameObject.transform;
    }

    /// <summary>
    /// Init the specified dataProperties and cardOnBoard.
    /// </summary>
    /// <param name="dataProperties">Data properties.</param>
    /// <param name="cardOnBoard">Card on board.</param>
    public void Init(CardDataProperties dataProperties, Enums.CardBoard cardOnBoard, bool IsUnlocked = false)
    {
        // Init the propeties for this cards.
        if (!ReferenceEquals(properties, null))
        {
            properties.InitCard(dataProperties, cardOnBoard);
        }
        else
        {
            throw new UnityException("Cards was not found the properties.");
        }

        // Set State draging = false;
        IsDrag = false;

        // Lock or unlock this card if they are available or not.
        UnlockCard(IsUnlocked);
    }

    /// <summary>
    /// Unlocks the card.
    /// </summary>
    /// <param name="IsUnlock">If set to <c>true</c> is unlock.</param>
    public void UnlockCard(bool IsUnlock)
    {
        properties.IsCardUnlocked = IsUnlock;

        if (IsUnlock)
        {
            // Set image unlock for this card if available.

            cardUI.UpdateImageCard(properties.GetDataProperties().GetCardSprite());

            if (GamePlay.Instance != null)
            {
                // Update state list Unlock cards to check wining.
                GamePlay.Instance.UpdateUnlockedCard(this);
            }
        }
        else
        {
            // Set image lock for this card if available.

            if (GamePlay.Instance != null)
            {
                cardUI.UpdateImageCard(DataSystem.Instance.GetDefaultCard((int)GamePlay.Instance.GameThemes));
            }
            else
            {
                cardUI.UpdateImageCard(DataSystem.Instance.GetDefaultCard());
            }

            if (GamePlay.Instance != null)
            {
                // Update state list lock cards to check wining.
                GamePlay.Instance.UpdatedLockedCard(this);
            }
        }
    }

    #endregion

    #region Event System

    /// <summary>
    /// Raises the drag event.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnDrag(PointerEventData eventData)
    {
        if (!properties.IsCardUnlocked)
            return;

        // If game state do not equal Playing return.
        if (GameManager.Instance.IsGameReady() == false)
            return;

        if (!IsDrag)
        {
            return;
        }

        // Check if Cards on the hinden cards.
        if (properties.IsCardOnHidenHolder())
        {
            if (!HidenCardsManager.Instance.IsThatLastCardsShowing(this))
            {
                return;
            }
        }

        positionUpdate = Helper.GetWorldPosition();

        positionUpdate.x = positionUpdate.x + pointer.x;

        positionUpdate.y = positionUpdate.y + pointer.y;

        positionUpdate.z = 0;

        transform.position = positionUpdate;

        LogGame.DebugLog(string.Format("On Drag - {0}", transform.name));
    }

    /// <summary>
    /// Raises the end drag event.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!properties.IsCardUnlocked)
            return;

        if (GameManager.Instance.IsGameReady() == false)
            return;

        if (properties.IsCardOnHidenHolder())
        {
            if (!HidenCardsManager.Instance.IsThatLastCardsShowing(this))
            {
                return;
            }
        }

        DisableOutline();

        bool HaveMove = false;
        Vector3 positionGetFromCards = transform.position;

        if (DoCheckOnPlayingCards(Enums.CardPlayingEnums.holderOne, positionGetFromCards))
        {
            HaveMove = true;
        }
        else if (DoCheckOnPlayingCards(Enums.CardPlayingEnums.holderTwo, positionGetFromCards))
        {
            HaveMove = true;
        }
        else if (DoCheckOnPlayingCards(Enums.CardPlayingEnums.holderThree, positionGetFromCards))
        {
            HaveMove = true;
        }
        else if (DoCheckOnPlayingCards(Enums.CardPlayingEnums.holderFour, positionGetFromCards))
        {
            HaveMove = true;
        }
        else if (DoCheckOnPlayingCards(Enums.CardPlayingEnums.holderFive, positionGetFromCards))
        {
            HaveMove = true;
        }
        else if (DoCheckOnPlayingCards(Enums.CardPlayingEnums.holderSix, positionGetFromCards))
        {
            HaveMove = true;
        }
        else if (DoCheckOnPlayingCards(Enums.CardPlayingEnums.holderSeven, positionGetFromCards))
        {
            HaveMove = true;
        }
        else if (DoCheckOnResultCards(Enums.ResultManager.holderOne, positionGetFromCards))
        {
            HaveMove = true;
        }
        else if (DoCheckOnResultCards(Enums.ResultManager.holderTwo, positionGetFromCards))
        {
            HaveMove = true;
        }
        else if (DoCheckOnResultCards(Enums.ResultManager.holderThree, positionGetFromCards))
        {
            HaveMove = true;
        }
        else if (DoCheckOnResultCards(Enums.ResultManager.holderFour, positionGetFromCards))
        {
            HaveMove = true;
        }

        if (HaveMove)
        {
            HudSystem.Instance.UpdateMove(++Contains.Moves);
            return;
        }

        /// Moving to the current position.
        MovingToPosition(targetPositionCards, false, () =>
        {
            if (parentHolder != null)
            {
                transform.SetParent(parentHolder);

                transform.SetSiblingIndex(LastSlidingIndex);

                DistributeTheFollowCards(false);

                DoErrorChoise();

                parentHolder = null;

                GamePlay.Instance.DoCheckWiningCondition();

                if (HidenCardsManager.Instance.IsExistAnyCards() == false)
                {
                    GamePlay.Instance.CheckIsConditionLose();
                }
            }
        });

        LogGame.DebugLog(string.Format("On End Drag - {0}", transform.name));
    }

    // ================== Update Holder ====================== //

    /// <summary>
    /// Updates the holder.
    /// </summary>
    /// <returns><c>true</c>, if holder was updated, <c>false</c> otherwise.</returns>
    /// <param name="holder">Holder.</param>
    /// <param name="cardType">Card type.</param>
    protected bool DoCheckOnResultCards(Enums.ResultManager holder, Vector3 positionCard)
    {
        bool IsCardOnHint = properties.IsCardOnHidenHolder();

        var PositionGet = ResultCardsManager.Instance.GetLastPositionInHolder(holder);

        var PositionCardRightUp = new Vector2(PositionGet.x + Contains.OffSetWidthCard / 2,
            PositionGet.y + Contains.OffSetHeightCard / 2);

        var PositionCardLeftDown = new Vector2(PositionGet.x - Contains.OffSetWidthCard / 2,
            PositionGet.y - Contains.OffSetHeightCard / 2);

        if (positionCard.x < PositionCardRightUp.x && positionCard.y < PositionCardRightUp.y &&
            positionCard.x > PositionCardLeftDown.x && positionCard.y > PositionCardLeftDown.y &&
            !ResultCardsManager.Instance.IsExistsThisCard(this, holder))
        {
            CardBehaviour cardGet = ResultCardsManager.Instance.GetLastCard(holder);

            if (cardGet == null && properties.GetDataProperties().GetEnumCardValue() == Enums.CardVariables.One ||
                cardGet != null && properties.IsBiggerThan(cardGet.GetDataCard()) &&
                !HelperCardsManager.Instance.IsExistsAnyCardsInTheHolder() &&
                ResultCardsManager.Instance.IsHaveSameTypeCard(holder, this))
            {
                if (IsCardOnHint)
                {
                    UndoSystem.Instance.RecordState(this, targetPositionCards, properties.IsCardOnHidenHolder(),
                        Enums.CardPlayingEnums.None, Enums.ResultManager.None, IsUnlocked(),
                        HidenCardsManager.Instance.IsHaveCardsOnShower(this), Contains.Score, false);

                    HidenCardsManager.Instance.OutOfHolder(this);

                    Helper.UpdateScore(Contains.ScoreResultCards);

                    HudSystem.Instance.UpdateScore(Contains.Score);
                }
                else
                {
                    Enums.ResultManager valueGet = ResultCardsManager.Instance.ReturnEnumHolder(this);

                    Enums.CardPlayingEnums valueGet_II = PlayingCardsManager.Instance.ReturnEnumHolder(this);

                    UndoSystem.Instance.RecordState(this, targetPositionCards, false, valueGet_II, valueGet,
                        IsUnlocked(), false, Contains.Score, false);

                    if (valueGet != Enums.ResultManager.None)
                    {
                        Helper.UpdateScore(Contains.ScoreMoveCards);

                        HudSystem.Instance.UpdateScore(Contains.Score);

                        ResultCardsManager.Instance.OutOfHolder(this);
                    }

                    if (valueGet_II != Enums.CardPlayingEnums.None)
                    {
                        Helper.UpdateScore(Contains.ScoreMoveCards);

                        HudSystem.Instance.UpdateScore(Contains.Score);

                        PlayingCardsManager.Instance.RemoveThisCard(this);
                    }
                }

                if (IsCardOnHint)
                {
                    HidenCardsManager.Instance.RefreshCards();
                }

                DoUpdateResultRegions(holder);

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Updates the holder.
    /// </summary>
    /// <returns><c>true</c>, if holder was updated, <c>false</c> otherwise.</returns>
    /// <param name="holder">Holder.</param>
    /// <param name="cardType">Card type.</param>
    protected bool DoCheckOnResultCards(Enums.ResultManager holder, bool IsFromHolderHint = false)
    {
        CardBehaviour cardGet = ResultCardsManager.Instance.GetLastCard(holder);

        if (!IsFromHolderHint)
        {
            if (!PlayingCardsManager.Instance.IsThisLastCard(this))
            {
                return false;
            }
        }

        if (cardGet == null && properties.GetDataProperties().GetEnumCardValue() == Enums.CardVariables.One ||
            cardGet != null && properties.IsBiggerThan(cardGet.GetDataCard()) &&
            !HelperCardsManager.Instance.IsExistsAnyCardsInTheHolder() &&
            ResultCardsManager.Instance.IsHaveSameTypeCard(holder, this))
        {
            if (IsFromHolderHint)
            {
                UndoSystem.Instance.RecordState(this, targetPositionCards, properties.IsCardOnHidenHolder(),
                    Enums.CardPlayingEnums.None, Enums.ResultManager.None, IsUnlocked(),
                    HidenCardsManager.Instance.IsHaveCardsOnShower(this), Contains.Score, false);

                HidenCardsManager.Instance.OutOfHolder(this);

                Helper.UpdateScore(Contains.ScoreResultCards);

                HudSystem.Instance.UpdateScore(Contains.Score);
            }
            else
            {
                Enums.ResultManager valueGet = ResultCardsManager.Instance.ReturnEnumHolder(this);

                Enums.CardPlayingEnums valueGet_II = PlayingCardsManager.Instance.ReturnEnumHolder(this);

                UndoSystem.Instance.RecordState(this, targetPositionCards, false, valueGet_II, valueGet, IsUnlocked(),
                    false, Contains.Score, false);

                if (valueGet != Enums.ResultManager.None)
                {
                    HudSystem.Instance.UpdateScore(Contains.Score);

                    ResultCardsManager.Instance.OutOfHolder(this);
                }

                if (valueGet_II != Enums.CardPlayingEnums.None)
                {
                    Helper.UpdateScore(Contains.ScoreMoveCards);

                    HudSystem.Instance.UpdateScore(Contains.Score);

                    PlayingCardsManager.Instance.RemoveThisCard(this);
                }
            }

            DoUpdateResultRegions(holder);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Updates the holder play.
    /// </summary>
    /// <returns><c>true</c>, if holder play was updated, <c>false</c> otherwise.</returns>
    /// <param name="holder">Holder.</param>
    /// <param name="positionCard">Position card.</param>
    protected bool DoCheckOnPlayingCards(Enums.CardPlayingEnums holder, Vector3 positionCard)
    {
        bool IsCardOnHint = properties.IsCardOnHidenHolder();

        var PositionGet = PlayingCardsManager.Instance.GetLastPositionInHolder(holder);

        var PositionCardRightUp = new Vector2(PositionGet.x + Contains.OffSetWidthCard / 2,
            PositionGet.y + Contains.OffSetHeightCard / 2);

        var PositionCardLeftDown = new Vector2(PositionGet.x - Contains.OffSetWidthCard / 2,
            PositionGet.y - Contains.OffSetHeightCard / 2);

        if (positionCard.x < PositionCardRightUp.x && positionCard.y < PositionCardRightUp.y &&
            positionCard.x > PositionCardLeftDown.x && positionCard.y > PositionCardLeftDown.y &&
            !PlayingCardsManager.Instance.IsExistsThisCard(this, holder))
        {
            CardBehaviour cardGet = PlayingCardsManager.Instance.GetLastCard(holder);

            if (cardGet == null && properties.GetDataProperties().GetEnumCardValue() == Enums.CardVariables.King ||
                cardGet != null && !properties.IsSameColorCard(cardGet.GetDataCard()) &&
                properties.IsSmallerThan(cardGet.GetDataCard()))
            {
                if (IsCardOnHint)
                {
                    UndoSystem.Instance.RecordState(this, targetPositionCards, properties.IsCardOnHidenHolder(),
                        Enums.CardPlayingEnums.None, Enums.ResultManager.None, IsUnlocked(),
                        HidenCardsManager.Instance.IsHaveCardsOnShower(this), Contains.Score, false);

                    HidenCardsManager.Instance.OutOfHolder(this);

                    Helper.UpdateScore(Contains.ScoreMoveCards);

                    HudSystem.Instance.UpdateScore(Contains.Score);
                }
                else
                {
                    Enums.ResultManager valueGet = ResultCardsManager.Instance.ReturnEnumHolder(this);

                    Enums.CardPlayingEnums valueGet_II = PlayingCardsManager.Instance.ReturnEnumHolder(this);

                    UndoSystem.Instance.RecordState(this, targetPositionCards, false, valueGet_II, valueGet,
                        IsUnlocked(), false, Contains.Score, false);

                    if (valueGet != Enums.ResultManager.None)
                    {
                        Helper.UpdateScore(-Contains.ScoreResultCards);

                        HudSystem.Instance.UpdateScore(Contains.Score);

                        ResultCardsManager.Instance.OutOfHolder(this);
                    }

                    if (valueGet_II != Enums.CardPlayingEnums.None)
                    {
                        HudSystem.Instance.UpdateScore(Contains.Score);

                        PlayingCardsManager.Instance.RemoveThisCard(this);
                    }
                }

                if (IsCardOnHint)
                {
                    HidenCardsManager.Instance.RefreshCards();
                }

                DoingUpdatePlayingRegions(holder);

                return true;
            }
        }

        return false;
    }

    protected bool DoCheckOnPlayingCards(Enums.CardPlayingEnums holder)
    {
        bool IsCardOnHint = properties.IsCardOnHidenHolder();

        CardBehaviour cardGet = PlayingCardsManager.Instance.GetLastCard(holder);

        if (cardGet == null && properties.GetDataProperties().GetEnumCardValue() == Enums.CardVariables.King ||
            cardGet != null && !properties.IsSameColorCard(cardGet.GetDataCard()) &&
            properties.IsSmallerThan(cardGet.GetDataCard()))
        {
            if (!IsCardOnHint)
            {
                Enums.CardPlayingEnums holderGet = PlayingCardsManager.Instance.ReturnEnumHolder(this);

                List<CardBehaviour> cardfound = PlayingCardsManager.Instance.GetCardAfterThis(holderGet, this);

                if (cardfound != null && cardfound.Count > 0)
                {
                    HelperCardsManager.Instance.cardsMemoryTemp.AddRange(cardfound);

                    for (int i = 0; i < cardfound.Count; i++)
                    {
                        cardfound[i].transform.SetParent(this.transform);
                    }
                }
            }

            if (IsCardOnHint)
            {
                UndoSystem.Instance.RecordState(this, targetPositionCards, properties.IsCardOnHidenHolder(),
                    Enums.CardPlayingEnums.None, Enums.ResultManager.None, IsUnlocked(),
                    HidenCardsManager.Instance.IsHaveCardsOnShower(this), Contains.Score, false);

                HidenCardsManager.Instance.OutOfHolder(this);

                Helper.UpdateScore(Contains.ScoreMoveCards);

                HudSystem.Instance.UpdateScore(Contains.Score);
            }
            else
            {
                Enums.ResultManager valueGet = ResultCardsManager.Instance.ReturnEnumHolder(this);

                Enums.CardPlayingEnums valueGet_II = PlayingCardsManager.Instance.ReturnEnumHolder(this);

                UndoSystem.Instance.RecordState(this, targetPositionCards, false, valueGet_II, valueGet, IsUnlocked(),
                    false, Contains.Score, false);

                if (valueGet != Enums.ResultManager.None)
                {
                    Helper.UpdateScore(-Contains.ScoreResultCards);

                    HudSystem.Instance.UpdateScore(Contains.Score);

                    ResultCardsManager.Instance.OutOfHolder(this);
                }

                if (valueGet_II != Enums.CardPlayingEnums.None)
                {
                    HudSystem.Instance.UpdateScore(Contains.Score);

                    PlayingCardsManager.Instance.RemoveThisCard(this);
                }
            }

            if (IsCardOnHint)
            {
                HidenCardsManager.Instance.RefreshCards();
            }

            DoingUpdatePlayingRegions(holder);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Updates to holder result.
    /// </summary>
    /// <param name="holder">Holder.</param>
    protected void DoUpdateResultRegions(Enums.ResultManager holder)
    {
        Transform transformGet = ResultCardsManager.Instance.GetHolderCards(holder);

        UpdateStateCard(Enums.CardBoard.CardUse);

        if (transformGet != null)
        {
            transform.SetParent(HelperCardsManager.Instance.GetTheTransformOfHolder());

            targetPositionCards = ResultCardsManager.Instance.GetLastPositionInHolder(holder);

            MovingToPosition(targetPositionCards, false, () =>
            {
                transform.SetParent(transformGet);

                transform.SetAsLastSibling();

                PlayingCardsManager.Instance.UnlockLastCards();

                SoundSystems.Instance.PlaySound(Enums.SoundIndex.Draw);

                GamePlay.Instance.DoCheckWiningCondition();

                if (HidenCardsManager.Instance.IsExistAnyCards() == false)
                {
                    GamePlay.Instance.CheckIsConditionLose();
                }
            });

            ResultCardsManager.Instance.UpdateNewCardToHolder(this, holder);

            transform.SetAsLastSibling();
        }
    }

    /// <summary>
    /// Updates to play holder.
    /// </summary>
    /// <param name="holder">Holder.</param>
    protected void DoingUpdatePlayingRegions(Enums.CardPlayingEnums holder)
    {
        Transform transformGet = PlayingCardsManager.Instance.GetParrentOfCards(holder);

        UpdateStateCard(Enums.CardBoard.CardUse);

        if (transformGet != null)
        {
            transform.SetParent(HelperCardsManager.Instance.GetTheTransformOfHolder());

            targetPositionCards = PlayingCardsManager.Instance.GetLastPositionInHolder(holder);

            MovingToPosition(targetPositionCards, false, () =>
            {
                transform.SetParent(transformGet);

                transform.SetAsLastSibling();

                DistributeTheFollowCards();

                PlayingCardsManager.Instance.UnlockLastCards();

                SoundSystems.Instance.PlaySound(Enums.SoundIndex.Draw);

                GamePlay.Instance.DoCheckWiningCondition();

                if (HidenCardsManager.Instance.IsExistAnyCards() == false)
                {
                    GamePlay.Instance.CheckIsConditionLose();
                }
            });

            PlayingCardsManager.Instance.UpdateNewCardToHolder(this, holder);

            transform.SetAsLastSibling();
        }
    }

    /// <summary>
    /// Raises the begin drag event.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        IsDrag = true;

        if (!properties.IsCardUnlocked)
            return;

        if (GameManager.Instance.IsGameReady() == false)
            return;

        if (properties.IsCardOnHidenHolder())
        {
            if (!HidenCardsManager.Instance.IsThatLastCardsShowing(this))
            {
                return;
            }
        }

        transform.DOComplete(true);

        GamePlay.Instance.DisableHintGame();

        // HudSystem.Instance.UpdateMove(++Contains.Moves);

        pointer = Helper.GetWorldPosition();

        pointer.x = transform.position.x - pointer.x;

        pointer.y = transform.position.y - pointer.y;

        pointer.z = 0;

        parentHolder = transform.parent;

        LastSlidingIndex = transform.GetSiblingIndex();

        transform.SetParent(HelperCardsManager.Instance.GetTheTransformOfHolder());

        transform.SetAsLastSibling();

        Enums.CardPlayingEnums holderGet = PlayingCardsManager.Instance.ReturnEnumHolder(this);

        if (holderGet != Enums.CardPlayingEnums.None)
        {
            List<CardBehaviour> cardsGet =
                new List<CardBehaviour>(PlayingCardsManager.Instance.GetCardAfterThis(holderGet, this));

            HelperCardsManager.Instance.cardsMemoryTemp.AddRange(cardsGet);

            for (int i = 0; i < cardsGet.Count; i++)
            {
                cardsGet[i].transform.SetParent(this.transform);
            }
        }

        //EnableOutLine ();

        LogGame.DebugLog(string.Format("On Begin Drag - {0}", transform.name));
    }

    #endregion

    #region Helper

    /// <summary>
    /// Updates the state card.
    /// </summary>
    /// <param name="cardState">Card state.</param>
    public void UpdateStateCard(Enums.CardBoard cardState)
    {
        properties.UpdateStateCard(cardState);
    }

    /// <summary>
    /// Determines whether this instance is unlocked.
    /// </summary>
    /// <returns><c>true</c> if this instance is unlocked; otherwise, <c>false</c>.</returns>
    public bool IsUnlocked()
    {
        return properties.IsCardUnlocked;
    }

    /// <summary>
    /// Determines whether this instance is card use for playing.
    /// </summary>
    /// <returns><c>true</c> if this instance is card use for playing; otherwise, <c>false</c> card will use for help.</returns>
    public bool IsCardUseForPlaying()
    {
        return properties.IsCardOnBoard();
    }

    /// <summary>
    /// Moves to.
    /// </summary>
    /// <param name="position">Position.</param>
    /// <param name="isLocalMove">If set to <c>true</c> is local move.</param>
    /// <param name="OnCompletedMoving">On completed moving.</param>
    public void MovingToPosition(Vector3 position, bool isLocalMove = false, System.Action OnCompletedMoving = null)
    {
        transform.DOComplete(true);

        if (isLocalMove)
        {
            transform.DOLocalMove(position, Contains.DurationMoving).OnComplete(() =>
            {
                if (OnCompletedMoving != null)
                {
                    OnCompletedMoving();
                }
            });
        }
        else
        {
            transform.DOMove(position, Contains.DurationMoving).OnComplete(() =>
            {
                if (OnCompletedMoving != null)
                {
                    OnCompletedMoving();
                }
            });
        }
    }

    /// <summary>
    /// Raises the pointer click event.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!IsUnlocked())
        {
            return;
        }

        if (!GameManager.Instance.IsGameReady())
        {
            return;
        }

        if (IsDrag)
        {
            IsDrag = false;

            return;
        }

        if (properties.IsCardOnHidenHolder())
        {
            if (!HidenCardsManager.Instance.IsThatLastCardsShowing(this))
            {
                return;
            }
        }

        transform.DOComplete(true);

        parentHolder = transform.parent;

        LastSlidingIndex = transform.GetSiblingIndex();

        GamePlay.Instance.DisableHintGame();

        // Check move to completed card

        bool HaveMove = false;
        bool IsCardOnHint = properties.IsCardOnHidenHolder();

        if (DoCheckOnResultCards(Enums.ResultManager.holderOne, IsCardOnHint))
        {
            // HaveMove = true;
            if (IsCardOnHint)
            {
                HidenCardsManager.Instance.RefreshCards();
            }

            PlayingCardsManager.Instance.UnlockLastCards();

            HaveMove = true;
        }
        else if (DoCheckOnResultCards(Enums.ResultManager.holderTwo, IsCardOnHint))
        {
            if (IsCardOnHint)
            {
                HidenCardsManager.Instance.RefreshCards();
            }

            PlayingCardsManager.Instance.UnlockLastCards();

            HaveMove = true;
        }
        else if (DoCheckOnResultCards(Enums.ResultManager.holderThree, IsCardOnHint))
        {
            if (IsCardOnHint)
            {
                HidenCardsManager.Instance.RefreshCards();
            }

            PlayingCardsManager.Instance.UnlockLastCards();

            HaveMove = true;
        }
        else if (DoCheckOnResultCards(Enums.ResultManager.holderFour, IsCardOnHint))
        {
            if (IsCardOnHint)
            {
                HidenCardsManager.Instance.RefreshCards();
            }

            PlayingCardsManager.Instance.UnlockLastCards();

            HaveMove = true;
        }
        else if (IsCardOnHint || !ResultCardsManager.Instance.IsExistsThisCard(this, Enums.ResultManager.holderOne) &&
                 !ResultCardsManager.Instance.IsExistsThisCard(this, Enums.ResultManager.holderTwo) &&
                 !ResultCardsManager.Instance.IsExistsThisCard(this, Enums.ResultManager.holderThree) &&
                 !ResultCardsManager.Instance.IsExistsThisCard(this, Enums.ResultManager.holderFour))
        {
            if (DoCheckOnPlayingCards(Enums.CardPlayingEnums.holderOne))
            {
                HaveMove = true;
            }
            else if (DoCheckOnPlayingCards(Enums.CardPlayingEnums.holderTwo))
            {
                HaveMove = true;
            }
            else if (DoCheckOnPlayingCards(Enums.CardPlayingEnums.holderThree))
            {
                HaveMove = true;
            }
            else if (DoCheckOnPlayingCards(Enums.CardPlayingEnums.holderFour))
            {
                HaveMove = true;
            }
            else if (DoCheckOnPlayingCards(Enums.CardPlayingEnums.holderFive))
            {
                HaveMove = true;
            }
            else if (DoCheckOnPlayingCards(Enums.CardPlayingEnums.holderSix))
            {
                HaveMove = true;
            }
            else if (DoCheckOnPlayingCards(Enums.CardPlayingEnums.holderSeven))
            {
                HaveMove = true;
            }
        }

        if (HaveMove)
        {
            HudSystem.Instance.UpdateMove(++Contains.Moves);
            return;
        }

        // Check move to play board

        DoErrorChoise();
    }

    /// <summary>
    /// Gets the data card.
    /// </summary>
    /// <returns>The data card.</returns>
    public CardDataProperties GetDataCard()
    {
        return properties.GetDataProperties();
    }

    public void UpdateStyle(CardDataProperties card)
    {
        Init(card, IsCardUseForPlaying() ? Enums.CardBoard.CardUse : Enums.CardBoard.CardHint, IsUnlocked());

        UnlockCard(IsUnlocked());
    }

    /// <summary>
    /// Updates the data card.
    /// </summary>
    /// <param name="param">Parameter.</param>
    public void UpdateDataCard(CardProperties param)
    {
        properties = param;
    }

    /// <summary>
    /// Gets the properties.
    /// </summary>
    /// <returns>The properties.</returns>
    public CardProperties GetProperties()
    {
        return properties;
    }

    public void DistributeTheFollowCards(bool resetPosition = true)
    {
        if (!HelperCardsManager.Instance.IsExistsAnyCardsInTheHolder())
        {
            return;
        }

        CardBehaviour cardGet;

        Enums.CardPlayingEnums holderGet = PlayingCardsManager.Instance.ReturnEnumHolder(this);

        for (int i = 0; i < HelperCardsManager.Instance.cardsMemoryTemp.Count; i++)
        {
            cardGet = HelperCardsManager.Instance.cardsMemoryTemp[i];

            if (resetPosition)
            {
                cardGet.targetPositionCards = PlayingCardsManager.Instance.GetLastPositionInHolder(holderGet);
            }

            PlayingCardsManager.Instance.RemoveThisCard(cardGet);

            cardGet.transform.SetParent(transform.parent);

            PlayingCardsManager.Instance.UpdateNewCardToHolder(cardGet, holderGet);
        }

        HelperCardsManager.Instance.RefreshTheMemory();
    }

    public bool DoUpdateResultRegions()
    {
        if (DoCheckOnResultCards(Enums.ResultManager.holderOne))
        {
            return true;
        }

        if (DoCheckOnResultCards(Enums.ResultManager.holderTwo))
        {
            return true;
        }

        if (DoCheckOnResultCards(Enums.ResultManager.holderThree))
        {
            return true;
        }

        if (DoCheckOnResultCards(Enums.ResultManager.holderFour))
        {
            return true;
        }

        return false;
    }

    #endregion

    #region Animation

    /// <summary>
    /// Disables the animation high light.
    /// </summary>
    public void DisableOutline()
    {
        HintCardsManager.Instance.DisableHint();
    }

    /// <summary>
    /// Error Choise
    /// </summary>
    public void DoErrorChoise()
    {
        this.animation.SetBool(IsRotation, true);

        CancelInvoke("TurnOffErrorNotification");

        Invoke("TurnOffErrorNotification", 0.05f);
    }

    protected void TurnOffErrorNotification()
    {
        this.animation.SetBool(IsRotation, false);
    }

    #endregion
}