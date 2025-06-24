using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NoAdsPack : MonoBehaviour
{
    public List<GameObject> selected;
    public List<GameObject> deselected;
    public Button click => GetComponent<Button>();
    [SerializeField] bool select;
    float dst = 20f;
    Vector3 originalPosition;

    void Awake()
    {
        // Store the original position
        originalPosition = transform.localPosition;
    }

    public void Select()
    {
        if (select)
        {
            Debug.LogWarning("selected - select true");
            return;
        }
        else
        {
            Debug.LogWarning("selected - select false");
        }

        select = true;
        selected.ForEach(s => s.gameObject.SetActive(true));
        deselected.ForEach(s => s.gameObject.SetActive(false));

        // Kill any existing animations before starting new ones
        transform.DOKill();
        // Move to absolute position (original + offset)
        transform.DOLocalMoveY(originalPosition.y + dst, 0.35f);
        transform.DOScale(1.15f, 0.35f);
    }

    public void DeSelect()
    {
        selected.ForEach(s => s.gameObject.SetActive(false));
        deselected.ForEach(s => s.gameObject.SetActive(true));

        if (select)
        {
            Debug.LogWarning("deselect - select true");
            transform.DOKill();
            // Move back to original absolute position
            transform.DOLocalMoveY(originalPosition.y, 0.35f);
            transform.DOScale(1f, 0.35f);
            select = false;
        }
        else
        {
            Debug.LogWarning("deselect - select false");
        }
    }
}