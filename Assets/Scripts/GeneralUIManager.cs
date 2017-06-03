using UnityEngine;
using System.Collections;
using GlobalDefine;
using UnityEngine.UI;
public class GeneralUIManager : MonoBehaviour {

    public static GeneralUIManager instance = null;
    public GameObject MoneyInfo;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }    
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowMoneyInfo(bool bol)
    {
        MoneyInfo.SetActive(bol);
    }

    public void SetMoneyInfo(string str)
    {
        MoneyInfo.transform.FindChild("moneyContent").GetComponent<Text>().text=str;
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
