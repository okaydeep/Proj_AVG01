using UnityEngine;
using System.Collections;
using GlobalDefine;

public class CanvasManager : MonoBehaviour {

	public static CanvasManager instance = null;

	/*
	 * 0.讀取畫面
	 * 1.主選單
	 * 2.遊戲選單
	 * 3.隊伍資訊
	 * 4.關卡
	 * 5.市場
	 * 6.商店
	 */
	public GameObject[] CanvasObjs;
    public GameObject GeneralCanvasObj;

    private int showingCanvasIdx;

	void Awake () {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        showingCanvasIdx = -1;
	}

	// Use this for initialization
	void Start () {
        initCanvasDisplay(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public int GetShowingCanvasIdx()
	{
		return showingCanvasIdx;
	}

    public void ShowCanvas(GCanvas canvas)
    {
        if (showingCanvasIdx < 0 || showingCanvasIdx == (int)canvas)
            return;

        // 最後一次顯示的canvas隱藏
        CanvasObjs[showingCanvasIdx].SetActive(false);

        // 更新目前顯示的canvas index
        showingCanvasIdx = (int)canvas;

        // 顯示canvas
        CanvasObjs[(int)canvas].SetActive(true);

        if (canvas == GCanvas.BattleHistory ||
            canvas == GCanvas.Market ||
            canvas == GCanvas.NormalStage ||
            canvas == GCanvas.Setting ||
            canvas == GCanvas.TeamInfo)
        {
            GeneralCanvasObj.SetActive(true);
        }
        else
        {
            GeneralCanvasObj.SetActive(false);
        }
    }

    void initCanvasDisplay(int showCanvasIdx)
    {
        if (showCanvasIdx >= 0)
        {
            for (int i = 0; i < CanvasObjs.Length; i++)
            {
                if (i == showCanvasIdx)
                {
                    showingCanvasIdx = i;
                    CanvasObjs[i].SetActive(true);
                }
                else
                {
                    CanvasObjs[i].SetActive(false);
                }
            }
        }

        GeneralCanvasObj.SetActive(false);
    }
}
