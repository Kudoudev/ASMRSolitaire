using MovementEffects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct History
{
	/// <summary>
	/// The card.
	/// </summary>
	public CardBehaviour card;

	/// <summary>
	/// The data.
	/// </summary>
	public CardProperties data;

	/// <summary>
	/// The holder play.
	/// </summary>
	public Enums.CardPlayingEnums HolderPlay;

	/// <summary>
	/// The holder result.
	/// </summary>
	public Enums.ResultManager HolderResult;

	/// <summary>
	/// The is card on hint.
	/// </summary>
	public bool IsCardOnHint;

	/// <summary>
	/// The is on shower card.
	/// </summary>
	public bool IsOnShowerCard;

	/// <summary>
	/// The score.
	/// </summary>
	public int Score;

	/// <summary>
	/// The is card unlock.
	/// </summary>
	public bool IsCardUnlock;

	/// <summary>
	/// The position.
	/// </summary>
    public Vector3 position;

	/// <summary>
	/// The cards follow.
	/// </summary>
    public List<CardBehaviour> cardsFollow;
}

/// <summary>
/// Undo system.
/// </summary>
public class UndoSystem : SingletonMonoBehaviour < UndoSystem > {

    // =============================== Variables ========================== //
	/// <summary>
	/// The history.
	/// </summary>
    protected List<History> history = new List<History>();

	/// <summary>
	/// The states.
	/// </summary>
    protected List<List<History>> states = new List<List<History>>();

	/// <summary>
	/// The is ready undo.
	/// </summary>
    protected bool IsReadyUndo = true;

	/// <summary>
	/// The maximum record.
	/// </summary>
	protected int MaximumRecord = 100;

	/// <summary>
	/// The index.
	/// </summary>
    protected int index = 0;
// =========================== Functional ============================ //
	/// <summary>
	/// Records the state.
	/// </summary>
	/// <param name="card">Card.</param>
	/// <param name="position">Position.</param>
	/// <param name="IsCardOnHint">If set to <c>true</c> is card on hint.</param>
	/// <param name="holderPlay">Holder play.</param>
	/// <param name="holderResult">Holder result.</param>
	/// <param name="isCardUnlocked">If set to <c>true</c> is card unlocked.</param>
	/// <param name="IsOnShowerCard">If set to <c>true</c> is on shower card.</param>
	/// <param name="score">Score.</param>
	/// <param name="IsSameState">If set to <c>true</c> is same state.</param>
	public void RecordState(CardBehaviour card , Vector3 position , bool IsCardOnHint , Enums.CardPlayingEnums holderPlay, Enums.ResultManager holderResult , bool isCardUnlocked  ,  bool IsOnShowerCard = false, int score = 0 , bool IsSameState = false )
    {
        if (states.Count >= MaximumRecord)
        {
            states.RemoveAt(0);
        }

        if (states.Count == 0) {
			states.Add (new List<History>());
		}else
        {
            if (!IsSameState)
            {
                states.Add(new List<History>());
            }
        }		

		history = states [states.Count - 1];

        History value = new History();

		value.data = card.GetProperties ();

		value.HolderPlay = holderPlay;

		value.HolderResult = holderResult;

		value.IsOnShowerCard = IsOnShowerCard;

		value.Score = score;

		value.card = card;

		value.IsCardOnHint = IsCardOnHint;

		value.IsCardUnlock = isCardUnlocked;

		value.position = card.transform.parent.InverseTransformPoint(position);

        if (HelperCardsManager.Instance.cardsMemoryTemp.Count > 0 )
        {
            value.cardsFollow = new List<CardBehaviour>(HelperCardsManager.Instance.cardsMemoryTemp);
        }   

        history.Add(value);

        states [states.Count - 1] = history;
    }

	/// <summary>
	/// Undos the state.
	/// </summary>
    public void UndoState()
    {
		if (!IsReadyUndo || GameManager.Instance.IsGameEnd() || !GameManager.Instance.IsGameReady())
            return;

        if (states.Count == 0)
        {
            return;
        }

        GameManager.Instance.UpdateState(Enums.StateGame.Waiting);

		Timing.RunCoroutine(Undo());
    }

	/// <summary>
	/// Undo this instance.
	/// </summary>
	IEnumerator < float > Undo()
	{
		// TODO: something before another undo is ready.

		history = states [states.Count - 1];

		states.RemoveAt (states.Count - 1);

		for (int i = history.Count - 1; i > -1; i--) {

			History _history = history [i];

			CardBehaviour card = _history.card;

			if (_history.HolderPlay != Enums.CardPlayingEnums.None) {

				PlayingCardsManager.Instance.RemoveThisCard (card);

                ResultCardsManager.Instance.OutOfHolder(card);

                HidenCardsManager.Instance.OutOfHolder(card);

                if (_history.cardsFollow != null && _history.cardsFollow.Count > 0)
                {
                    HelperCardsManager.Instance.cardsMemoryTemp.AddRange (_history.cardsFollow);

                    for ( int j = 0; j < HelperCardsManager.Instance.cardsMemoryTemp.Count; j++)
                    {
                        HelperCardsManager.Instance.cardsMemoryTemp[j].transform.SetParent(card.transform);
                    }
                }

                card.UpdateStateCard(Enums.CardBoard.CardUse);

                Transform transformGet = PlayingCardsManager.Instance.GetParrentOfCards (_history.HolderPlay);

                card.transform.SetParent(HelperCardsManager.Instance.GetTheTransformOfHolder());

				card.targetPositionCards = PlayingCardsManager.Instance.GetLastPositionInHolder (_history.HolderPlay);	

                card.MovingToPosition (card.targetPositionCards, false, () => {

					card.transform.SetParent (transformGet);

					card.transform.SetAsLastSibling ();

                    card.DistributeTheFollowCards();
                });               

                PlayingCardsManager.Instance.UpdateNewCardToHolder (card, _history.HolderPlay);

			} else if (_history.HolderResult != Enums.ResultManager.None) {

                PlayingCardsManager.Instance.RemoveThisCard(card);

                ResultCardsManager.Instance.OutOfHolder (card);

                HidenCardsManager.Instance.OutOfHolder(card);

                card.UpdateStateCard(Enums.CardBoard.CardUse);

                card.transform.SetParent (HelperCardsManager.Instance.GetTheTransformOfHolder ());

				Transform transformGet = ResultCardsManager.Instance.GetHolderCards (_history.HolderResult);

				card.targetPositionCards = ResultCardsManager.Instance.GetLastPositionInHolder (_history.HolderResult);	

                card.MovingToPosition (card.targetPositionCards, false, () => {

					card.transform.SetParent (transformGet);

					card.transform.SetAsLastSibling ();
				});

				ResultCardsManager.Instance.UpdateNewCardToHolder (card, _history.HolderResult);
			
			} else if (_history.IsCardOnHint) {

                PlayingCardsManager.Instance.RemoveThisCard(card);

                ResultCardsManager.Instance.OutOfHolder(card);

                HidenCardsManager.Instance.OutOfHolder(card);

                card.UpdateStateCard(Enums.CardBoard.CardHint);

                HidenCardsManager.Instance.AddTheNewCardsToHidenCards (card, _history.IsOnShowerCard);
			}
				
			card.UnlockCard (_history.IsCardUnlock);

			card.UpdateDataCard (_history.data);

			Contains.Score = _history.Score;

			HudSystem.Instance.UpdateScore (Contains.Score);
		}

		yield return Timing.WaitForSeconds (0.6f);

		GameManager.Instance.UpdateState (Enums.StateGame.Playing);

		yield return 0f;

        IsReadyUndo = true;
    }
}
