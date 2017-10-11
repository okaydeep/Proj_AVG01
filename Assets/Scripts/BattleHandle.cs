using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefine;
using System.Text;

public class Battle {

    public static int GetTeamDmg(List<CharBase> team)
    {
        int finalAtk = 0;

        for (int i = 0; i < team.Count; i++)
        {
            if (team[i].currentHP > 0)
                finalAtk += team[i].currentAtk;
        }

        return finalAtk;
    }

    public static void DoTeamDmg(int dmg, List<CharBase> takenDmgTeam)
    {
        for (int i = 0; i < takenDmgTeam.Count; i++)
        {
            if (dmg <= 0)
                break;

            if (takenDmgTeam[i].currentHP > 0)
            {
                if (dmg >= takenDmgTeam[i].currentHP)
                {
                    dmg -= takenDmgTeam[i].currentHP;
                    takenDmgTeam[i].currentHP = 0;
                }
                else
                {
                    takenDmgTeam[i].currentHP -= dmg;
                    break;
                }
            }
        }
    }

    public static bool AnyTeammateAlive(List<CharBase> team)
    {
        bool ret = true;

        int count = 0;
        for (int i = 0; i < team.Count; i++)
        {
            if (team[i].currentHP <= 0)
                count++;
        }

        if (count >= team.Count)
            ret = false;

        return ret;
    }

}

public class BattleHandle : MonoBehaviour
{

    public static BattleHandle instance;

    public int targetFloor;
    public bool autoUseItem;

    bool writeHistory;
    bool isBattling;
    bool isResultCalculating;

	int currentFloor;	      		// 目前樓層
    long startBattleTS;      		// 開始更新戰鬥結果的時間戳記
    long lastUpdateTS;       		// 上一次更新戰鬥結果的時間戳記
    public int updateCount;         // 已更新次數
    public int nextRoundLeftTime;   // 下次更新的剩餘時間
    int resultCalculatingTime;      // 計算結果花費的讀取時間

    List<CharBase> playerTeam;
    List<CharBase> enemyTeam;

    public delegate void CalaulateBattleFinish();

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        writeHistory = true;
		isBattling = false;
		isResultCalculating = false;

        playerTeam = new List<CharBase>();
        enemyTeam = new List<CharBase>();

        UpdateTimeDisp();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        //Debug.Log(pauseStatus);
    }

    // 戰鬥開始(按下探索按鈕)
    public void BattleStart()
    {
        // 如果有開始戰鬥時間的key, 則擋下
		if (PlayerPrefs.HasKey(GameSetting.START_BATTLE_TIME_KEY) == true || isBattling == true)
            return;

		isBattling = true;

        StartCoroutine(CalculatingTimeCount());

        if (File.Exists(GameSetting.BATTLE_HISTORY_FILEPATH))
            File.WriteAllText(GameSetting.BATTLE_HISTORY_FILEPATH, "");
        
        currentFloor = 1;
//      SetPlayerTeamData();
//      SetEnemyTeamByFloor(currentFloor);

		// 更新開始戰鬥時間戳記
		SetStartBattleTS();
		lastUpdateTS = startBattleTS;

		// 取得已經過時間內需要更新的次數
		updateCount = GetUpdateCountFromStart();
		BattleHistoryManager.instance.updateCountText.text = updateCount.ToString() + " 次";

		// 更新剩餘時間顯示
		SetLeftTimeDisplay(GameSetting.BATTLE_ROUND_TIME);

        BattleCountDown();
    }

    void UpdateTimeDisp()
    {
        if (PlayerPrefs.HasKey(GameSetting.START_BATTLE_TIME_KEY) == true)
        {
			startBattleTS = long.Parse(PlayerPrefs.GetString(GameSetting.START_BATTLE_TIME_KEY));
			lastUpdateTS = startBattleTS;

			Debug.Log(GeneralFunctions.GetNowTimestamp() - startBattleTS);

            updateCount = GetUpdateCountFromStart();
			BattleHistoryManager.instance.updateCountText.text = updateCount.ToString() + " 次";

            SetLeftTimeDisplay((int)(GeneralFunctions.GetNowTimestamp() - lastUpdateTS));
        }

        BattleCountDown();
    }

    void BattleCountDown()
    {
//        Invoke("UpdateNextRoundLeftTime", 1.0f);
        StartCoroutine(UpdateNextRoundLeftTime());
    }

    IEnumerator UpdateNextRoundLeftTime()
    {
        yield return new WaitForSeconds(1f);

        if (nextRoundLeftTime > 0)
			SetLeftTimeDisplay(-1);

        if (nextRoundLeftTime <= 0)
        {
            Debug.Log(GeneralFunctions.GetNowTimestamp() - lastUpdateTS);

            if (GeneralFunctions.GetNowTimestamp() - lastUpdateTS >= GameSetting.BATTLE_ROUND_TIME)
            {
                // 更新戰鬥
                updateCount = GetUpdateCountFromStart();
                BattleHistoryManager.instance.updateCountText.text = updateCount.ToString() + " 次";
				lastUpdateTS = lastUpdateTS + GameSetting.BATTLE_ROUND_TIME;
                SetLeftTimeDisplay(GameSetting.BATTLE_ROUND_TIME);
            }
        }

        BattleCountDown();
    }

    IEnumerator CalculatingTimeCount()
    {
		if (isResultCalculating == true)
            yield break;

		isResultCalculating = true;

		while (isResultCalculating)
        {
            yield return new WaitForSeconds(1f);
            resultCalculatingTime += 1;
        }
    }

	// 更新開始戰鬥時間戳記
	void SetStartBattleTS()
    {
		startBattleTS = GeneralFunctions.GetNowTimestamp();
		PlayerPrefs.SetString(GameSetting.START_BATTLE_TIME_KEY, startBattleTS.ToString());
        PlayerPrefs.Save();
    }

	// 取得已經過時間內需要更新的次數
	int GetUpdateCountFromStart()
	{
		return Mathf.FloorToInt((float)(GeneralFunctions.GetNowTimestamp() - startBattleTS) / (float)GameSetting.BATTLE_ROUND_TIME);
	}

    // 更新戰鬥時戳
    void UpdateTimestamp(long newTS = 0)
    {
        if (newTS == 0f)
			lastUpdateTS = startBattleTS;
        else
            lastUpdateTS = newTS;
    }

	// 更新剩餘時間顯示
    void SetLeftTimeDisplay(int time)
    {
        // 更新剩餘時間
        nextRoundLeftTime = time;

        // 更新顯示
        BattleHistoryManager.instance.updateLeftTimeText.text = nextRoundLeftTime + " 秒";
    }

    float GetResultCalculatingTime()
    {
		isResultCalculating = false;
        float ret = resultCalculatingTime;
        resultCalculatingTime = 0;
        return ret;
    }

    void BattleResult()
    {
        PlayerAttack();
    }

    void PlayerAttack()
    {
        int dmg = Battle.GetTeamDmg(playerTeam);
        Battle.DoTeamDmg(dmg, enemyTeam);

        if (Battle.AnyTeammateAlive(enemyTeam) == true)
        {
            EnemyAttack();
        }
        else
        {
            // 玩家勝利, 樓層突破
            currentFloor++;

            if (currentFloor >= targetFloor)
            {
                // 到達目標樓層, 結束探索
            }
            else
            {
                UpdateTimestamp((long)GameSetting.BATTLE_ROUND_TIME);
                SetLeftTimeDisplay(GameSetting.BATTLE_ROUND_TIME);
                SetEnemyTeamByFloor(currentFloor);
                BattleCountDown();
            }
        }
    }

    void EnemyAttack()
    {
        int dmg = Battle.GetTeamDmg(enemyTeam);
        Battle.DoTeamDmg(dmg, playerTeam);

        if (Battle.AnyTeammateAlive(playerTeam))
        {
            // 重置顯示時間, 準備下一回合
            UpdateTimestamp((long)GameSetting.BATTLE_ROUND_TIME);
            SetLeftTimeDisplay(GameSetting.BATTLE_ROUND_TIME);
            BattleCountDown();
        }
        else
        {
            // 敵人勝利, 結束探索
        }
    }

    void StartBattleUpdate()
    {
        StartCoroutine(BattleUpdate());
    }

    // 一般戰鬥回合更新
    IEnumerator BattleUpdate()
    {
        if (enemyTeam.Count <= 0)
        {
            SaveHistory(currentFloor.ToString() + " floor passed!");
            currentFloor++;
            SetEnemyTeamByFloor(currentFloor);
            Invoke("StartBattleUpdate", GameSetting.BATTLE_ROUND_TIME);
            yield break;
        }

        if (playerTeam.Count <= 0)
        {
            SaveHistory(currentFloor.ToString() + " floor failed!");
            // TODO 結算, 更新玩家資料
            yield break;
        }

        Invoke("StartBattleUpdate", GameSetting.BATTLE_ROUND_TIME);

        yield return null;
    }

    // 戰鬥回合更新(計算用)
    IEnumerator CalculateBattleUpdate()
    {
        if (enemyTeam.Count <= 0)
        {
            SaveHistory(currentFloor.ToString() + " floor passed!");
            currentFloor++;
            SetEnemyTeamByFloor(currentFloor);
            yield break;
        }

        if (playerTeam.Count <= 0)
        {
            SaveHistory(currentFloor.ToString() + " floor failed!");
            yield break;
        }

        yield return null;
    }

    // 取得玩家隊伍資料
    void SetPlayerTeamData()
    {
        if (playerTeam != null)
            playerTeam.Clear();
        
        // official
//        PlayerData playerData = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));
//        int teamMemberCount = playerData.teamData;
//        for (int i = 1; i <= teamMemberCount; i++)
//        {
//            string dataPath = "character" + i;
//            Character character = (Character)PlayerDataManager.instance.Load(dataPath, typeof(Character));
//            CharBase chb = new CharBase();
//            chb.currentHP = character.equipHP;
//            chb.currentAtk = character.equipAtk;
//            chb.currentDef = character.equipDef;
//            playerTeam.Add(chb);
//
//        }

        // test
        Character chr = new Character();
        chr.chrName = "Henry";
        chr.equipHP = 300;
        chr.equipAtk = 6;
        chr.equipDef = 2;
        playerTeam.Add(chr);
        chr = new Character();
        chr.chrName = "Davis";
        chr.equipHP = 200;
        chr.equipAtk = 7;
        chr.equipDef = 1;
        playerTeam.Add(chr);
        chr = new Character();
        chr.chrName = "Woody";
        chr.equipHP = 350;
        chr.equipAtk = 4;
        chr.equipDef = 4;
        playerTeam.Add(chr);

        playerTeam = playerTeam.OrderBy(val => val.currentHP).ToList();
    }

    // 根據樓層取得敵人隊伍資料
    void SetEnemyTeamByFloor(int floor)
    {
        if (enemyTeam != null)
            enemyTeam.Clear();

        // test
        for (int i = 0; i < 3; i++)
        {
            Monster mob = new Monster();
            mob.equipHP = floor * 10 + 5;
            mob.equipAtk = floor * 3 + 2;
            mob.equipDef = floor * 1 + 1;
            CharBase chb = new CharBase();
            chb.currentHP = mob.equipHP;
            chb.currentAtk = mob.equipAtk;
            chb.currentDef = mob.equipDef;
            enemyTeam.Add(chb);
        }

        enemyTeam = enemyTeam.OrderBy(val => val.currentHP).ToList();
    }

    // 儲存紀錄
    public void SaveHistory(string msg)
    {
        if (writeHistory == false)
            return;

        using (StreamWriter sw = (File.Exists(GameSetting.BATTLE_HISTORY_FILEPATH)) ? File.AppendText(GameSetting.BATTLE_HISTORY_FILEPATH) : File.CreateText(GameSetting.BATTLE_HISTORY_FILEPATH))
        {
            sw.WriteLine(msg);
        }

        Debug.Log(msg);
        BattleHistoryManager.instance.AddMsgToList(msg);
    }

    // 讀取紀錄
    public string[] LoadHistory()
    {
        StreamReader r = File.OpenText(GameSetting.BATTLE_HISTORY_FILEPATH);
        string info = r.ReadToEnd();
        r.Close();

        string[] history = info.Split('\n');
        return history;
    }

    public void StartCalculateBattleResult(CalaulateBattleFinish finishEvt)
    {
        StartCoroutine(CalculateBattleResult(finishEvt));
    }

    IEnumerator CalculateBattleResult(CalaulateBattleFinish finishEvt)
    {
        writeHistory = false;

        if (PlayerPrefs.HasKey(GameSetting.START_BATTLE_TIME_KEY))
            lastUpdateTS = long.Parse(PlayerPrefs.GetString(GameSetting.START_BATTLE_TIME_KEY, "0"));
        
        if (lastUpdateTS <= 0)
        {
            if (finishEvt != null)
                finishEvt();
            yield break;
        }

        currentFloor = 1;
        SetPlayerTeamData();
        SetEnemyTeamByFloor(currentFloor);

        long finalTime = GeneralFunctions.GetNowTimestamp();
        long timeDisp = finalTime - lastUpdateTS;
        int allUpdateTimes = Mathf.FloorToInt((float)timeDisp / (float)GameSetting.BATTLE_ROUND_TIME);

        for (int i = 0; i < allUpdateTimes; i++)
        {
            if (playerTeam.Count <= 0)
            {
                // TODO 結算, 更新玩家資料
                if (finishEvt != null)
                    finishEvt();
                yield break;
            }

            yield return StartCoroutine(CalculateBattleUpdate());
        }

        if (finishEvt != null)
            finishEvt();
        Invoke("StartBattleUpdate", GameSetting.BATTLE_ROUND_TIME);
    }
}
