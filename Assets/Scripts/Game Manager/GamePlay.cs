using System;
using MovementEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using DG.Tweening;
using Random = UnityEngine.Random;

public class GamePlay : SingletonMonoBehaviour<GamePlay>
{
    // ========================== References ======================= //

    #region References

    /// <summary>
    /// The blur.
    /// </summary>
    [SerializeField] private Blur blur;

    #endregion

    #region Variables

    /// <summary>
    /// The handle.
    /// </summary>
    protected List<CoroutineHandle> handle = new List<CoroutineHandle>();

    /// <summary>
    /// The card get.
    /// </summary>
    protected List<CardBehaviour> cardGet;

    /// <summary>
    /// The card locked.
    /// </summary>
    protected List<CardBehaviour> CardLocked = new List<CardBehaviour>();

    /// <summary>
    /// The hint cards.
    /// </summary>
    List<CardBehaviour> hintCards;

    /// <summary>
    /// The lasted hint cards.
    /// </summary>
    [HideInInspector] public List<CardBehaviour> TheLastedHintCards = new List<CardBehaviour>();

    #endregion

    #region Handle

    /// <summary>
    /// The wining handle.
    /// </summary>
    protected CoroutineHandle winingHandle;

    /// <summary>
    /// The lost handle.
    /// </summary>
    protected CoroutineHandle lostHandle;

    /// <summary>
    /// The is show hint.
    /// </summary>
    protected bool IsShowHint;

    /// <summary>
    /// The card identifier.
    /// </summary>
    public Enums.Themes GameThemes;

    #endregion


    // ========================= Functional ======================== //

    #region Functional

    /// <summary>
    /// Awake this instance.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        if ((GameManager.Instance.PlayMode == Enums.PlayMode.Normal && GameManager.Instance.stage == 1) ||
            GameManager.Instance.PlayMode == Enums.PlayMode.Daily ||
            GameManager.Instance.PlayMode == Enums.PlayMode.Event) Contains.Score = 0;
        if(GameManager.Instance.stage > 1) HelperCardsManager.Instance.ShowStage(GameManager.Instance.stage);

        Contains.Moves = 0;

        GameManager.Instance.UpdateState(Enums.StateGame.Waiting);
        
        HudSystem.Instance.StartGame();

        if (PoolSystem.Instance.GetNumberOfCards() < Contains.NumberCards)
        {
            PoolSystem.Instance.ClearCards();

            GameManager.Instance.InitCards();
        }

        CardLocked.Clear();

        cardGet = new List<CardBehaviour>(PoolSystem.Instance.GetAllCards());

        if (cardGet == null || cardGet.Count == 0)
        {
            throw new System.Exception("Can not play this game.");
        }

        if (MutilResolution.Instance.IsPortraitView)
        {
            UpdatePortrait();
        }
        else
        {
            UpdateLandscape();
        }

        GameThemes = Contains.CurrentStyle;

        ChangeStyleCards(GameThemes);

        int lenght = cardGet.Count;

        CardBehaviour card;

        for (int i = 0; i < lenght; i++)
        {
            card = cardGet[i];

            if (card != null)
            {
                card.UnlockCard(false);

                card.UpdateStateCard(Enums.CardBoard.CardUse);

                card.gameObject.SetActive(true);

                card.DisableOutline();
            }
            else
            {
                LogGame.DebugLog(string.Format("[Game Play] This Card Was Null"));
            }
        }

        // GameManager.Instance.ModeGame = Enums.ModeGame.None;

        cardGet = Helper.SortRandom(cardGet);

        HidenCardsManager.Instance.UpdateCards(cardGet);

        handle.Add(Timing.RunCoroutine(PrepareInitGame()));

        //AdSystem.Instance.ResetInterstitial ();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ShowWin();
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            ShowLose();
        }
    }

    [Header("Landscape")]
    /// <summary>
    /// The landscape right transform.
    /// </summary>
    public Transform LandscapeRightTransform;

    /// <summary>
    /// The landscape left transform.
    /// </summary>
    public Transform LandscapeLeftTransform;

    [Header("Portrait")]
    /// <summary>
    /// The portrait left transform.
    /// </summary>
    public Transform PortraitLeftTransform;

    /// <summary>
    /// The portrait right transform.
    /// </summary>
    public Transform PortraitRightTransform;

    /// <summary>
    /// Updates the portrait.
    /// </summary>
    public void UpdatePortrait()
    {
        Timing.PauseCoroutines();

        // Stop all the cards are moving.
        for (int i = 0; i < cardGet.Count; i++)
        {
            cardGet[i].transform.DOKill(true);
        }

        // Check the condition to update view of cards.
        if (Contains.IsRightHanded)
        {
            // Disable Portrait Left Transform.
            PortraitLeftTransform.gameObject.SetActive(false);

            // Enable Portrait Right Transform.
            PortraitRightTransform.gameObject.SetActive(true);

            // Disable Landscape Left Transform.
            LandscapeLeftTransform.gameObject.SetActive(false);

            // Disable Landscape Right Transform.
            LandscapeRightTransform.gameObject.SetActive(false);

            // Update UI Portrait view of Playing Cards.
            PlayingCardsManager.Instance.UpdatePortraitRight();

            // Update UI Portrait view of Result Cards.
            ResultCardsManager.Instance.UpdatePortraitOnRight();

            // Update UI Portrait view of Hidden Cards.
            HidenCardsManager.Instance.UpdatePortraitOnRight();
        }
        else
        {
            // Enable Portrait Left Transform.
            PortraitLeftTransform.gameObject.SetActive(true);

            // Disable Portrait Right Transform.
            PortraitRightTransform.gameObject.SetActive(false);

            // Disable Landscape Left Transform.
            LandscapeLeftTransform.gameObject.SetActive(false);

            // Disable Landscape Right Transform.
            LandscapeRightTransform.gameObject.SetActive(false);

            // Update UI Portrait view of Playing Cards.
            PlayingCardsManager.Instance.UpdatePortraitLeft();

            // Update UI Portrait view of Result Cards.
            ResultCardsManager.Instance.UpdatePortraitOnLeft();

            // Update UI Portrait view of Hidden Cards.
            HidenCardsManager.Instance.UpdatePortraitOnLeft();
        }

        // Cancel the current invoke with sort cards.
        CancelInvoke("ResortCards");

        // Invoke sort cards.
        Invoke("ResortCards", Time.deltaTime);
    }

    /// <summary>
    /// Updates the landscape.
    /// </summary>
    public void UpdateLandscape()
    {
        Timing.PauseCoroutines();

        // Stop all the cards are moving.
        for (int i = 0; i < cardGet.Count; i++)
        {
            cardGet[i].transform.DOKill(true);
        }

        // Check the condition to update view of cards.
        if (Contains.IsRightHanded)
        {
            // Disable Portrait Left Transform.
            PortraitLeftTransform.gameObject.SetActive(false);

            // Disable Portrait Right Transform.
            PortraitRightTransform.gameObject.SetActive(false);

            // Disable Landscape Left Transform.
            LandscapeLeftTransform.gameObject.SetActive(false);

            // Enable Landscape Right Transform.
            LandscapeRightTransform.gameObject.SetActive(true);

            // Update UI Landscape view of Playing Cards.
            PlayingCardsManager.Instance.UpdateLandscapeRight();

            // Update UI Landscape view of Result Cards.
            ResultCardsManager.Instance.UpdateLandscapeOnRight();

            // Update UI Landscape view of Hidden Cards.
            HidenCardsManager.Instance.UpdateLandscapeOnRight();
        }
        else
        {
            // Disable Portrait Left Transform.
            PortraitLeftTransform.gameObject.SetActive(false);

            // Disable Portrait Right Transform.
            PortraitRightTransform.gameObject.SetActive(false);

            // Enable Landscape Left Transform.
            LandscapeLeftTransform.gameObject.SetActive(true);

            // Disable Landscape Right Transform.
            LandscapeRightTransform.gameObject.SetActive(false);

            // Update UI Landscape view of Playing Cards.
            PlayingCardsManager.Instance.UpdateLandscapeLeft();

            // Update UI Landscape view of Result Cards.
            ResultCardsManager.Instance.UpdateLandscapeOnLeft();

            // Update UI Landscape view of Hidden Cards.
            HidenCardsManager.Instance.UpdateLandscapeOnLeft();
        }

        // Cancel the current invoke with sort cards.
        CancelInvoke("ResortCards");

        // Invoke sort cards.
        Invoke("ResortCards", Time.deltaTime);
    }

    protected void ResortCards()
    {
        // Sort the cards after update new UI.
        PlayingCardsManager.Instance.SortCards();

        // Sort the cards after update new UI.
        ResultCardsManager.Instance.SortCards();

        // Sort the cards after update new UI.
        HidenCardsManager.Instance.RefreshCards();

        // Disable Hint Cards.
        DisableHintGame();

        Timing.ResumeCoroutines();

        //AdSystem.Instance.DestroyAndRequestBanner ();
    }

    /// <summary>
    /// Changes the style cards.
    /// </summary>
    /// <param name="cardStyle">Card style.</param>
    public void ChangeStyleCards(Enums.Themes cardStyle)
    {
        Contains.CurrentStyle = cardStyle;

        GameThemes = cardStyle;

        HudSystem.Instance.UpdateSprite(cardStyle);

        List<CardDataProperties> getDataCards =
            new List<CardDataProperties>(DataSystem.Instance.GetCardsData((int)cardStyle));

        if (getDataCards != null || getDataCards.Count > 0)
        {
            for (int i = 0; i < cardGet.Count; i++)
            {
                int cardValue = cardGet[i].GetDataCard().GetCardValue();

                Enums.CardType cardType = cardGet[i].GetDataCard().GetCardType();

                for (int j = 0; j < getDataCards.Count; j++)
                {
                    if (getDataCards[j].GetCardType() == cardType && getDataCards[j].GetCardValue() == cardValue)
                    {
                        cardGet[i].UpdateStyle(getDataCards[j]);

                        getDataCards.RemoveAt(j);

                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Prepares the init game.
    /// </summary>
    /// <returns>The init game.</returns>
    IEnumerator<float> PrepareInitGame()
    {
        // Introdution the Game.

        // CoroutineHandle handleRuntime = Timing.RunCoroutine(Introdution());
        //
        // handle.Add(handleRuntime);
        //
        yield return Timing.WaitForSeconds(0.5f);

        // Distribute the Cards.

        CoroutineHandle handleRuntime = Timing.RunCoroutine(DistributeTheCards());

        handle.Add(handleRuntime);

        yield return Timing.WaitUntilDone(handleRuntime);

        // Starting the Game

        handleRuntime = Timing.RunCoroutine(StartTheGame());

        handle.Add(handleRuntime);

        yield return Timing.WaitUntilDone(handleRuntime);
    }

    #endregion

    #region Animation

    /// <summary>
    /// Introdution this instance.
    /// </summary>
    IEnumerator<float> Introdution()
    {
        SoundSystems.Instance.PlayerMusic(Enums.MusicIndex.StartMusic, false);

        while (GameManager.Instance.ModeGame == Enums.ModeGame.None)
        {
            yield return 0f;
        }

        yield return Timing.WaitForSeconds(Contains.DurationPreview);
    }

    /// <summary>
    /// Distributes the cards.
    /// </summary>
    /// <returns>The the cards.</returns>
    IEnumerator<float> DistributeTheCards()
    {
        int increase = 0;

        CardBehaviour cardFound = null;

        for (int i = 0; i < Contains.MaximumHolderCards; i++)
        {
            float waitingTime = Contains.DurationDraw / 2;

            if (increase < 1)
            {
                cardFound = HidenCardsManager.Instance.GetLastCardHolder();

                if (cardFound != null)
                {
                    Transform transformGet =
                        PlayingCardsManager.Instance.GetParrentOfCards(Enums.CardPlayingEnums.holderOne);

                    if (transformGet != null)
                    {
                        InstatiateTheCard(Enums.CardPlayingEnums.holderOne, cardFound, transformGet, increase, 0);

                        SoundSystems.Instance.PlaySound(Enums.SoundIndex.Draw);

                        yield return Timing.WaitForSeconds(waitingTime);
                    }
                }

                yield return Timing.WaitForSeconds(waitingTime);
            }

            if (increase < 2)
            {
                cardFound = HidenCardsManager.Instance.GetLastCardHolder();

                if (cardFound != null)
                {
                    Transform transformGet =
                        PlayingCardsManager.Instance.GetParrentOfCards(Enums.CardPlayingEnums.holderTwo);

                    if (transformGet != null)
                    {
                        InstatiateTheCard(Enums.CardPlayingEnums.holderTwo, cardFound, transformGet, increase, 1);

                        SoundSystems.Instance.PlaySound(Enums.SoundIndex.Draw);

                        yield return Timing.WaitForSeconds(waitingTime);
                    }
                }

                yield return Timing.WaitForSeconds(waitingTime);
            }

            if (increase < 3)
            {
                cardFound = HidenCardsManager.Instance.GetLastCardHolder();

                if (cardFound != null)
                {
                    Transform transformGet =
                        PlayingCardsManager.Instance.GetParrentOfCards(Enums.CardPlayingEnums.holderThree);

                    if (transformGet != null)
                    {
                        InstatiateTheCard(Enums.CardPlayingEnums.holderThree, cardFound, transformGet, increase, 2);

                        SoundSystems.Instance.PlaySound(Enums.SoundIndex.Draw);

                        yield return Timing.WaitForSeconds(waitingTime);
                    }
                }

                yield return Timing.WaitForSeconds(waitingTime);
            }

            if (increase < 4)
            {
                cardFound = HidenCardsManager.Instance.GetLastCardHolder();

                if (cardFound != null)
                {
                    Transform transformGet =
                        PlayingCardsManager.Instance.GetParrentOfCards(Enums.CardPlayingEnums.holderFour);

                    if (transformGet != null)
                    {
                        InstatiateTheCard(Enums.CardPlayingEnums.holderFour, cardFound, transformGet, increase, 3);

                        SoundSystems.Instance.PlaySound(Enums.SoundIndex.Draw);

                        yield return Timing.WaitForSeconds(waitingTime);
                    }
                }

                yield return Timing.WaitForSeconds(waitingTime);
            }

            if (increase < 5)
            {
                cardFound = HidenCardsManager.Instance.GetLastCardHolder();

                if (cardFound != null)
                {
                    Transform transformGet =
                        PlayingCardsManager.Instance.GetParrentOfCards(Enums.CardPlayingEnums.holderFive);

                    if (transformGet != null)
                    {
                        InstatiateTheCard(Enums.CardPlayingEnums.holderFive, cardFound, transformGet, increase, 4);

                        SoundSystems.Instance.PlaySound(Enums.SoundIndex.Draw);

                        yield return Timing.WaitForSeconds(waitingTime);
                    }
                }

                yield return Timing.WaitForSeconds(waitingTime);
            }

            if (increase < 6)
            {
                cardFound = HidenCardsManager.Instance.GetLastCardHolder();

                if (cardFound != null)
                {
                    Transform transformGet =
                        PlayingCardsManager.Instance.GetParrentOfCards(Enums.CardPlayingEnums.holderSix);

                    if (transformGet != null)
                    {
                        InstatiateTheCard(Enums.CardPlayingEnums.holderSix, cardFound, transformGet, increase, 5);

                        SoundSystems.Instance.PlaySound(Enums.SoundIndex.Draw);

                        yield return Timing.WaitForSeconds(waitingTime);
                    }
                }

                yield return Timing.WaitForSeconds(waitingTime);
            }

            if (increase < 7)
            {
                cardFound = HidenCardsManager.Instance.GetLastCardHolder();

                if (cardFound != null)
                {
                    Transform transformGet =
                        PlayingCardsManager.Instance.GetParrentOfCards(Enums.CardPlayingEnums.holderSeven);

                    if (transformGet != null)
                    {
                        InstatiateTheCard(Enums.CardPlayingEnums.holderSeven, cardFound, transformGet, increase, 6);

                        SoundSystems.Instance.PlaySound(Enums.SoundIndex.Draw);

                        yield return Timing.WaitForSeconds(waitingTime);
                    }
                }

                yield return Timing.WaitForSeconds(waitingTime);
            }

            increase++;
        }

        yield return 0f;

        HidenCardsManager.Instance.RefreshCards();
    }

    /// <summary>
    /// Starts the game.
    /// </summary>
    /// <returns>The the game.</returns>
    IEnumerator<float> StartTheGame()
    {
        // Playing the music.

        SoundSystems.Instance.PlayerMusic(
            (Enums.MusicIndex)Random.Range((int)Enums.MusicIndex.Background_I,
                (int)Enums.MusicIndex.Background_III + 1), true);

        // Reset position of cards.

        HidenCardsManager.Instance.Reset();

        // Update the state of game.

        GameManager.Instance.UpdateState(Enums.StateGame.Playing);

        yield return 0f;
    }

    #endregion

    #region Helper

    /// <summary>
    /// Stops all coroutine.
    /// </summary>
    public void StopAllCoroutine()
    {
        // Killing all the hanlde in this session.

        for (int i = 0; i < handle.Count; i++)
        {
            if (handle != null)
            {
                Timing.KillCoroutines(handle[i]);
            }
        }

        // Killing wining session.
        if (winingHandle != null)
        {
            Timing.KillCoroutines(winingHandle);
        }

        // killing lose session.

        if (lostHandle != null)
        {
            Timing.KillCoroutines(lostHandle);
        }
    }

    #endregion

    #region Update

    /// <summary>
    /// Instatiates the card.
    /// </summary>
    /// <param name="holder">Holder.</param>
    /// <param name="cardFound">Card found.</param>
    /// <param name="transformGet">Transform get.</param>
    /// <param name="increase">Increase.</param>
    public void InstatiateTheCard(Enums.CardPlayingEnums holder, CardBehaviour cardFound, Transform transformGet,
        int increase, int indexCardUnlock)
    {
        // Set parent of cards to holder.

        cardFound.transform.SetParent(HelperCardsManager.Instance.GetTheTransformOfHolder());

        // set card waiting.

        CardBehaviour cardWaiting = cardFound;

        // Reset position cards are going to move.

        cardFound.targetPositionCards = PlayingCardsManager.Instance.GetLastPositionInHolder(holder);

        // Moving this cards to the position.

        cardFound.MovingToPosition(cardFound.targetPositionCards, false, () =>
        {
            cardWaiting.transform.SetParent(transformGet);

            cardWaiting.transform.SetAsLastSibling();
        });

        // Update this cards to playing holder.

        PlayingCardsManager.Instance.UpdateNewCardToHolder(cardFound, holder);

        // Reset index of cards in list transform.

        cardFound.transform.SetAsLastSibling();

        if (increase == indexCardUnlock)
        {
            cardFound.UnlockCard(true);
        }
    }

    /// <summary>
    /// Updateds the locked card.
    /// </summary>
    /// <param name="card">Card.</param>
    public void UpdatedLockedCard(CardBehaviour card)
    {
        if (CardLocked.Contains(card))
        {
            return;
        }

        CardLocked.Add(card);
    }

    /// <summary>
    /// Updates the unlocked card.
    /// </summary>
    /// <param name="card">Card.</param>
    public void UpdateUnlockedCard(CardBehaviour card)
    {
        if (CardLocked.Contains(card))
        {
            CardLocked.Remove(card);
        }
    }

    public void ShowHintGame()
    {
        // Hint On Game Holder Play
        if (IsShowHint)
            return;

        IsShowHint = true;

        hintCards = PlayingCardsManager.Instance.GetHintCards();

        if (hintCards != null && hintCards.Count > 0)
        {
            bool IsDifferentCards = false;

            for (int i = 0; i < hintCards.Count; i++)
            {
                if (!TheLastedHintCards.Contains(hintCards[i]))
                {
                    IsDifferentCards = true;
                }
            }

            if (hintCards.Count > 1)
            {
                HintCardsManager.Instance.ShowHint(hintCards[0].transform.position, hintCards[1].transform.position,
                    hintCards[0].GetProperties().GetDataProperties().GetCardSprite());
            }
            else if (hintCards.Count == 1)
            {
                HintCardsManager.Instance.ShowHint(hintCards[0].transform.position, hintCards[0].transform.position,
                    hintCards[0].GetProperties().GetDataProperties().GetCardSprite());
            }

            if (IsDifferentCards)
            {
                TheLastedHintCards.Clear();

                TheLastedHintCards.AddRange(hintCards);
            }

            return;
        }

        // Hint On Game Holder Hint

        hintCards = HidenCardsManager.Instance.GetHintCards();

        if (hintCards != null && hintCards.Count > 0)
        {
            bool IsDifferentCards = false;

            CardBehaviour LastCard = null;

            for (int i = 0; i < hintCards.Count; i++)
            {
                if (HidenCardsManager.Instance.IsLastedCard(hintCards[i]))
                {
                    LastCard = hintCards[i];
                }


                if (!TheLastedHintCards.Contains(hintCards[i]))
                {
                    IsDifferentCards = true;
                }
            }

            if (LastCard != null)
            {
                if (hintCards.Count > 1)
                {
                    HintCardsManager.Instance.ShowHint(hintCards[0].transform.position, hintCards[1].transform.position,
                        hintCards[0].GetDataCard().GetCardSprite());
                }
                else if (hintCards.Count == 1)
                {
                    HintCardsManager.Instance.ShowHint(hintCards[0].transform.position, hintCards[0].transform.position,
                        hintCards[0].GetDataCard().GetCardSprite());
                }
            }
            else
            {
                HidenCardsManager.Instance.EnableHintAnimation();
            }


            if (IsDifferentCards)
            {
                TheLastedHintCards.Clear();

                TheLastedHintCards.AddRange(hintCards);
            }

            return;
        }

        if (HidenCardsManager.Instance.IsHaveLockedCards())
        {
            HidenCardsManager.Instance.EnableHintAnimation();

            return;
        }

        IsShowHint = false;
    }

    /// <summary>
    /// Disables the hint.
    /// </summary>
    public void DisableHintGame()
    {
        if (hintCards != null && hintCards.Count > 0)
        {
            for (int i = 0; i < hintCards.Count; i++)
            {
                hintCards[i].DisableOutline();
            }

            hintCards.Clear();
        }

        HidenCardsManager.Instance.DisableHintAnimation();

        IsShowHint = false;
    }

    public bool IsCanotMove()
    {
        hintCards = PlayingCardsManager.Instance.GetHintCards();

        if (hintCards != null && hintCards.Count > 0)
        {
            return false;
        }

        // Hint On Game Holder Hint

        hintCards = HidenCardsManager.Instance.GetHintCards();

        if (hintCards != null && hintCards.Count > 0)
        {
            return false;
        }

        if (HidenCardsManager.Instance.IsHaveLockedCards())
        {
            return false;
        }

        if (PlayingCardsManager.Instance.IsHaveEmptySpace())
        {
            return false;
        }

        return true;
    }

    #endregion

    #region Condition

    /// <summary>
    /// Checks the is condition window.
    /// </summary>
    public void DoCheckWiningCondition()
    {
        if (GameManager.Instance.IsGameEnd())
        {
            return;
        }

        if (IsUnlockedAllCards())
        {
            if (winingHandle != null)
            {
                Timing.KillCoroutines(winingHandle);
            }

            winingHandle = Timing.RunCoroutine(DoWining());
        }
    }

    /// <summary>
    /// Checks the is condition lose.
    /// </summary>
    public void CheckIsConditionLose()
    {
        if (GameManager.Instance.IsGameEnd() || !GameManager.Instance.IsGameReady())
        {
            return;
        }

        if (lostHandle != null)
        {
            Timing.KillCoroutines(lostHandle);
        }

        lostHandle = Timing.RunCoroutine(DoLose());
    }

    /// <summary>
    /// Determines whether this instance is unlocked all cards.
    /// </summary>
    /// <returns><c>true</c> if this instance is unlocked all cards; otherwise, <c>false</c>.</returns>
    public bool IsUnlockedAllCards()
    {
        return CardLocked.Count == 0 && !HidenCardsManager.Instance.IsExistAnyCards();
    }

    /// <summary>
    /// Dos the wining.
    /// </summary>
    /// <returns>The wining.</returns>
    IEnumerator<float> DoWining()
    {
        GameManager.Instance.UpdateState(Enums.StateGame.Win);
        if (GameManager.Instance.PlayMode == Enums.PlayMode.Daily)
        {
            HomeSceneController.Instance.OnDailyChallengeWin();
        }
        else if (GameManager.Instance.PlayMode == Enums.PlayMode.Event)
        {
            HomeSceneController.Instance.OnEventChallengeWin();
        }

        while (PlayingCardsManager.Instance.IsHaveCards())
        {
            if (PlayingCardsManager.Instance.CollectCardsToResultHolder())
            {
                yield return Timing.WaitForSeconds(Contains.DurationDraw);
            }
            else
            {
                yield return Timing.WaitForOneFrame;
            }
        }

        yield return Timing.WaitForSeconds(1f);

        if (GameManager.Instance.PlayMode == Enums.PlayMode.Daily || GameManager.Instance.PlayMode == Enums.PlayMode.Event)
        {
            HudSystem.Instance.EnableWin();
        }
        else
        {
            GameManager.Instance.stage++;
            HudSystem.Instance.RestartGame();
        }

        yield return 0f;
    }

    public void ShowWin()
    {
        if (!GameManager.Instance.IsGameReady()) return;
        GameManager.Instance.UpdateState(Enums.StateGame.Win);
        if (GameManager.Instance.PlayMode == Enums.PlayMode.Daily)
        {
            HomeSceneController.Instance.OnDailyChallengeWin();
            HudSystem.Instance.EnableWin();
        }
        else if (GameManager.Instance.PlayMode == Enums.PlayMode.Event)
        {
            HomeSceneController.Instance.OnEventChallengeWin();
            HudSystem.Instance.EnableWin();
        }
        else
        {
            GameManager.Instance.stage++;
            // HudSystem.Instance.ReadyRestart();
            HudSystem.Instance.RestartGame();
        }
    }

    /// <summary>
    /// Dos the lose.
    /// </summary>
    /// <returns>The lose.</returns>
    IEnumerator<float> DoLose()
    {
        bool IsLose = IsCanotMove();

        if (IsLose)
        {
            HudSystem.Instance.EnableLose();

            GameManager.Instance.UpdateState(Enums.StateGame.Lose);

            yield return 0f;

            LogGame.DebugLog("Game Over.");
        }
    }

    public void ShowLose()
    {
        if (!GameManager.Instance.IsGameReady()) return;
        HudSystem.Instance.EnableLose();

        GameManager.Instance.UpdateState(Enums.StateGame.Lose);
    }

    #endregion

    /// <summary>
    /// Enables the blur.
    /// </summary>
    public void EnableBlur()
    {
        if (blur != null)
        {
            blur.enabled = true;
        }
    }

    /// <summary>
    /// Disables the blur.
    /// </summary>
    public void DisableBlur()
    {
        if (blur != null)
        {
            blur.enabled = false;
        }
    }
}