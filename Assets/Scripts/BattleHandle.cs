using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalDefine;

public class BattleHandle : MonoBehaviour
{

    public static BattleHandle instance;

    public int targetFloor;
    public bool autoUseItem;

    bool traveling;
    bool writeHistory;
    long startBattleTime;  // 最後一次戰鬥的時間
    int currentFloor;
    int alreadyUpdateTimes;     // 已經更新過的次數, app開啟時, 戰鬥更新的次數
    List<Character> playerTeam;
    List<Monster> enemyTeam;

    public delegate void CalaulateBattleFinish();

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        traveling = false;
        writeHistory = true;

        playerTeam = new List<Character>();
        enemyTeam = new List<Monster>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void _getPlayerData()
    {

    }

    // 更新戰鬥時戳
    void UpdateBattleTime()
    {
        startBattleTime = GeneralFunctions.GetNotTimestamp();

        PlayerPrefs.SetString(GameSetting.START_BATTLE_TIME_KEY, startBattleTime.ToString());
        PlayerPrefs.Save();
    }

    // 戰鬥開始(按下探索按鈕)
    public void BattleStart()
    {
        if (File.Exists(GameSetting.BATTLE_HISTORY_FILEPATH))
            File.WriteAllText(GameSetting.BATTLE_HISTORY_FILEPATH, "");

        currentFloor = 1;
        alreadyUpdateTimes = 0;
        PlayerPrefs.SetInt(GameSetting.ALREADY_UPDATE_TIMES_KEY, alreadyUpdateTimes);
        PlayerPrefs.Save();
        SetPlayerTeamData();
        SetEnemyTeamByFloor(currentFloor);
        UpdateBattleTime();
        Invoke("StartBattleUpdate", GameSetting.BATTLE_ROUND_TIME);
    }

    void StartBattleUpdate()
    {
        StartCoroutine(BattleUpdate());
    }

    // 一般戰鬥回合更新
    IEnumerator BattleUpdate()
    {
        alreadyUpdateTimes++;
        PlayerPrefs.SetInt(GameSetting.ALREADY_UPDATE_TIMES_KEY, alreadyUpdateTimes);
        PlayerPrefs.Save();

        PlayerTeamAttack();

        if (enemyTeam.Count <= 0)
        {
            SaveHistory(currentFloor.ToString() + " floor passed!");
            currentFloor++;
            SetEnemyTeamByFloor(currentFloor);
            Invoke("StartBattleUpdate", GameSetting.BATTLE_ROUND_TIME);
            yield break;
        }

        EnemyTeamAttack();
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
        PlayerTeamAttack();

        if (enemyTeam.Count <= 0)
        {
            SaveHistory(currentFloor.ToString() + " floor passed!");
            currentFloor++;
            SetEnemyTeamByFloor(currentFloor);
            yield break;
        }

        EnemyTeamAttack();

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
        PlayerData playerData = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));
        int teamMemberCount = playerData.teamData;
        for (int i = 1; i <= teamMemberCount; i++)
        {
            string dataPath = "character" + i;
           Character character = (Character)PlayerDataManager.instance.Load(dataPath, typeof(Character));
           playerTeam.Add(character);

        }
        // test
        //Character chr = new Character();
        //chr.chrName = "Henry";
        //chr.equipHP = 300;
        //chr.equipAtk = 6;
        //chr.equipDef = 2;
        //playerTeam.Add(chr);
        //chr = new Character();
        //chr.chrName = "Davis";
        //chr.equipHP = 200;
        //chr.equipAtk = 7;
        //chr.equipDef = 1;
        //playerTeam.Add(chr);
        //chr = new Character();
        //chr.chrName = "Woody";
        //chr.equipHP = 350;
        //chr.equipAtk = 4;
        //chr.equipDef = 4;
        //playerTeam.Add(chr);

        playerTeam = playerTeam.OrderBy(val => val.equipHP).ToList();
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
            enemyTeam.Add(mob);
        }
    }

    void PlayerTeamAttack()
    {
        for (int i = 0; i < playerTeam.Count; i++)
        {
            if (enemyTeam.Count <= 0)
                break;

            int dmg = playerTeam[i].equipAtk - enemyTeam[0].equipDef;
            if (dmg <= 0)
                dmg = 1;

            enemyTeam[0].equipHP -= dmg;

            SaveHistory("Enemy takes damage " + dmg.ToString() + " from " + playerTeam[i].chrName + " (remain hp: " + enemyTeam[0].equipHP + ")");

            if (enemyTeam[0].equipHP <= 0)
            {
                SaveHistory("Enemy died");

                enemyTeam.RemoveAt(0);
            }
        }
    }

    void EnemyTeamAttack()
    {
        for (int i = 0; i < enemyTeam.Count; i++)
        {
            if (playerTeam.Count <= 0)
                break;

            int dmg = enemyTeam[i].equipAtk - playerTeam[0].equipDef;
            if (dmg <= 0)
                dmg = 1;

            playerTeam[0].equipHP -= dmg;

            SaveHistory(playerTeam[0].chrName + " takes damage " + dmg.ToString() + " from enemy (remain hp: " + playerTeam[0].equipHP + ")");

            if (playerTeam[0].equipHP <= 0)
            {
                SaveHistory(playerTeam[0].chrName + " died");

                playerTeam.RemoveAt(0);
            }
        }
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
            startBattleTime = long.Parse(PlayerPrefs.GetString(GameSetting.START_BATTLE_TIME_KEY, "0"));

        if (PlayerPrefs.HasKey(GameSetting.ALREADY_UPDATE_TIMES_KEY))
            alreadyUpdateTimes = PlayerPrefs.GetInt(GameSetting.ALREADY_UPDATE_TIMES_KEY, 0);

        if (startBattleTime <= 0)
        {
            if (finishEvt != null)
                finishEvt();
            yield break;
        }

        currentFloor = 1;
        SetPlayerTeamData();
        SetEnemyTeamByFloor(currentFloor);

        long finalTime = GeneralFunctions.GetNotTimestamp();
        long timeDisp = finalTime - startBattleTime;
        int allUpdateTimes = Mathf.FloorToInt((float)timeDisp / (float)GameSetting.BATTLE_ROUND_TIME);

        for (int i = 0; i < allUpdateTimes; i++)
        {
            if (i >= alreadyUpdateTimes)
                writeHistory = true;

            if (playerTeam.Count <= 0)
            {
                // TODO 結算, 更新玩家資料
                if (finishEvt != null)
                    finishEvt();
                yield break;
            }

            yield return StartCoroutine(CalculateBattleUpdate());
        }

        alreadyUpdateTimes = 0;
        PlayerPrefs.SetInt(GameSetting.ALREADY_UPDATE_TIMES_KEY, alreadyUpdateTimes);
        PlayerPrefs.Save();
        if (finishEvt != null)
            finishEvt();
        Invoke("StartBattleUpdate", GameSetting.BATTLE_ROUND_TIME);
    }
}
