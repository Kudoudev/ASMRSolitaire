using UnityEngine;
using System.Collections;

public class ScreenShotScript : MonoBehaviour {
	public string ImagePrefix = "Image ";
	int imageName = 0;
	public int resolutionX = 2;


	public float VideoRecordingFPS = 12.0f;

	// Use this for initialization
	void Start () {
		imageName = PlayerPrefs.GetInt ("ImageName");
	}	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.F))
		{
			TakeScreenShot();
		}

		if (Input.GetKeyDown (KeyCode.I)) {
				
			InvokeRepeating("TakeScreenShot", 1.0f/VideoRecordingFPS, 1.0f/VideoRecordingFPS);
		
		}
	
		if (Input.GetKeyDown (KeyCode.O)) {
			

			CancelInvoke("TakeScreenShot");
		}

	
	}
	void TakeScreenShot()
	{

		imageName++;
		PlayerPrefs.SetInt ("ImageName", imageName);
		print("meknvlkefnvldknv");
		ScreenCapture.CaptureScreenshot(ImagePrefix+imageName+".png", resolutionX);

	}
	void OnMouseDown()
	{
	}
}
