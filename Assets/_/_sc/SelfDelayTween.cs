using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SelfDelayTween : MonoBehaviour
{
    public float mDelay = 0.1f;
    public float maxDelay = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    IEnumerator Start()
    {
        transform.DOScale(0, 0f);
        float delay = Random.Range(mDelay, maxDelay);
        yield return new WaitForSeconds(delay);
        GetComponent<DOTweenAnimation>().DOPlay();
     
    }

    
}
