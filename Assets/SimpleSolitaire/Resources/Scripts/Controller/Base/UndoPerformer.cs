﻿using SimpleSolitaire.Model.Config;
using System.Collections.Generic;
using System.Linq;
using SimpleSolitaire.Model.Enum;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

namespace SimpleSolitaire.Controller
{
    public abstract class UndoPerformer : MonoBehaviour
    {
        [Header("Components:")] [SerializeField]
        protected CardLogic _cardLogicComponent;

        [SerializeField] protected GameManager _gameMgrComponent;
        [SerializeField] protected HintManager _hintComponent;
        [SerializeField] protected Button _undoButton;
        [SerializeField] protected Animator _undoButtonAnim;
        [SerializeField] protected Text _undoAvailableCountsText;

        [Header("Options:")] [Tooltip("If TRUE UndoLogic will be available only AvailableUndoCounts times. After that need to watch ads for getting new undo uses.")]
        public bool IsCountable = false;

        [Tooltip("How much UndoLogic uses user have from start and after ads watching.")]
        public int DefaultUndoCounts = 0;

        public abstract UndoData StatesData { get; set; }
        protected abstract string LastGameKey { get; }

        protected int AvailableUndoCounts = 0;
        private readonly string _undoBtnAnimationKey = "IsAnimate";

        protected bool IsCountableLogic
        {
            get
            {
#if UNITY_STANDALONE || UNITY_WEBGL
                return false;
#endif
                return IsCountable;
            }
        }

        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Action of Undo all decks/cards states.
        /// </summary>
        public virtual void Undo(bool removeOnlyState = false)
        {
            if (StatesData.States.Count > 0)
            {
                if (removeOnlyState)
                {
                    StatesData.States.RemoveAt(StatesData.States.Count - 1);
                    ActivateUndoButton();
                    return;
                }

                if (IsCountableLogic && AvailableUndoCounts > 0)
                {
                    AvailableUndoCounts--;
                    _undoAvailableCountsText.text = AvailableUndoCounts.ToString();
                }
                else if (IsCountableLogic && AvailableUndoCounts == 0)
                {
                    _gameMgrComponent.OnClickGetUndoAdsBtn();
                    return;
                }

                _hintComponent.IsHintWasUsed = false;
                _cardLogicComponent.IsNeedResetPack = false;

                for (int i = 0; i < _cardLogicComponent.AllDeckArray.Length; i++)
                {
                    _cardLogicComponent.AllDeckArray[i].Clear();
                }

                _cardLogicComponent.PackDeck.PushCardArray(_cardLogicComponent.CardsArray.ToArray(), false, 0);

                UndoProcess();

                _hintComponent.UpdateAvailableForDragCards();
                _cardLogicComponent.GameManagerComponent.CardMove();
                StatesData.States.RemoveAt(StatesData.States.Count - 1);
                ActivateUndoButton();
            }
        }

        protected virtual void UndoProcess()
        {
            int statesCount = StatesData.States.Count;
            for (int i = 0; i < StatesData.States[statesCount - 1].DecksRecord.Count; i++)
            {
                DeckRecord deckRecord = StatesData.States[statesCount - 1].DecksRecord[i];
                Deck deck = _cardLogicComponent.AllDeckArray.FirstOrDefault(x => x.DeckNum == deckRecord.DeckNum);

                if (deck == null)
                {
                    return;
                }

                for (int j = 0; j < deckRecord.CardsRecord.Count; j++)
                {
                    Card card = _cardLogicComponent.PackDeck.Pop();
                    card.Deck = deck;
                    deck.CardsArray.Add(card);
                }

                if (deck.HasCards)
                {
                    for (int j = 0; j < deckRecord.CardsRecord.Count; j++)
                    {
                        Card card = deck.CardsArray[j];
                        CardRecord cardRecord = deckRecord.CardsRecord[j];

                        card.CardType = cardRecord.CardType;
                        card.CardNumber = cardRecord.CardNumber;
                        card.Number = cardRecord.Number;
                        card.CardStatus = cardRecord.CardStatus;
                        card.CardColor = cardRecord.CardColor;
                        card.IsDraggable = cardRecord.IsDraggable;
                        card.IndexZ = cardRecord.IndexZ;
                        card.Deck = deck;
                        card.transform.localPosition = cardRecord.Pos.VectorPos;
                        card.transform.SetSiblingIndex(cardRecord.SiblingIndex);
#if UNITY_EDITOR
                        string cardName = $"{card.GetTypeName()}_{card.Number}";
                        card.gameObject.name = $"CardHolder ({cardName})";
                        card.BackgroundImage.gameObject.name = $"Card_{cardName}";
#endif
                    }
                }

                deck.UpdateCardsPosition(false);
            }
        }

        /// <summary>
        /// Setup <see cref="DefaultUndoCounts"/> value to _availableUndoCounts.
        /// </summary>
        public void UpdateUndoCounts()
        {
            if (IsCountableLogic)
            {
                _undoButtonAnim.SetBool(_undoBtnAnimationKey, false);

                AvailableUndoCounts = DefaultUndoCounts;
                _undoAvailableCountsText.text = AvailableUndoCounts.ToString();
                _undoAvailableCountsText.enabled = true;
            }
        }

        /// <summary>
        /// Collect new state.
        /// </summary>
        public virtual void AddUndoState(Deck[] allDeckArray, bool isTemp = false)
        {
            StatesData.States.Add(new UndoStates(allDeckArray, isTemp));
        }

        /// <summary>
        /// Activate for user undo button on bottom panel.
        /// </summary>
        public void ActivateUndoButton()
        {
            bool isHasUndoState = IsHasUndoState();

            if (IsCountableLogic && AvailableUndoCounts == 0 && StatesData.States.Count > 0)
            {
                _undoButtonAnim.SetBool(_undoBtnAnimationKey, true);
            }

            _undoButton.interactable = (IsCountableLogic) ? StatesData.States.Count != 0 : isHasUndoState;

            _undoAvailableCountsText.text = AvailableUndoCounts.ToString();
            _undoAvailableCountsText.enabled = IsCountableLogic && isHasUndoState;
        }

        /// <summary>
        /// Check for existing undo states.
        /// </summary>
        private bool IsHasUndoState()
        {
            return IsCountableLogic && AvailableUndoCounts != 0 && StatesData.States.Count > 0
                   || !IsCountableLogic && StatesData.States.Count > 0;
        }

        /// <summary>
        /// Clear array with states
        /// </summary>
        public void ResetUndoStates()
        {
            _undoButtonAnim.SetBool(_undoBtnAnimationKey, false);
            AvailableUndoCounts = DefaultUndoCounts;
            StatesData.States.Clear();
            ActivateUndoButton();
        }

        /// <summary>
        /// Save game with current game state.
        /// </summary>
        /// <param name="time"></param>
        /// <param name="steps"></param>
        /// <param name="score"></param>
        public abstract void SaveGame(int time, int steps, int score);

        /// <summary>
        /// Load game if it exist.
        /// </summary>
        public abstract void LoadGame();

        /// <summary>
        /// Set all numbers for cards of session. Needs to replay session whe user load last game.
        /// </summary>
        public void InitCardsNumberArray()
        {
            _cardLogicComponent.InitSpecificCardNums(StatesData.CardsNums);
        }

        /// <summary>
        /// Is has saved game process.
        /// </summary>
        public abstract bool IsHasGame();

        /// <summary>
        /// Delete last game.
        /// </summary>
        public void DeleteLastGame()
        {
            PlayerPrefs.DeleteKey(LastGameKey);
        }

        public string SerializeData(object lastGameData)
        {
            return JsonConvert.SerializeObject(lastGameData, Public.UNDO_DATA_SETTINGS);
        }

        public T DeserializeData<T>(string lastGameData) where T : UndoData
        {
            return JsonConvert.DeserializeObject<T>(lastGameData, Public.UNDO_DATA_SETTINGS);
        }
    }

    [System.Serializable]
    public class UndoData
    {
        public bool IsCountable;
        public int AvailableUndoCounts;
        public int Score;
        public int Steps;
        public int Time;
        public List<UndoStates> States = new List<UndoStates>();
        public int[] CardsNums;
    }

    [System.Serializable]
    public class KlondikeUndoData : UndoData
    {
        public DeckRule Rule;
    }

    [System.Serializable]
    public class SpiderUndoData : UndoData
    {
        public SpiderSuitsType SuitsType;
    }

    [System.Serializable]
    public class TripeaksUndoData : UndoData
    {
        public int LayoutId;
    }

    [System.Serializable]
    public class FreecellUndoData : UndoData
    {
        public FreecellAmountType AmountType;
        public FreecellDifficultyType Difficulty;
    }

    [System.Serializable]
    public class PyramidUndoData : UndoData
    {
        public int LayoutId;
    }

    [System.Serializable]
    public class UndoStates
    {
        public bool IsTemp;

        public List<DeckRecord> DecksRecord = new List<DeckRecord>();

        public UndoStates()
        {
        }

        public UndoStates(Deck[] decksStates, bool isTemp = false)
        {
            IsTemp = isTemp;

            foreach (var deck in decksStates)
            {
                DecksRecord.Add(new DeckRecord(deck.CardsArray, deck.DeckNum));
            }
        }
    }

    [System.Serializable]
    public class TripeaksUndoStates : UndoStates
    {
        public TripeaksUndoStates()
        {
        }

        public TripeaksUndoStates(Deck[] decksStates, bool isTemp = false) : base(decksStates, isTemp)
        {
            DecksRecord = new List<DeckRecord>();

            foreach (var deck in decksStates)
            {
                DecksRecord.Add(new TripeaksDeckRecord(deck.CardsArray, deck.DeckNum));
            }
        }
    }

    [System.Serializable]
    public class PyramidUndoStates : UndoStates
    {
        public PyramidUndoStates()
        {
        }

        public PyramidUndoStates(Deck[] decksStates, bool isTemp = false) : base(decksStates, isTemp)
        {
            DecksRecord = new List<DeckRecord>();

            foreach (var deck in decksStates)
            {
                DecksRecord.Add(new PyramidDeckRecord(deck.CardsArray, deck.DeckNum));
            }
        }
    }

    [System.Serializable]
    public class CardRecord
    {
        public int CardType;
        public int CardNumber;
        public int Number;
        public int CardStatus;
        public int CardColor;
        public bool IsDraggable;

        public int IndexZ;
        public int SiblingIndex;
        public int DeckNum;
        public CardPosition Pos;

        public CardRecord()
        {
        }

        public CardRecord(Card card)
        {
            CardType = card.CardType;
            CardNumber = card.CardNumber;
            Number = card.Number;
            CardStatus = card.CardStatus;
            CardColor = card.CardColor;
            IsDraggable = card.IsDraggable;
            IndexZ = card.IndexZ;
            DeckNum = card.Deck.DeckNum;
            SiblingIndex = card.transform.GetSiblingIndex();
            Pos = new CardPosition(card.transform.localPosition);
        }
    }

    [System.Serializable]
    public class TripeaksCardRecord : CardRecord
    {
        public int InfoId;

        public TripeaksCardRecord()
        {
        }

        public TripeaksCardRecord(Card card, CardPositionInfo info) : base(card)
        {
            InfoId = info.Id;
        }
    }

    public class PyramidCardRecord : CardRecord
    {
        public int InfoId;

        public PyramidCardRecord()
        {
        }

        public PyramidCardRecord(Card card, CardPositionInfo info) : base(card)
        {
            InfoId = info.Id;
        }
    }

    [System.Serializable]
    public class DeckRecord
    {
        public List<CardRecord> CardsRecord = new List<CardRecord>();
        public int DeckNum;

        public DeckRecord()
        {
        }

        public DeckRecord(List<Card> cards, int deckNum)
        {
            DeckNum = deckNum;

            foreach (var item in cards)
            {
                CardsRecord.Add(new CardRecord(item));
            }
        }
    }

    [System.Serializable]
    public class TripeaksDeckRecord : DeckRecord
    {
        public TripeaksDeckRecord()
        {
        }

        public TripeaksDeckRecord(List<Card> cards, int deckNum) : base(cards, deckNum)
        {
            CardsRecord = new List<CardRecord>();

            foreach (var item in cards)
            {
                TripeaksCard card = item as TripeaksCard;
                CardsRecord.Add(new TripeaksCardRecord(card, card.Info));
            }
        }
    }

    public class PyramidDeckRecord : DeckRecord
    {
        public PyramidDeckRecord()
        {
        }

        public PyramidDeckRecord(List<Card> cards, int deckNum) : base(cards, deckNum)
        {
            CardsRecord = new List<CardRecord>();

            foreach (var item in cards)
            {
                PyramidCard card = item as PyramidCard;
                CardsRecord.Add(new PyramidCardRecord(card, card.Info));
            }
        }
    }
}