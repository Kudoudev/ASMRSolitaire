using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSticker : MonoBehaviour
{
    public Image render => GetComponent<Image>();

    public void DoShow(Sprite s)
    {
        // StartCoroutine(IDelay(() =>
        {
            render.DOKill();
            render.transform.DOScaleX(0f, 0f);
            render.DOFade(0f, 0f);
            render.rectTransform.pivot = new Vector2(0f, 0.5f);

            // render.sprite = s;
            render.transform.DOScaleX(1f, 0.25f);
            render.DOFade(1f, 0.35f);
        }
        // ));

    }

    IEnumerator IDelay(System.Action callback)
    {
        yield return new WaitForSeconds(0.032f);

        callback();
    }
    public void DoHide(Sprite s)
    {
        // StartCoroutine(IDelay(() =>
        {
            // render.sprite = s;
            render.DOKill();
            render.transform.DOScaleX(1f, 0f);
            render.DOFade(1f, 0f);

            render.rectTransform.pivot = new Vector2(1f, 0.5f);

            // render.transform.DOScaleX(1f, 0.25f);
            render.DOFade(0f, 0.35f);
        }
        // ));

    }
}
