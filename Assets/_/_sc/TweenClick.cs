using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class TweenClick : MonoBehaviour
{
    public List<DOTweenAnimation> tweens;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => DoWork());
    }

    void DoWork()
    {

        foreach (var t in tweens)
            t.DOPlay();
    }

}
