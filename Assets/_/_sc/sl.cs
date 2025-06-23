using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sl : MonoBehaviour
{

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(LoadScene);
    }
    void LoadScene()
    {
        SceneManager.LoadScene(1);
    }
}
