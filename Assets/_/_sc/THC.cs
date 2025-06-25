using UnityEngine;

public class THC : MonoBehaviour
{
    public Vector2 pos;

    void OnEnable()
    {
        GetComponent<RectTransform>().anchoredPosition = pos;
    }
}
