using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

/// <summary>
/// Controller everything about touch.
/// </summary>
public class TouchEventSystem : SingletonMonoBehaviour<TouchEventSystem>
{
    protected bool IsWaitingPress;

    #region Button Function

    public void TouchQuitGame()
    {
        if (!GameManager.Instance.IsGameReady()) return;
        SoundSystems.Instance.PlaySound(Enums.SoundIndex.Press);
        SceneLoader.Instance.UnloadScene(Contains.GamePlayScene);
        HomeSceneController.Instance.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    /// <summary>
    /// Restart the game.
    /// </summary>
    public void TouchRestartGame()
    {
        SoundSystems.Instance.PlaySound(Enums.SoundIndex.Press);

        DialogSystem.Instance.ShowYesNo("Solitaire", "Do you really want to restart the game?",
            () => { DoRestartGame(); });
    }

    public void DoRestartGame()
    {
        GamePlay.Instance.StopAllCoroutine();

        // LoadingBehaviour.Instance.OnStartLoading = () =>
        // {
        List<CardBehaviour> cardFound = new List<CardBehaviour>(PoolSystem.Instance.GetAllCards());

        for (int i = 0; i < cardFound.Count; i++)
        {
            PoolSystem.Instance.ReturnToPool(cardFound[i]);

            cardFound[i].UnlockCard(false);
        }
        // };

        SoundSystems.Instance.StopMusic();
        SceneLoader.Instance.ShowFade();
        SceneLoader.Instance.UnloadScene(Contains.GamePlayScene, false);
        SceneLoader.Instance.LoadScene(Contains.GamePlayScene, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Show hint for game with the condition.
    /// </summary>
    public void TouchHintGame()
    {
        if (!GameManager.Instance.IsGameReady()) return;

        if (Contains.HintAmmount > 0)
        {
            HelpManager.Instance.UpdateHint(-1);
            SoundSystems.Instance.PlaySound(Enums.SoundIndex.Press);

            GamePlay.Instance.ShowHintGame();
        }
        else
        {
            IronSourceManager.Instance.ShowRewardedAds(() =>
            {
                HelpManager.Instance.UpdateHint(3);
            });
        }
        
}

    /// <summary>
    /// Dos the show dialog themes.
    /// </summary>
    public void DoShowDialogThemes()
    {
        DialogSystem.Instance.ShowDialogThemes();
    }

    /// <summary>
    /// Touchs the show menu settings.
    /// </summary>
    /// <param name="animation">Animation.</param>
    public void TouchShowMenuSettings(Animator animation)
    {
        //AdsManager.instance.ShowInterstitial ();
        SoundSystems.Instance.PlaySound(Enums.SoundIndex.Press);

        if (animation.GetBool("IsAppear") == false)
        {
            animation.SetBool("IsAppear", true);
        }
    }

    /// <summary>
    /// Touchs the hide menu settings.
    /// </summary>
    /// <param name="animation">Animation.</param>
    public void TouchHideMenuSettings(Animator animation)
    {
        SoundSystems.Instance.PlaySound(Enums.SoundIndex.Press);

        if (animation.GetBool("IsAppear") == true)
        {
            animation.SetBool("IsAppear", false);
        }
    }

    /// <summary>
    /// Touchs the undo.
    /// </summary>
    public void TouchUndo()
    {
        if (!GameManager.Instance.IsGameReady())
            return;

        if (Contains.UndoAmmount > 0)
        {
            HelpManager.Instance.UpdateUndo(-1);
            
            SoundSystems.Instance.PlaySound(Enums.SoundIndex.Press);

            GamePlay.Instance.DisableHintGame();

            UndoSystem.Instance.UndoState();
        }
        else
        {
            IronSourceManager.Instance.ShowRewardedAds(() =>
            {
                HelpManager.Instance.UpdateUndo(3);
            });
        }
    }

    /// <summary>
    /// Touchs the show hint cards.
    /// </summary>
    public void TouchShowHintCards()
    {
        if (GameManager.Instance.IsGameReady())
        {
            SoundSystems.Instance.PlaySound(Enums.SoundIndex.Press);

            HidenCardsManager.Instance.DoShowingLockedCards();
        }
    }

    /// <summary>
    /// Enables the game object.
    /// </summary>
    /// <param name="param">Parameter.</param>
    public void EnableGameObject(GameObject param)
    {
        param.gameObject.SetActive(true);
    }

    /// <summary>
    /// Disables the game object.
    /// </summary>
    /// <param name="param">Parameter.</param>
    public void DisableGameObject(GameObject param)
    {
        param.gameObject.SetActive(false);
    }

    /// <summary>
    /// Touchs the dialog options.
    /// </summary>
    public void TouchDialogOptions()
    {
        SoundSystems.Instance.PlaySound(Enums.SoundIndex.Press);

        DialogSystem.Instance.ShowDialogOptions();
    }

    /// <summary>
    /// Touchs the game hard.
    /// </summary>
    public void TouchGameHard()
    {
        if (GameManager.Instance.ModeGame != Enums.ModeGame.None)
            return;

        SoundSystems.Instance.PlaySound(Enums.SoundIndex.Press);

        GameManager.Instance.ModeGame = Enums.ModeGame.Hard;

        SoundSystems.Instance.StopMusic();
    }

    /// <summary>
    /// Touchs the game easy.
    /// </summary>
    public void TouchGameEasy()
    {
        if (GameManager.Instance.ModeGame != Enums.ModeGame.None)
            return;

        SoundSystems.Instance.PlaySound(Enums.SoundIndex.Press);

        GameManager.Instance.ModeGame = Enums.ModeGame.Easy;
        //AdsManager.instance.ShowInterstitial ();
        SoundSystems.Instance.StopMusic();
    }

    /// <summary>
    /// Touchs the disable canvas group.
    /// </summary>
    /// <param name="canvas">Canvas.</param>
    public void TouchDisableCanvasGroup(CanvasGroup canvas)
    {
        canvas.DOFade(0, Contains.DurationFade).OnComplete(() =>
        {
            canvas.gameObject.SetActive(false);

            if (GamePlay.Instance != null)
            {
                GamePlay.Instance.DisableBlur();
            }
        });
    }

    #endregion
}