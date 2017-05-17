using UnityEngine;
using System.Collections;

public class LoadingImageManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("turnToHomeMenu", 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void turnToHomeMenu()
	{
		CanvasManager.instance.ShowCanvas (GlobalDefine.GCanvas.HomeMenu);
	}
}
