using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using MovementEffects;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// Loading behaviour.
/// </summary>
public class LoadingBehaviour : SingletonMonoBehaviour < LoadingBehaviour > {

	// =============================== References ============================== //


	[Header("Controller")]

	/// <summary>
	/// The canvas group.
	/// </summary>
	[SerializeField]
	private CanvasGroup canvasGroup;

	/// <summary>
	/// The user interface loading process.
	/// </summary>
	[SerializeField]
	private Image UILoadingProcess;

	/// <summary>
	/// The user interface loading information.
	/// </summary>
	[SerializeField]
	private Text UILoadingInformation;

	/// <summary>
	/// The on start loading.
	/// </summary>
    [HideInInspector]
    public System.Action OnStartLoading;

	// =============================== Functional ============================== //
	#region Functional 
	/// <summary>
	/// Shows the loading.
	/// </summary>
	/// <param name="sceneLoad">Scene load.</param>
	/// <param name="isFade">If set to <c>true</c> is fade.</param>
	/// <param name="isUseSplashScreen">If set to <c>true</c> is use splash screen.</param>
	/// <param name="message">Message.</param>
	public void ShowLoading(string sceneLoad , bool isFade = true , bool isUseSplashScreen = true ,string message = "")
	{
		transform.gameObject.SetActive(true);

		UILoadingInformation.text = message;

		canvasGroup.alpha = 0;

		UILoadingProcess.fillAmount = 0;

		Timing.RunCoroutine  (LoadingTime (sceneLoad , isFade , isUseSplashScreen)) ;
	}

	/// <summary>
	/// Loadings the time.
	/// </summary>
	/// <returns>The time.</returns>
	/// <param name="sceneLoad">Scene load.</param>
	/// <param name="isFade">If set to <c>true</c> is fade.</param>
	/// <param name="isUseSplashScreen">If set to <c>true</c> is use splash screen.</param>
	private IEnumerator < float > LoadingTime(string sceneLoad , bool isFade , bool isUseSplashScreen )
	{

        if (isUseSplashScreen)
        {

            if (isFade)
            {

                while (canvasGroup.alpha < 1)
                {

                    canvasGroup.alpha = Mathf.Clamp(canvasGroup.alpha + Time.deltaTime, 0, 1);

                    yield return 0f;
                }

            }
            else
            {

                yield return 2f;
            }
        }

        if (OnStartLoading != null)
        {
            OnStartLoading();

            OnStartLoading = null;
        }


        if (isUseSplashScreen)
        {

            AsyncOperation async = SceneManager.LoadSceneAsync(sceneLoad, LoadSceneMode.Single);

            while (!async.isDone)
            {
                UILoadingProcess.fillAmount = async.progress;

                yield return 0f;
            }

            UILoadingProcess.fillAmount = 1;

            canvasGroup.alpha = 1;

            while (canvasGroup.alpha > 0)
            {
                canvasGroup.alpha = Mathf.Clamp(canvasGroup.alpha - Time.deltaTime * 2, 0, 1);

                yield return 0f;
            }

        }else
        {
            SceneManager.LoadScene(sceneLoad, LoadSceneMode.Single);
        }

		transform.gameObject.SetActive(false);
	}
	#endregion
}