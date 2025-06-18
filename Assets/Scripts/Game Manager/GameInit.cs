using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        LoadingBehaviour.Instance.ShowLoading(Contains.HomeScene);
    }
}