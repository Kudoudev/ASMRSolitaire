using UnityEngine;
using UnityEngine.UI;

public class LC : MonoBehaviour
{
    public string link;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Application.OpenURL(link);
        });
        
    }

}
