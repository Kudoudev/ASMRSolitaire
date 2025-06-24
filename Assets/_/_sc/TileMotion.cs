using UnityEngine;
using DG.Tweening;

public class TileMotion : MonoBehaviour
{
    [Header("Motion Settings")]
    public float floatHeight = 0.05f;
    public float floatDuration = 2f;
    public float scaleAmount = 0.05f;
    public float scaleDuration = 3f;
    public float rotateAmount = 5f;
    public float rotateDuration = 4f;

    private Vector3 originalPosition;
    private Vector3 originalScale;

    void Start()
    {
        originalPosition = transform.localPosition;
        originalScale = transform.localScale;

        AnimateFloating();
        AnimateScaling();
        AnimateRotation();
    }

    void AnimateFloating()
    {
        transform.DOLocalMoveY(originalPosition.y + floatHeight, floatDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    void AnimateScaling()
    {
        transform.DOScale(originalScale * (1 + scaleAmount), scaleDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutQuad);
    }

    void AnimateRotation()
    {
        transform.DOLocalRotate(new Vector3(0, 0, rotateAmount), rotateDuration, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
