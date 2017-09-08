using System;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalDefine
{

    public enum GCanvas
    {
        LoadingImage = 0,
        HomeMenu = 1,
        BattleMenu = 2,
        TeamInfo = 3,
        NormalStage = 4,
        Market = 5,
        Setting = 6,
        BattleHistory = 7
    }

    public class GameSetting
    {
		public const float BATTLE_ROUND_TIME = 10f;		// 戰鬥一回合所需花費的時間(秒)

		public const int NORMALSTAGE_MAX_FLOOR = 5000;	// 一般關卡最高樓層

        public static string BATTLE_HISTORY_FILEPATH = Application.persistentDataPath + "\\" + "history.txt";

        public static string START_BATTLE_TIME_KEY = "START_BATTLE_TIME_KEY";
        public static string ALREADY_UPDATE_TIMES_KEY = "ALREADY_UPDATE_TIMES_KEY";
    }

    public class GeneralFunctions
    {
        public static long GetNowTimestamp()
        {
            return (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }
    }

    [Serializable]
    public class PlayerData
    {
        [SerializeField]
        public int playerLevel;                     // 玩家等級
        [SerializeField]
        public int clearStageMaxLevel;              // 最佳通關記錄
        [SerializeField]
        public int money;                           // 錢
        [SerializeField]
        public List<ItemData> ownItemData; // 物品資料, 第一個參數是物品id, 第二個是數量
                                           // public List<Dictionary<int, int>> itemData;
        [SerializeField]
        public List<ExpData> expDataList;

        [SerializeField]
        public int teamData;

        [SerializeField]
        public List<int> usedId;

        [SerializeField]
        public List<CharacterInfo> characterInfoList;
        // public List<Character> teamData;            // 隊伍資料
    }

    [Serializable]
    public class CharacterInfo
    {
        public int id;
        public string name;
    }

    [Serializable]
    public class ExpData
    {
        public int lv;
        public int needExp;
    }


    [Serializable]
    public class ItemData
    {
        public int id;
        public string name;
        public int price;
        public int ownCount;
        public int equipmentCount;
        public int attr;
    }
    [Serializable]
    public class CharBase
    {
        [SerializeField]
        public int level;            // 等級
        [SerializeField]
        public int baseFixHP;           // 基本血量
        [SerializeField]
        public int baseVarHP;           // 基本變動血量
        [SerializeField]
        public int baseAtk;          // 基本攻擊
        [SerializeField]
        public int baseDef;          // 基本防禦  
        [SerializeField]
        public int equipHP;          // 裝備血量 (經過技能, 裝備加成)
        [SerializeField]
        public int equipAtk;     // 裝備攻擊
        [SerializeField]
        public int equipDef;     // 裝備防禦

        public int currentHP;
        public int currentAtk;
        public int currentDef;

        /*
        public CharBase()
        {
            level = 1;
            baseFixHP = level * 50;
            baseAtk = level * 20;
            baseDef = level * 10;
        }*/

    }
    [Serializable]
    public class Character : CharBase
    {
        [SerializeField]
        public string chrName;             // 名稱
        [SerializeField]
        public int item_atk;             // 武器id
        [SerializeField]
        public int item_def;             // 防具id
        [SerializeField]
        public int item_accessories_1;   // 配件1id
        [SerializeField]
        public int item_accessories_2;   // 配件2id
        [SerializeField]
        public int currentExp;           // 目前經驗值
        [SerializeField]
        public int currentMaxExp;        // 目前經驗最大值

      /*  public Character()
        {
            UnityEngine.Random.InitState(System.Guid.NewGuid().GetHashCode());
            level = 1;
            baseFixHP = UnityEngine.Random.Range(200, 241);
            baseVarHP = baseFixHP;
            baseAtk = UnityEngine.Random.Range(10, 16);
            baseDef = UnityEngine.Random.Range(10, 16);
            item_atk = 0;
            item_def = 0;
            item_accessories_1 = 0;
            item_accessories_2 = 0;
            currentExp = 0;
            currentMaxExp = level * 100;

            //UpdateBaseStatus();
        }*/

        public int GetLevel()
        {
            return level;
        }

        public void SetLevel(int l)
        {
            level = l;
        }

        // TODO
        public void UpdateBaseStatus()
        {
            baseFixHP = level * 50;
            baseAtk = level * 20;
            baseDef = level * 10;
       
            item_atk = 0;
            item_def = 0;
            item_accessories_1 = 0;
            item_accessories_2 = 0;
            currentExp = 0;
            currentMaxExp = level * 100;
        }

        public int GetFinalHP()
        {
            int ret = baseFixHP;
            return ret;
        }

        public int GetFinalAtk()
        {
            int ret = baseAtk;
            return ret;
        }

        public int GetFinalDef()
        {
            int ret = baseDef;
            return ret;
        }
        /*
        public int GetFinalHit()
        {
            int ret = baseHit;
            return ret;
        }

        public int GetFinalDod()
        {
            int ret = baseDod;
            return ret;
        }

        public int GetFinalCrt()
        {
            int ret = baseCrt;
            return ret;
        }
        */
        public int GetCurrentExp()
        {
            return currentExp;
        }

        public int GetCurrentMaxExp()
        {
            int ret = level * 100;
            return ret;
        }

    }


    public class Monster : CharBase
    {
        protected int expReward;                        // 擊敗獲得經驗
        protected int moneyReward;                      // 擊敗獲得金錢
        protected bool isBoss;                          // 是否為boss
        protected List<Dictionary<int, int>> dropData;  // 掉落物品資料, 第一個值為物品id, 第二個值為掉落機率, 數量皆為1

    }

}
