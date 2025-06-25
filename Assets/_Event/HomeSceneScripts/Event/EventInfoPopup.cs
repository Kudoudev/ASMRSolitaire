using DanielLochner.Assets.SimpleScrollSnap;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Button = UnityEngine.UI.Button;

public class EventInfoPopup : MonoBehaviour
{
    public SimpleScrollSnap Scroll;
    public Button btnContinue;

    private void OnEnable()
    {
        Scroll.GoToPanel(0);
        btnContinue.transform.GetChild(0).GetComponent<TMP_Text>().text = "Continue";
    }

    public void OnChangeStep()
    {
        if(Scroll.CenteredPanel == 2) btnContinue.transform.GetChild(0).GetComponent<TMP_Text>().text = "OK";
        else btnContinue.transform.GetChild(0).GetComponent<TMP_Text>().text = "Continue";
    }

    public void OnContinueClick()
    {
        if (Scroll.CenteredPanel == 2)
        {
            gameObject.SetActive(false);
        }
    }
}