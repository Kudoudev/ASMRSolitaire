using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public static class Helper
{
    /// <summary>
    /// Gets the world position.
    /// </summary>
    /// <returns>The world position.</returns>
    public static Vector3 GetWorldPosition()
    {
        Vector3 valueReturn = Input.mousePosition;

        valueReturn = Camera.main.ScreenToWorldPoint(valueReturn);

        return valueReturn;
    }


    /// <summary>
    /// Sorts the cards.
    /// </summary>
    /// <param name="nubmerLastCard">Nubmer last card.</param>
    /// <param name="distance">Distance.</param>
    public static void SortCards(List<CardBehaviour> cardSort, Transform holder, int nubmerLastCard = 3,
        Enums.Direction direction = Enums.Direction.Right, float distance = Contains.DistanceSortUnlockedCards,
        bool IsUseAnimation = false)
    {
        Vector3 PositionStart = holder.position;

        int litmitCards = Mathf.Clamp(cardSort.Count - nubmerLastCard, 0, int.MaxValue);

        CardBehaviour card;

        for (int i = 0; i < litmitCards; i++)
        {
            card = cardSort[i];

            if (card.transform.parent != holder)
            {
                card.transform.SetParent(holder);

                card.transform.localScale = Vector3.one;
            }

            card.targetPositionCards = PositionStart;

            if (IsUseAnimation)
            {
                card.MovingToPosition(card.targetPositionCards);
            }
            else
            {
                card.transform.position = card.targetPositionCards;
            }
        }

        for (int i = litmitCards; i < cardSort.Count; i++)
        {
            card = cardSort[i];

            if (card.transform.parent != holder)
            {
                card.transform.SetParent(holder);

                card.transform.localScale = Vector3.one;
            }

            card.targetPositionCards = PositionStart;

            if (IsUseAnimation)
            {
                card.MovingToPosition(card.targetPositionCards);
            }
            else
            {
                card.transform.position = card.targetPositionCards;
            }
        }
    }

    public static void SortCardsUnlock(List<CardBehaviour> cardSort, Transform holder, int nubmerLastCard = 3,
        Enums.Direction direction = Enums.Direction.Right, float distance = Contains.DistanceSortUnlockedCards,
        bool IsUseAnimation = false)
    {
        if (cardSort.Count == 0) return;
        Vector3 PositionStart = holder.position;

        CardBehaviour card;

        for (int i = cardSort.Count - 1; i > cardSort.Count - nubmerLastCard && i >= 0; i--)
        {
            card = cardSort[i];

            if (card.transform.parent != holder)
            {
                card.transform.SetParent(holder);
                card.transform.localScale = Vector3.one;
            }

            card.transform.SetAsFirstSibling();

            if (direction == Enums.Direction.Right)
                card.targetPositionCards = PositionStart - (cardSort.Count - 1 - i) * new Vector3(distance, 0);
            else card.targetPositionCards = PositionStart + (cardSort.Count - 1 - i) * new Vector3(distance, 0);

            if (IsUseAnimation)
            {
                card.MovingToPosition(card.targetPositionCards);
            }
            else
            {
                card.transform.position = card.targetPositionCards;
            }
        }

        for (int i = cardSort.Count - nubmerLastCard; i >= 0; i--)
        {
            card = cardSort[i];

            if (card.transform.parent != holder)
            {
                card.transform.SetParent(holder);
                card.transform.localScale = Vector3.one;
            }

            card.transform.SetAsFirstSibling();

            if (direction == Enums.Direction.Right)
                card.targetPositionCards = PositionStart - 2 * new Vector3(distance, 0);
            else card.targetPositionCards = PositionStart + 2 * new Vector3(distance, 0);


            if (IsUseAnimation)
            {
                card.MovingToPosition(card.targetPositionCards);
            }
            else
            {
                card.transform.position = card.targetPositionCards;
            }
        }
    }

    /// <summary>
    /// Sorts the unlocked cards.
    /// </summary>
    /// <param name="cardSort">Card sort.</param>
    /// <param name="holder">Holder.</param>
    /// <param name="direction">Direction.</param>
    /// <param name="distanceUnlocked">Distance unlocked.</param>
    /// <param name="distanceLocked">Distance locked.</param>
    /// <param name="IsUseAnimation">If set to <c>true</c> is use animation.</param>
    public static void SortUnlockedCards(List<CardBehaviour> cardSort, Transform holder,
        Enums.Direction direction = Enums.Direction.Down, float distanceUnlocked = Contains.DistanceSortUnlockedCards,
        float distanceLocked = Contains.DistanceSortLockedCards, bool IsUseAnimation = false)
    {
        Vector3 PositionStart = holder.position;

        CardBehaviour card;

        for (int i = 0; i < cardSort.Count; i++)
        {
            card = cardSort[i];

            if (card.transform.parent != holder)
            {
                card.transform.SetParent(holder);

                card.transform.localScale = Vector3.one;
            }

            card.targetPositionCards = PositionStart;

            if (IsUseAnimation)
            {
                card.MovingToPosition(card.targetPositionCards);
            }
            else
            {
                card.transform.position = card.targetPositionCards;
            }

            if (card.IsUnlocked())
            {
                switch (direction)
                {
                    case Enums.Direction.Down:

                        PositionStart.y = PositionStart.y - Math.Abs(distanceUnlocked);
                        break;
                    case Enums.Direction.Left:

                        PositionStart.x = PositionStart.x - Math.Abs(distanceUnlocked);
                        break;
                    case Enums.Direction.Right:

                        PositionStart.x = PositionStart.x + Math.Abs(distanceUnlocked);
                        break;
                    case Enums.Direction.Up:

                        PositionStart.y = PositionStart.y + Math.Abs(distanceUnlocked);
                        break;
                }
            }
            else
            {
                switch (direction)
                {
                    case Enums.Direction.Down:
                        PositionStart.y = PositionStart.y - Math.Abs(distanceLocked);
                        break;
                    case Enums.Direction.Left:
                        PositionStart.x = PositionStart.x - Math.Abs(distanceLocked);
                        break;
                    case Enums.Direction.Right:
                        PositionStart.x = PositionStart.x + Math.Abs(distanceLocked);
                        break;
                    case Enums.Direction.Up:
                        PositionStart.y = PositionStart.y + Math.Abs(distanceLocked);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Gets the position in the holder cards.
    /// </summary>
    /// <returns>The position in the holder cards.</returns>
    /// <param name="CardsHolder">Cards holder.</param>
    /// <param name="direction">Direction.</param>
    /// <param name="distanceUnlocked">Distance unlocked.</param>
    /// <param name="distanceLocked">Distance locked.</param>
    public static Vector3 GetPositionInTheHolderCards(List<CardBehaviour> CardsHolder, Enums.Direction direction,
        float distanceUnlocked = Contains.DistanceSortUnlockedCards,
        float distanceLocked = Contains.DistanceSortLockedCards)
    {
        Vector3 position = Vector3.zero;

        if (CardsHolder.Count > 0)
        {
            CardBehaviour card = CardsHolder[CardsHolder.Count - 1];

            position = card.targetPositionCards;

            switch (direction)
            {
                case Enums.Direction.Down:

                    if (CardsHolder[CardsHolder.Count - 1].IsUnlocked())
                    {
                        position.y = card.targetPositionCards.y - distanceUnlocked;
                    }
                    else
                    {
                        position.y = card.targetPositionCards.y - distanceLocked;
                    }

                    break;
                case Enums.Direction.Left:

                    if (CardsHolder[CardsHolder.Count - 1].IsUnlocked())
                    {
                        position.x = card.targetPositionCards.x - distanceUnlocked;
                    }
                    else
                    {
                        position.x = card.targetPositionCards.x - distanceLocked;
                    }

                    break;
                case Enums.Direction.Right:

                    if (CardsHolder[CardsHolder.Count - 1].IsUnlocked())
                    {
                        position.x = card.targetPositionCards.x + distanceUnlocked;
                    }
                    else
                    {
                        position.x = card.targetPositionCards.x + distanceLocked;
                    }

                    break;
                case Enums.Direction.Up:

                    if (CardsHolder[CardsHolder.Count - 1].IsUnlocked())
                    {
                        position.y = card.targetPositionCards.y + distanceUnlocked;
                    }
                    else
                    {
                        position.y = card.targetPositionCards.y + distanceLocked;
                    }

                    break;
            }
        }

        return position;
    }

    /// <summary>
    /// Sorts the random.
    /// </summary>
    /// <returns>The random.</returns>
    /// <param name="cards">Cards.</param>
    public static List<CardBehaviour> SortRandom(List<CardBehaviour> cards)
    {
        List<CardBehaviour> cardsReturn = new List<CardBehaviour>();

        List<CardBehaviour> cardsGet = new List<CardBehaviour>(cards);

        switch (GameManager.Instance.PlayMode)
        {
            case Enums.PlayMode.Normal:
                Random.InitState(Environment.TickCount);
                while (cardsGet.Count != 0)
                {
                    var card = cardsGet[Random.Range(0, cardsGet.Count)];

                    cardsGet.Remove(card);

                    cardsReturn.Add(card);
                }

                break;
            case Enums.PlayMode.Daily:
            case Enums.PlayMode.Event:
                Random.InitState(GameManager.Instance.startingHandsToLoad);
                int handsToLoad = Random.Range(1, Contains.StartingHandsCount + 1);
                string hands = ReadHandsFromFile(handsToLoad);
                string[] listCard = hands.Split(",");
                foreach (string s in listCard)
                {
                    if (String.IsNullOrEmpty(s)) continue;
                    string val = s.Substring(1, s.Length - 1);
                    string type = s.Substring(0, 1);
                    Enums.CardType cardType = Enums.CardType.Spade;
                    Enums.CardVariables cardVar = Enums.CardVariables.One;
                    switch (val)
                    {
                        case "2":
                            cardVar = Enums.CardVariables.Two;
                            break;
                        case "3":
                            cardVar = Enums.CardVariables.Three;
                            break;
                        case "4":
                            cardVar = Enums.CardVariables.Four;
                            break;
                        case "5":
                            cardVar = Enums.CardVariables.Five;
                            break;
                        case "6":
                            cardVar = Enums.CardVariables.Six;
                            break;
                        case "7":
                            cardVar = Enums.CardVariables.Seven;
                            break;
                        case "8":
                            cardVar = Enums.CardVariables.Eight;
                            break;
                        case "9":
                            cardVar = Enums.CardVariables.Nine;
                            break;
                        case "01":
                            cardVar = Enums.CardVariables.Ten;
                            break;
                        case "J":
                            cardVar = Enums.CardVariables.Jack;
                            break;
                        case "Q":
                            cardVar = Enums.CardVariables.Queen;
                            break;
                        case "K":
                            cardVar = Enums.CardVariables.King;
                            break;
                    }

                    switch (type)
                    {
                        case "c":
                            cardType = Enums.CardType.Club;
                            break;
                        case "d":
                            cardType = Enums.CardType.Diamond;
                            break;
                        case "h":
                            cardType = Enums.CardType.Heart;
                            break;
                    }

                    CardBehaviour card;
                    for (int i = 0; i < cardsGet.Count; i++)
                    {
                        card = cardsGet[i];
                        if (card.GetProperties().GetDataProperties().GetCardValue() == (int)cardVar &&
                            card.GetProperties().GetDataProperties().GetCardType() == cardType)
                        {
                            cardsReturn.Add(card);
                            cardsGet.Remove(card);
                            break;
                        }
                    }
                }

                break;
        }

        return cardsReturn;
    }

    private static string ReadHandsFromFile(int handsToLoad)
    {
        string startHands = Resources.Load<TextAsset>("StartingHands/cards" + handsToLoad).text;
        string[] listCardPerRow =
            startHands.Substring(11).Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(s => s.Trim())
                .ToArray();
        string reverseHands = String.Join("", listCardPerRow).Trim(',');
        string hands = new string(reverseHands.ToCharArray().Reverse().ToArray());
        return hands;
    }

    /// <summary>
    /// Updates the score.
    /// </summary>
    /// <param name="param">Parameter to add.</param>
    public static void UpdateScore(int param)
    {
        Contains.Score =
            Mathf.Clamp(
                Contains.Score + param * (GameManager.Instance.ModeGame == Enums.ModeGame.Easy ? 1 : 3) *
                (GameManager.Instance.PlayMode == Enums.PlayMode.Normal ? GameManager.Instance.stage : 1), 0,
                int.MaxValue);
        if (Contains.Score > Contains.BestScoreSolitaire) Contains.BestScoreSolitaire = Contains.Score;
    }
}