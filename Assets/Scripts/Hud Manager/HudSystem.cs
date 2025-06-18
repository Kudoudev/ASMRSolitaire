using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// User interface image contents.
/// </summary>
[System.Serializable]
public struct UIImageContents
{
    /// <summary>
    /// The image undo.
    /// </summary>
    public Image[] ImageUndo;

    /// <summary>
    /// The image hint.
    /// </summary>
    public Image[] ImageHint;

    /// <summary>
    /// The image restart.
    /// </summary>
    public Image[] ImageRestart;

    /// <summary>
    /// The image themes.
    /// </summary>
    public Image[] ImageThemes;

    /// <summary>
    /// The image result cards.
    /// </summary>
    public Image[] ImageResultCards;

    /// <summary>
    /// The reset cards.
    /// </summary>
    public Image[] ResetCards;

    /// <summary>
    /// The portrait background.
    /// </summary>
    public Image[] PortraitBackground;

    /// <summary>
    /// The landscape background.
    /// </summary>
    public MeshRenderer[] LandscapeBackground;
}

[System.Serializable]
public struct UISpriteContents
{
    /// <summary>
    /// The style identifier.
    /// </summary>
    public Enums.Themes StyleId;

    /// <summary>
    /// The sprite undo.
    /// </summary>
    public Sprite spriteUndo;

    /// <summary>
    /// The sprite restart.
    /// </summary>
    public Sprite spriteRestart;

    /// <summary>
    /// The sprite hint.
    /// </summary>
    public Sprite spriteHint;

    /// <summary>
    /// The sprite theme.
    /// </summary>
    public Sprite spriteTheme;

    /// <summary>
    /// The sprite result cards.
    /// </summary>
    public Sprite spriteResultCards;

    /// <summary>
    /// The sprite reset cards.
    /// </summary>
    public Sprite spriteResetCards;

    /// <summary>
    /// The sprite portrait background.
    /// </summary>
    public Sprite spritePortraitBackground;

    /// <summary>
    /// The sprite landscape background.
    /// </summary>
    public Material spriteLandscapeBackground;
}

/// <summary>
/// Hud system.
/// </summary>
public class HudSystem : SingletonMonoBehaviour<HudSystem>
{
    // ================================== References ============================ //

    #region UI

    [Header("UI Player")]
    /// <summary>
    /// The user interface play mode display.
    /// </summary>
    [SerializeField]
    private Text[] UIDealDisplay;

    /// <summary>
    /// The user interface best score display.
    /// </summary>
    [SerializeField] private Text[] UIBestScoreDisplay;

    /// <summary>
    /// The user interface time display.
    /// </summary>
    [SerializeField] private Text[] UITimeDisplay;

    /// <summary>
    /// The user interface score display.
    /// </summary>
    [SerializeField] private Text[] UIScoreDisplay;

    /// <summary>
    /// The user interface move display.
    /// </summary>
    [SerializeField] private Text[] UIMoveDisplay;

    [Header("Banner Game")]
    /// <summary>
    /// The user interface banner.
    /// </summary>
    [SerializeField]
    private Transform UIBanner;

    /// <summary>
    /// The user interface window.
    /// </summary>
    [SerializeField] private Transform UIWin;

    /// <summary>
    /// The user interface lose.
    /// </summary>
    [SerializeField] private Transform UILose;

    [SerializeField] private GameObject loseDailyEvent, loseNormal;

    /// <summary>
    /// The hud UI.
    /// </summary>
    // [SerializeField] private CanvasGroup HudUI;
    [Header("Hint")] [SerializeField] private Text UITextHint;

    /// <summary>
    /// The bar menu.
    /// </summary>
    [SerializeField] private RectTransform[] barMenu;

    [Header("Themes")]
    /// <summary>
    /// The image contents.
    /// </summary>
    public UIImageContents imageContents;

    /// <summary>
    /// The sprite contents.
    /// </summary>
    public UISpriteContents[] spriteContents;

    #endregion


    #region Variables

    /// <summary>
    /// The is ready restart.
    /// </summary>
    protected bool IsReadyRestart;

    #endregion


    #region Functional

    /// <summary>
    /// Updates the sprite.
    /// </summary>
    /// <param name="style">Style.</param>
    public void UpdateSprite(Enums.Themes style)
    {
        for (int i = 0; i < spriteContents.Length; i++)
        {
            // Check id style as same as the id from condition.
            if (spriteContents[i].StyleId == style)
            {
                // Update the sprite of Hint user interface.
                for (int j = 0; j < imageContents.ImageHint.Length; j++)
                {
                    // Update the parameters from ImageHints.
                    imageContents.ImageHint[j].sprite = spriteContents[i].spriteHint;
                }

                // Update the sprite of restart user interface.
                for (int j = 0; j < imageContents.ImageRestart.Length; j++)
                {
                    // Update the parameters from ImageRestart.
                    imageContents.ImageRestart[j].sprite = spriteContents[i].spriteRestart;
                }

                // Update the sprite of undo user interface.
                for (int j = 0; j < imageContents.ImageUndo.Length; j++)
                {
                    // Update the parameters from ImageUndo.
                    imageContents.ImageUndo[j].sprite = spriteContents[i].spriteUndo;
                }

                // Update the sprite of Result user interface.
                for (int j = 0; j < imageContents.ImageResultCards.Length; j++)
                {
                    // Update the parameters from ImageResultCards.
                    imageContents.ImageResultCards[j].sprite = spriteContents[i].spriteResultCards;
                }

                // Update the sprite of Themes user interface.
                for (int j = 0; j < imageContents.ImageThemes.Length; j++)
                {
                    // Update the parameters from ImageThemes.
                    imageContents.ImageThemes[j].sprite = spriteContents[i].spriteTheme;
                }

                // Update the sprite of Portrait Background.
                for (int j = 0; j < imageContents.PortraitBackground.Length; j++)
                {
                    // Update the parameters from PortraitBackground.
                    imageContents.PortraitBackground[j].sprite = spriteContents[i].spritePortraitBackground;
                }

                // Update the sprite of Landscape Background.
                for (int j = 0; j < imageContents.LandscapeBackground.Length; j++)
                {
                    // Update the parameters from LandscapeBackground.
                    imageContents.LandscapeBackground[j].material = spriteContents[i].spriteLandscapeBackground;
                }

                // Update the sprite of Reset Cards.
                for (int j = 0; j < imageContents.ResetCards.Length; j++)
                {
                    // Update the parameters from ResetCards.
                    imageContents.ResetCards[j].sprite = spriteContents[i].spriteResetCards;
                }

                break;
            }
        }
    }

    /// <summary>
    /// Awake this instance.
    /// </summary>
    public void StartGame()
    {
        // Invoke time display when playing.
        InvokeRepeating(nameof(UpdateTimeDisplay), 0, 1);

        if (GameManager.Instance.PlayMode == Enums.PlayMode.Normal)
        {
            foreach (var t in UIDealDisplay) t.text = "Stage\n" + GameManager.Instance.stage;
            foreach (var t in UIBestScoreDisplay)
            {
                t.gameObject.SetActive(true);
                t.text = "All Time\n" + Contains.BestScoreSolitaire;
            }
        }
        else
        {
            foreach (var t in UIDealDisplay) t.text = "Deal\n" + GameManager.Instance.PlayMode;
            foreach (var t in UIBestScoreDisplay) t.gameObject.SetActive(false);
        }


        // Reset number of move to zero.
        UpdateMove(0);

        // Reset number of score to zero.
        UpdateScore(Contains.Score);

        foreach (var image in imageContents.ImageRestart)
        {
            image.gameObject.SetActive(GameManager.Instance.PlayMode != Enums.PlayMode.Normal);
        }

        // Hide the ads.
        // DisableAds();

        // InvokeRepeating(nameof(InvokeCalleAds), 0, 0.5f);
    }

    protected bool IsCalled = false;

    protected bool IsDisable = false;

    /// <summary>
    /// Invokes the calle ads.
    /// </summary>
    public void InvokeCalleAds()
    {
        if (!MutilResolution.Instance.IsPortrait)
        {
            return;
        }

        //if (AdSystem.Instance.IsAdLoading && Contains.IsHavingRemoveAd == false && IsCalled == false ) {

        //	EnableBar ();	

        //	IsDisable = false;

        //	IsCalled = true;

        //} else if (AdSystem.Instance.IsAdLoading == false && IsDisable == false || Contains.IsHavingRemoveAd  == true && IsDisable == false) {

        //	DisableBar ();

        //	IsCalled = false;

        //	IsDisable = true;

        //}
    }

    /// <summary>
    /// Raises the destroy event.
    /// </summary>
    protected override void OnDestroy()
    {
        base.OnDestroy();

        // Cancel all invoke have used in this gameobject.
        CancelInvoke();
    }

    #endregion

    #region Update

    /// <summary>
    /// Disables the ads.
    /// </summary>
    public void DisableAds()
    {
        // Check the condition if this game has removed ads.
        if (Contains.IsHavingRemoveAd)
        {
            // Hide Banner ads if it already turned on.
            //AdSystem.Instance.HideBanner ();

            // Hide Full-screen ads if it already turned on.
            //AdSystem.Instance.HideInterstitialAd ();
        }
    }

    /// <summary>
    /// Enables the bar.
    /// </summary>
    public void EnableBar()
    {
        // for ( int i = 0 ; i < barMenu.Length ; i++ )
        // {
        // 	print (barMenu [i].localPosition.y);
        //
        // 	// Kill all session using Dotween from barMenu.
        // 	barMenu[i].DOKill ();
        //
        // 	// Moving the barmenu to start position.
        // 	barMenu[i].DOLocalMoveY (200f, Contains.DurationFade);
        // }
    }

    public void DisableBar()
    {
        // for ( int i = 0 ; i < barMenu.Length ; i++ )
        // {
        // 	// Kill all session using Dotween from barMenu.
        // 	barMenu[i].DOKill ();
        //
        // 	// Moving the barmenu to start position.
        // 	barMenu[i].DOLocalMoveY (0, Contains.DurationFade);
        // }
    }


    /// <summary>
    /// Updates the time display.
    /// </summary>
    protected void UpdateTimeDisplay()
    {
        for (int i = 0; i < UITimeDisplay.Length; i++)
        {
            // Update time of game with UI.
            UITimeDisplay[i].text = "Time\n" + TimeSystem.Instance.GetTimeDisplay();
        }
    }

    /// <summary>
    /// Updates the score.
    /// </summary>
    public void UpdateScore(int param)
    {
        for (int i = 0; i < UIScoreDisplay.Length; i++)
        {
            // Update score of game with UI.
            UIScoreDisplay[i].text = string.Format("{0}", param);
        }

        UpdateBestScore();
    }

    /// <summary>
    /// Updates the move.
    /// </summary>
    /// <param name="param">Parameter.</param>
    public void UpdateMove(int param)
    {
        for (int i = 0; i < UIMoveDisplay.Length; i++)
        {
            // Update move of game with UI.
            UIMoveDisplay[i].text = string.Format("Moves\n{0}", param);
        }
    }

    public void UpdateBestScore()
    {
        foreach (var t in UIBestScoreDisplay) t.text = "All Time\n" + Contains.BestScoreSolitaire;
    }

    /// <summary>
    /// Enables the lose.
    /// </summary>
    public void EnableLose()
    {
        UIBanner.gameObject.SetActive(true);

        // Disable Banner Winning of game.
        UIWin.gameObject.SetActive(false);

        // Enable Banner losing of game.
        UILose.gameObject.SetActive(true);
        if (GameManager.Instance.PlayMode == Enums.PlayMode.Daily ||
            GameManager.Instance.PlayMode == Enums.PlayMode.Event)
        {
            SoundSystems.Instance.PlayerMusic(Enums.MusicIndex.LoseMusic, false);
            loseDailyEvent.SetActive(true);
        }
        else
        {
            SoundSystems.Instance.PlayerMusic(Enums.MusicIndex.WinMusic, false);
            loseNormal.SetActive(true);
        }

        // Hide all UI of game.
        // HudUI.DOFade(0, Contains.DurationFade);

        // Enable effect blur.
        GamePlay.Instance.EnableBlur();

        // Playing music losing.


        // Running the function restart after 4s.
        // Invoke(nameof(ReadyRestart), 4f);

        // Showing the full-screen ads after 2s. 
        // Invoke(nameof(InvokeShowAd), 2f);
    }

    /// <summary>
    /// Invokes the show ad.
    /// </summary>
    void InvokeShowAd()
    {
        // Showing the full-screen ads.
        //AdSystem.Instance.ShowInterstitialAd();
    }

    /// <summary>
    /// Disables the window.
    /// </summary>
    public void EnableWin()
    {
        UIBanner.gameObject.SetActive(true);

        // Enable Winning banner.
        UIWin.gameObject.SetActive(true);

        // Disable Losing banner.
        UILose.gameObject.SetActive(false);

        // Disable hud of game.
        // HudUI.gameObject.SetActive(false);

        // Fade Hud.
        // HudUI.DOFade(0, Contains.DurationFade);

        // Enable Blur.
        GamePlay.Instance.EnableBlur();

        // Playing winning sound.
        SoundSystems.Instance.PlayerMusic(Enums.MusicIndex.WinMusic, false);

        // Enable touch restart after 4s.
        // Invoke(nameof(ReadyRestart), 4f);

        // Enable Full-screen ads after 2s.
        // Invoke(nameof(InvokeShowAd), 2f);
    }

    /// <summary>
    /// Readies the restart.
    /// </summary>
    // public void ReadyRestart()
    // {
    //     IsReadyRestart = true;
    // }

    /// <summary>
    /// Restarts the game.
    /// </summary>
    public void RestartGame()
    {
        // Touch to restart the game.
        // if (IsReadyRestart)
        // {
        TouchEventSystem.Instance.DoRestartGame();
        // }
    }

    public void OnNewGameNormal()
    {
        SoundSystems.Instance.StopMusic();
        IronSourceManager.Instance.ShowInterstitialAd(RestartGame);
    }
    
    public void OnBackToHomeClick()
    {
        SoundSystems.Instance.StopMusic();
        IronSourceManager.Instance.ShowInterstitialAd(() =>
        {
            UIBanner.GetComponent<CanvasGroup>().blocksRaycasts = false;
            HomeSceneController.Instance.GetComponent<CanvasGroup>().blocksRaycasts = false;
            if (GameManager.Instance.GetStateGame() == Enums.StateGame.Win)
            {
                if (GameManager.Instance.PlayMode == Enums.PlayMode.Daily)
                {
                    HomeSceneController.Instance.OnShowDailyChallengeComplete();
                }
                else if (GameManager.Instance.PlayMode == Enums.PlayMode.Event)
                {
                    HomeSceneController.Instance.OnShowEventChallengeComplete();
                }
                else
                {
                    HomeSceneController.Instance.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
            }
            else
            {
                HomeSceneController.Instance.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }

            SceneLoader.Instance.UnloadScene(Contains.GamePlayScene);
        });
    }

    #endregion
}