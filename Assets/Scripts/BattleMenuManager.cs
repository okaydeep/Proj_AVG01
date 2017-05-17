using UnityEngine;
using System.Collections;

public class BattleMenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ShowTeamInfo()
	{
		CanvasManager.instance.ShowCanvas (GlobalDefine.GCanvas.TeamInfo);
	}

	public void ShowStageMap()
	{
		CanvasManager.instance.ShowCanvas (GlobalDefine.GCanvas.NormalStage);
	}

	public void ShowMarket()
	{
		CanvasManager.instance.ShowCanvas (GlobalDefine.GCanvas.Market);
	}

	public void ShowBattleHistory()
	{
		CanvasManager.instance.ShowCanvas (GlobalDefine.GCanvas.BattleHistory);
	}
}
