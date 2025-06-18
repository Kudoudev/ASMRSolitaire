using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enums.
/// </summary>
public static class Enums  {

	// ======================== Cards Type ======================== //

	/// <summary>
	/// Card type.
	/// </summary>
	public enum CardType 
	{
		None 	= 0,
		Club 	= 1,
		Heart 	= 2,
		Diamond = 3,
		Spade 	= 4,
	}

	/// <summary>
	/// State card.
	/// </summary>
	public enum StateCard
	{
		None 	  = 0,
		Unlocked  = 1,
		Collected = 2,
	}

	/// <summary>
	/// Card board.
	/// </summary>
	public enum CardBoard
	{
		CardHint = 0,
		CardUse  = 1,
	}

	/// <summary>
	/// Card variables.
	/// </summary>
	public enum CardVariables
	{
		One 	= 1,
		Two 	= 2,
		Three 	= 3,
		Four 	= 4,
		Five 	= 5,
		Six 	= 6,
		Seven	= 7,
		Eight	= 8,
		Nine 	= 9,
		Ten		= 10,
		Jack	= 11,
		Queen	= 12,
		King	= 13,
	}

	// ======================== Game State ======================== //

	/// <summary>
	/// State game.
	/// </summary>
	public enum StateGame
	{
		None 	 = 0,
		Start	 = 1,
		Pause 	 = 2,
		Playing  = 3,
		Waiting  = 4,
		Win = 5,
		Lose = 6,
	}

	/// <summary>
	/// Mode game.
	/// </summary>
    public enum ModeGame
    {
		None,
        Easy,
        Hard,
    }

	public enum PlayMode
	{
		Normal,
		Daily,
		Event
	}

	// ======================== Direction ========================= //

	/// <summary>
	/// Direction.
	/// </summary>
	public enum Direction
	{
		Up = 0,
        Right = 1,
        Left = 2,
        Down = 3,
        None = 4,
	}

    // ======================== Holder ============================ //
	/// <summary>
	/// Card playing enums.
	/// </summary>
    public enum CardPlayingEnums
    {
        holderOne = 0,
        holderTwo = 1,
        holderThree = 2,
        holderFour = 3,
        holderFive = 4,
        holderSix = 5,
        holderSeven = 6,
        None = 7,
    }

	/// <summary>
	/// Result manager.
	/// </summary>
    public enum ResultManager
    {
        holderOne = 0,
        holderTwo = 1,
        holderThree = 2,
        holderFour = 3,
        None = 4,
    }

    // ======================= Sound ========================= //

	/// <summary>
	/// Sound index.
	/// </summary>
    public enum SoundIndex
    {
        None = 0,
		Draw = 1,
		Error = 2,
		Press = 3,
    }


    // ====================== Music ========================== //

	/// <summary>
	/// Music index.
	/// </summary>
    public enum MusicIndex
    {
        None = 0,
		Background_I = 1,
		Background_II = 2,
		Background_III = 3,
		WinMusic = 4,
		LoseMusic = 5,
		StartMusic = 6,
    }

	public enum Themes
	{
		Default = 0,
		Candy = 1,
	}
}
