using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using GlobalDefine;
using UnityEngine.UI;

public class GeneralUIManager : MonoBehaviour {
	
    public static GeneralUIManager instance = null;

    public GameObject BackButton;
    public GameObject MoneyInfo;
    public GameObject DialogUI;
	public GameObject LoadingMask;
    public Text DialogText;
	public Button DialogOkButton;
    public Button DialogLeftButton;
    public Button DialogRightButton;
	public RawImage loadingIconImg;

	private UnityAction leftBtnAct;
	private UnityAction rightBtnAct;

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
		DialogOkButton.onClick.AddListener(CloseDialogEvt);
		DialogLeftButton.onClick.AddListener(CloseDialogEvt);
		DialogRightButton.onClick.AddListener(CloseDialogEvt);
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

    public void ShowBackButton()
    {
        BackButton.SetActive(true);
    }

	public void HideBackButton()
	{
		BackButton.SetActive(false);
	}

	public void ShowMessage(string content)
	{		
		DialogText.text = content;
		DialogOkButton.gameObject.SetActive(true);
		DialogLeftButton.gameObject.SetActive(false);
		DialogRightButton.gameObject.SetActive(false);
		DialogUI.SetActive(true);
	}

	public void ShowDialog(string content, UnityAction leftClickEvt, UnityAction rightClickEvt)
    {
        DialogText.text = content;
        if (leftClickEvt != null) {
			DialogLeftButton.onClick.AddListener(leftClickEvt);
			leftBtnAct += leftClickEvt;
        }
		if (rightClickEvt != null) {
			DialogRightButton.onClick.AddListener(rightClickEvt);
			rightBtnAct += rightClickEvt;
		}

		DialogOkButton.gameObject.SetActive(false);
		DialogLeftButton.gameObject.SetActive(true);
		DialogRightButton.gameObject.SetActive(true);
        DialogUI.SetActive(true);
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

	public void ShowLoadingMask()
	{
		LoadingMask.SetActive (true);
	}

	public void CloseLoadingMask()
	{
		LoadingMask.SetActive (false);
	}

	void CloseDialogEvt()
	{
		// 清除暫時註冊的按鈕點擊事件
		if (leftBtnAct != null)
		{
			DialogLeftButton.onClick.RemoveListener(leftBtnAct);
			leftBtnAct = null;
		}
		if (rightBtnAct != null)
		{
			DialogRightButton.onClick.RemoveListener(rightBtnAct);
			rightBtnAct = null;
		}

		DialogUI.SetActive(false);
	}
}
