using System.Collections;
using System.Collections.Generic;
using MovementEffects;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : SingletonMonoBehaviour<SceneLoader>
{
    [SerializeField] private CanvasGroup canvasGroup;

    private bool isFading = false;
    public void ShowFade()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha = Mathf.Clamp(canvasGroup.alpha + Time.deltaTime * 2, 0, 1);
        }
    }

    public void LoadScene(string sceneLoad, LoadSceneMode mode = LoadSceneMode.Single, bool isFade = true)
    {
        // transform.gameObject.SetActive(true);
        // canvasGroup.alpha = 0;
        Timing.RunCoroutine(LoadYourAsyncScene(sceneLoad, mode, isFade));
    }

    private IEnumerator<float> LoadYourAsyncScene(string scene, LoadSceneMode mode = LoadSceneMode.Single,
        bool isFade = true)
    {
        while (isFading)
        {
            yield return 0f;
        }
        if (isFade)
        {
            isFading = true;
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha = Mathf.Clamp(canvasGroup.alpha + Time.deltaTime * 2, 0, 1);
                yield return 0f;
            }
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene, mode);

        while (!asyncLoad.isDone)
        {
            yield return 0f;
        }

        if (isFade)
        {
            canvasGroup.alpha = 1;

            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha = Mathf.Clamp(canvasGroup.alpha - Time.deltaTime * 2, 0, 1);
                yield return 0f;
            }
            canvasGroup.alpha = 0;
            isFading = false;
        }
        // transform.gameObject.SetActive(false);
    }

    public void UnloadScene(string sceneLoad, bool isFade = true)
    {
        if (sceneLoad == Contains.GamePlayScene)
        {
            SoundSystems.Instance.StopMusic();
            HomeSceneController.Instance.ReloadDailyEvent();
        }
        Timing.RunCoroutine(UnoadYourAsyncScene(sceneLoad, isFade));
    }


    private IEnumerator<float> UnoadYourAsyncScene(string scene, bool isFade = true)
    {
        while (isFading)
        {
            yield return 0f;
        }
        if (isFade)
        {
            isFading = true;
            while (canvasGroup.alpha < 1)
            {
                canvasGroup.alpha = Mathf.Clamp(canvasGroup.alpha + Time.deltaTime * 2, 0, 1);
                yield return 0f;
            }
        }

        AsyncOperation asyncLoad = SceneManager.UnloadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {
            yield return 0f;
        }
        
        if (isFade)
        {
            canvasGroup.alpha = 1;

            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha = Mathf.Clamp(canvasGroup.alpha - Time.deltaTime * 2, 0, 1);
                yield return 0f;
            }
            canvasGroup.alpha = 0;
            isFading = false;
        }
        // transform.gameObject.SetActive(false);
    }
}