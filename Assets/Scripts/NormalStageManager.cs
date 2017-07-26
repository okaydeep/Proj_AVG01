using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GlobalDefine;

public class NormalStageManager : MonoBehaviour {

	private int targetFloor;
	private int[] changeTargetFloorArr = new int[]{100, 500, -100, -500};

	public Toggle useItemToggle;
	public Text targetFloorText;
	public Button[] changeTargetFloorBtns;

	// Use this for initialization
	void Start () {
		UIEventInit ();
	}

	void OnEnable () {
		StatusInit ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void StatusInit()
	{
		targetFloor = 100;
		targetFloorText.text = targetFloor.ToString();
	}

	void UIEventInit()
	{
		// 自動使用道具切換事件
		useItemToggle.onValueChanged.AddListener((isOn) => {
			BattleHandle.instance.autoUseItem = isOn;
		});

		// 更改目標樓層按鈕事件
		for (int i = 0; i < changeTargetFloorArr.Length; i++) {
			int idx = i;
			changeTargetFloorBtns [idx].onClick.AddListener (() => changeTargetFloorBtnEvent (idx));
		}
	}

	void changeTargetFloorBtnEvent(int idx)
	{
		targetFloor += changeTargetFloorArr[idx];

		if (targetFloor > GameSetting.NORMALSTAGE_MAX_FLOOR)
			targetFloor = GameSetting.NORMALSTAGE_MAX_FLOOR;
		else if (targetFloor < 100)
			targetFloor = 100;

		targetFloorText.text = targetFloor.ToString ();
	}

	public void StartTravel()
	{
		PlayerData playerData = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));
		if (playerData.teamData <= 0) {
			GeneralUIManager.instance.ShowMessage("沒有可探索的成員");
			return;
		}

        BattleHandle.instance.autoUseItem = useItemToggle.isOn;
        BattleHandle.instance.targetFloor = targetFloor;
        BattleHandle.instance.BattleStart();
	}
}
