using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    // ======================== Variables ======================== //

    #region Variables

    /// <summary>
    /// The state game.
    /// </summary>
    protected Enums.StateGame stateGame = Enums.StateGame.None;

    /// <summary>
    /// The mode game.
    /// </summary>
    public Enums.ModeGame ModeGame = Enums.ModeGame.Easy;

    /// <summary>
    /// The mode play.
    /// </summary>
    public Enums.PlayMode PlayMode = Enums.PlayMode.Normal;

    public int startingHandsToLoad = 0;

    public DateTime playDate;

    public int stage = 1;

    /// <summary>
    /// The current style.
    /// </summary>
    public Enums.Themes CurrentStyle = Enums.Themes.Default;

    #endregion

    // ======================== Functional ====================== //


    #region Functional

    /// <summary>
    /// Awake this instance.
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        Input.multiTouchEnabled = false;
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        PlayerPrefs.SetInt(Contains.CurrentEvent + "_0", 1);
        // PlayerPrefs.SetInt(Contains.CurrentEvent + "_" + 0 + "_Complete", 1);
        // for (int i = 0; i < 8; i++)
        // {
        //     PlayerPrefs.SetInt(Contains.CurrentEvent + "_" + 0 + "_" + i, 1);
        // }
        //
        // PlayerPrefs.SetInt(Contains.CurrentEvent + "_1", 1);
        // PlayerPrefs.SetInt(Contains.CurrentEvent + "_" + 1 + "_Complete", 1);
        // for (int i = 0; i < 16; i++)
        // {
        //     PlayerPrefs.SetInt(Contains.CurrentEvent + "_" + 1 + "_" + i, 1);
        // }
        //
        // PlayerPrefs.SetInt(Contains.CurrentEvent + "_2", 1);
        // PlayerPrefs.SetInt(Contains.CurrentEvent + "_" + 2 + "_Complete", 1);
        // for (int i = 0; i < 24; i++)
        // {
        //     PlayerPrefs.SetInt(Contains.CurrentEvent + "_" + 2 + "_" + i, 1);
        // }
    }

    /// <summary>
    /// Start this instance.
    /// </summary>
    protected void Start()
    {
        if (PoolSystem.Instance == null)
        {
            throw new UnityException("The game can not start!");
        }

        InitStart();

        InitCards();
    }

    public void UpdateState(Enums.StateGame state)
    {
        stateGame = state;
    }

    #endregion

    // ====================== Init ============================ //

    #region Init

    /// <summary>
    /// Inits the cards.
    /// </summary>
    public void InitCards()
    {
        PoolSystem.Instance.ClearCards();

        CardDataProperties[] cards = DataSystem.Instance.GetCardsData();

        int count = cards.Length;

        CardDataProperties card;

        for (int i = 0; i < count;)
        {
            card = cards[i];

            if (!ReferenceEquals(card, null))
            {
                InitCard(DataSystem.Instance.GetCardPrefab(), card, Enums.CardBoard.CardUse);
            }

            i = PoolSystem.Instance.GetNumberOfCards();
        }
    }

    /// <summary>
    /// Inits the start.
    /// </summary>
    public void InitStart()
    {
        ModeGame = PlayerPrefs.GetString("ModeGame", "Easy") == "Easy" ? Enums.ModeGame.Easy : Enums.ModeGame.Hard;
        stateGame = Enums.StateGame.Start;
    }


    /// <summary>
    /// Inits the card.
    /// </summary>
    /// <param name="card">Card.</param>
    /// <param name="data">Data.</param>
    /// <param name="cardOnBoard">Card on board.</param>
    protected bool InitCard(CardBehaviour card, CardDataProperties data, Enums.CardBoard cardOnBoard)
    {
        GameObject param = Instantiate(card.gameObject) as GameObject;

        if (param.GetComponent<CardBehaviour>() != null)
        {
            CardBehaviour paramBehaviour = param.GetComponent<CardBehaviour>();

            paramBehaviour.Init(data, cardOnBoard);

            PoolSystem.Instance.ReturnToPool(paramBehaviour);

            return true;
        }
        else
        {
            Destroy(param);
        }

        return false;
    }

    #endregion


    // ======================= Helper ======================== //

    #region Helper

    /// <summary>
    /// Gets the state game.
    /// </summary>
    /// <returns>The state game.</returns>
    public Enums.StateGame GetStateGame()
    {
        return stateGame;
    }

    /// <summary>
    /// Determines whether this instance is game ready.
    /// </summary>
    /// <returns><c>true</c> if this instance is game ready; otherwise, <c>false</c>.</returns>
    public bool IsGameReady()
    {
        return stateGame == Enums.StateGame.Playing;
    }

    /// <summary>
    /// Determines whether this instance is game end.
    /// </summary>
    /// <returns><c>true</c> if this instance is game end; otherwise, <c>false</c>.</returns>
    public bool IsGameEnd()
    {
        return stateGame == Enums.StateGame.Lose || stateGame == Enums.StateGame.Win;
    }

    #endregion
}