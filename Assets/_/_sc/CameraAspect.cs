using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAspect : MonoBehaviour
{
    [Tooltip("The original design resolution width (e.g., 1242 for 1242x2208).")]
    public static float designWidth = 1080;

    [Tooltip("The original design resolution height (e.g., 2208 for 1242x2208).")]
    public static float designHeight = 1920f;

    private Camera cam;
    float defaultSize;


    static public float ORIGIN_ASPECT  => designHeight / designWidth;
    public static float ASPECT_RATIO => 1;//1 + (Mathf.Clamp(Screen.height / (float)Screen.width, 1.77777777778f, 3f) - ORIGIN_ASPECT);
    public static float DICK_ASPECT_RATIO => 1 + ((Screen.height / (float)Screen.width) - ORIGIN_ASPECT);

    void Start()
    {
        cam = GetComponent<Camera>();
        defaultSize = cam.orthographicSize;
        if (cam.orthographic)
        {
            FitCameraToAspect();
        }
        else
        {
            Debug.LogError("This script works only with orthographic cameras.");
        }
    }

    void Update()
    {
        #if UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.U))
        {
            FitCameraToAspect();

            Debug.LogError(ASPECT_RATIO);
        }
        #endif
    }
    void FitCameraToAspect()
    {
       

        var k = defaultSize / (designHeight / designWidth);
        var newAspect = Mathf.Clamp(Screen.height / (float)Screen.width, 1.77777777778f, 3f);
        cam.orthographicSize = k * newAspect;
    }

}
