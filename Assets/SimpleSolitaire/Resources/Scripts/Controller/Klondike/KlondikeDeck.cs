using DG.Tweening;
using SimpleSolitaire.Model.Enum;
using UnityEngine;

namespace SimpleSolitaire.Controller
{
    public class KlondikeDeck : Deck
    {
        /// <summary>
        /// Update card position in game by solitaire style
        /// </summary>
        /// <param name="firstTime">If it first game update</param>
        /// 
        float DT = 0.35f;
        public override void UpdateCardsPosition(bool firstTime)
        {
            for (int i = 0; i < CardsArray.Count; i++)
            {
                // Debug.LogError("update card pos: " +CardsArray[i].name);
                Card card = (Card)CardsArray[i];
                card.transform.SetAsLastSibling();
                if (Type == DeckType.DECK_TYPE_PACK)
                {
                    card.IsDraggable = false;
                    // card.gameObject.transform.position = gameObject.transform.position;
                    if (!card.dicked)
                        card.gameObject.transform.DOMove(gameObject.transform.position, DT).onComplete = (() =>
                        {
                            card.dicked = false;
                        });

                    else
                    {
                        card.gameObject.transform.position = gameObject.transform.position;
                    }
                    card.RestoreBackView();

                }
                else
                {
                    if (Type == DeckType.DECK_TYPE_ACE)
                    {
                        // card.gameObject.transform.position = gameObject.transform.position;
                        if (!card.dicked)
                            card.gameObject.transform.DOMove(gameObject.transform.position, DT).onComplete = (() =>
                        {
                            card.dicked = false;
                        });
                        else
                        {
                            card.gameObject.transform.position = gameObject.transform.position;
                        }
                    }
                    else if (Type == DeckType.DECK_TYPE_WASTE)
                    {
                        var wasteHorizontalSpace = CardLogicComponent.GetSpaceFromDictionary(DeckSpacesTypes.DECK_SPACE_HORIONTAL_WASTE);
                        card.IsDraggable = false;
                        // card.gameObject.transform.position = gameObject.transform.position;
                        int count = CardsArray.Count;
                        // Display up to 3 cards, and make them all draggable

                        if (count >= 0 && i >= count - 3)
                        {
                            float visibleIndex = (i - (count + 3)); // 0, 1, 2
                                                                    //65f -> 65% equal to 0.65 in waste space
                                                                    // card.gameObject.transform.position = gameObject.transform.position +(Vector3.right*Screen.width * 65f/100f) + new Vector3(visibleIndex * wasteHorizontalSpace, 0, 0);
                            if (!card.dicked)
                            {
                                card.gameObject.transform.DOMove(gameObject.transform.position +
                                                              (Vector3.right * Screen.width * 65f / 100f) +
                                                               new Vector3(visibleIndex * wasteHorizontalSpace, 0, 0), DT).onComplete = (() =>
                                                              {
                                                                  card.dicked = false;
                                                              });

                                if (!card.scaled)
                                {
                                    card.CardRect.pivot = new Vector2(1, 0.5f);
                                    card.transform.DOScaleX(0f, DT).From().SetDelay(0.05f);
                                    card.scaled = true;
                                }

                            }
                            else
                            {
                                card.gameObject.transform.position = gameObject.transform.position +
                                                             (Vector3.right * Screen.width * 65f / 100f) +
                                                              new Vector3(visibleIndex * wasteHorizontalSpace, 0, 0);
                            }
                            card.IsDraggable = true;
                        }
                    }

                    //last card in deck
                    if (i == CardsArray.Count - 1)
                    {
                        if (card.CardStatus != 0)
                        {
                            card.IsDraggable = true;
                            card.CardStatus = 1;
                            card.UpdateCardImg();
                        }
                        else
                        {
                            card.CardStatus = 1;
                            KlondikeHintManager.I.Schedule(0.05f, () =>
                            {
                                card.UpdateCardImg();
                            });
                            card.transform.DOLocalRotate(new Vector3(0, -90f, 0), DT).From().onComplete = (() =>
                            {
                                card.IsDraggable = true;
                                CardLogicComponent.ActionAfterEachStep();
                            });
                        }


                    }
                    else
                    {
                        if (firstTime)
                        {
                            card.IsDraggable = false;
                            card.CardStatus = 0;
                            card.UpdateCardImg();

                        }

                    }
                }
            }

            if (Type == DeckType.DECK_TYPE_BOTTOM)
            {
                for (int i = 0; i < CardsArray.Count; i++)
                {
                    Card card = CardsArray[i];
                    Card prevCard = i > 0 ? CardsArray[i - 1] : null;

                    var space = prevCard != null && prevCard.CardStatus == 1 ?
                        CardLogicComponent.GetSpaceFromDictionary(DeckSpacesTypes.DECK_SPACE_VERTICAL_BOTTOM_OPENED) :
                        CardLogicComponent.GetSpaceFromDictionary(DeckSpacesTypes.DECK_SPACE_VERTICAL_BOTTOM_CLOSED);

                    var spaceMultiplier = prevCard != null ? 1 : 0;
                    var deckPos = gameObject.transform.position;
                    var prevPos = prevCard != null ? prevCard.gameObject.transform.position : deckPos;

                    var curPos = prevPos - new Vector3(0, space, 0) * spaceMultiplier;

                    if (!card.dicked)
                        card.gameObject.transform.position = curPos;
                    else
                    {
                        card.gameObject.transform.position = curPos;
                    }

                }

            }
            UpdateCardsActiveStatus();
        }


        /// <summary>
        /// If we can drop card to other card it will be true.
        /// </summary>
        /// <param name="card">Checking card</param>
        /// <returns>We can drop or no</returns>
        public override bool AcceptCard(Card card)
        {
            Card topCard = GetTopCard();

            switch (Type)
            {
                case DeckType.DECK_TYPE_BOTTOM:
                    if (topCard != null)
                    {
                        //same type & card is higher than this card 1 value
                        if (topCard.CardColor != card.CardColor && topCard.Number == card.Number + 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        //king
                        if (card.Number == 13)
                        {
                            return true;
                        }

                        return false;
                    }
                case DeckType.DECK_TYPE_ACE:
                    Deck srcDeck = card.Deck;
                    if ((srcDeck.GetTopCard() != null && srcDeck.Type != DeckType.DECK_TYPE_WASTE) && srcDeck.GetTopCard() != card)
                    {
                        return false;
                    }

                    if (topCard != null)
                    {
                        if (topCard.CardType == card.CardType && topCard.Number == card.Number - 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (card.Number == 1)
                        {
                            return true;
                        }

                        return false;
                    }
            }

            return false;
        }

        public override void UpdateDraggableStatus()
        {
            if (Type == DeckType.DECK_TYPE_BOTTOM)
            {
                if (CardsCount == 0)
                {
                    return;
                }

                Card topCard = CardsArray[CardsArray.Count - 1];
                int topNumber = topCard.Number;
                bool isDraggable = true;
                topCard.IsDraggable = isDraggable;

                if (Type != DeckType.DECK_TYPE_WASTE)
                {

                    for (int i = CardsArray.Count - 2; i >= 0; i--)
                    {
                        var card = CardsArray[i];
                        int nextNumber = card.Number;

                        if (card.CardStatus == 1 && nextNumber == topNumber + 1)
                        {
                            card.IsDraggable = isDraggable;
                            topNumber++;
                        }
                        else
                        {
                            isDraggable = false;
                            card.IsDraggable = isDraggable;
                        }
                    }
                }
            }
        }

        public override void UpdateBackgroundColor()
        {

        }
    }
}


static public class DickExtension
{
    static public void Schedule(this MonoBehaviour mono,  float delay, System.Action action)
    {
        mono.StartCoroutine(ScheduleCoroutine(action, delay));
    }

    static private System.Collections.IEnumerator ScheduleCoroutine(System.Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }
}