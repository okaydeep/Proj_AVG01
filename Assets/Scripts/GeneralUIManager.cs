using UnityEngine;
using System.Collections;
using GlobalDefine;

public class GeneralUIManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Back()
	{
		int showingIdx = CanvasManager.instance.GetShowingCanvasIdx ();

        switch (showingIdx)
        {
            case (int)GCanvas.Setting:
                CanvasManager.instance.ShowCanvas(GCanvas.HomeMenu);
                break;
            case (int)GCanvas.TeamInfo:
                CanvasManager.instance.ShowCanvas(GCanvas.BattleMenu);
                break;
            case (int)GCanvas.NormalStage:
                CanvasManager.instance.ShowCanvas(GCanvas.BattleMenu);
                break;
            case (int)GCanvas.Market:
                CanvasManager.instance.ShowCanvas(GCanvas.BattleMenu);
                break;
            case (int)GCanvas.BattleHistory:
                CanvasManager.instance.ShowCanvas(GCanvas.BattleMenu);
                break;
            default:
                break;
        }
	}
}
