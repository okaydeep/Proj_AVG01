using System;
using System.Collections.Generic;

namespace GlobalDefine {
	
	public enum GCanvas {
		LoadingImage = 0,
		HomeMenu = 1,
		BattleMenu = 2,
		TeamInfo = 3,
		NormalStage = 4,
		Market = 5,
		Setting = 6,
		BattleHistory = 7			
	}

	public class GameSetting {
		public const int BATTLE_ROUND_TIME = 10;
	}

	public class PlayerData {
		public int playerLevel;						// 玩家等級
		public int clearStageMaxLevel;				// 最佳通關記錄
		public int money;							// 錢
		public long lastLoginTimestamp;				// 最後一次開啟遊戲時間
		public List<Dictionary<int, int>> itemData;	// 物品資料, 第一個參數是物品id, 第二個是數量
		public List<Character> teamData;			// 隊伍資料

		public void UpdateLastLoginTimestamp()
		{
			lastLoginTimestamp = (long)DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1, 0, 0, 0)).TotalSeconds;
		}

		public int GetAccmSecondsAfterLastLogin()
		{
			return (int)((long)DateTime.UtcNow.Subtract (new DateTime (1970, 1, 1, 0, 0, 0)).TotalSeconds - lastLoginTimestamp);
		}
	}

	public class CharBase {
		protected int level;			// 等級
		protected int baseHP;			// 基本血量
		protected int baseAtk;			// 基本攻擊
		protected int baseDef;			// 基本防禦
		protected int baseHit;		// 基本命中
		protected int baseDod;		// 基本閃避
		protected int baseCrt;		// 基本爆擊
		protected int finalHP;			// 最終血量 (經過技能, 裝備加成)
		protected int finalAtk;		// 最終攻擊
		protected int finalDef;		// 最終防禦
		protected int finalHit;		// 最終命中
		protected int finalDod;		// 最終閃避
		protected int finalCrt;		// 最終爆擊

		public CharBase ()
		{
			level = 1;
			baseHP = level * 50;
			baseAtk = level * 20;
			baseDef = level * 10;
			baseHit = level * 5;
			baseDod = level * 5;
			baseCrt = 5;
		}

	}

	public class Character : CharBase {
		protected int item_atk;				// 武器id
		protected int item_def;				// 防具id
		protected int item_accessories_1;	// 配件1id
		protected int item_accessories_2;	// 配件2id
		protected int currentExp;			// 目前經驗值
		protected int currentMaxExp;		// 目前經驗最大值

		public Character()
		{
			level = 1;
			UpdateBaseStatus ();
		}

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
			baseHP = level * 50;
			baseAtk = level * 20;
			baseDef = level * 10;
			baseHit = level * 5;
			baseDod = level * 5;
			baseCrt = 5;
			item_atk = 0;
			item_def = 0;
			item_accessories_1 = 0;
			item_accessories_2 = 0;
			currentExp = 0;
			currentMaxExp = level * 100;
		}

		public int GetFinalHP()
		{
			int ret = baseHP;
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

	public class Monster : CharBase {
		protected int expReward;						// 擊敗獲得經驗
		protected int moneyReward;						// 擊敗獲得金錢
		protected bool isBoss;							// 是否為boss
		protected List<Dictionary<int, int>> dropData;	// 掉落物品資料, 第一個值為物品id, 第二個值為掉落機率, 數量皆為1

	}

}
