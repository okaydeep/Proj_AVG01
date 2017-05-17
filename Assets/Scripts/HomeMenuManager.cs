using UnityEngine;
using System.Collections;

public class HomeMenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Resume()
	{
		CanvasManager.instance.ShowCanvas (GlobalDefine.GCanvas.BattleMenu);
	}

	public void NewTravel()
	{
		CanvasManager.instance.ShowCanvas (GlobalDefine.GCanvas.BattleMenu);
	}

	public void TurnToSetting()
	{
		CanvasManager.instance.ShowCanvas (GlobalDefine.GCanvas.Setting);
	}
}
