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

	public class PlayerData {
		public int playerLevel;
		public int passMaxLevel;
		public int money;
		public long lastLoginTimestamp;

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
		public int level;
		public int baseHP;
		public int baseAtk;
		public int baseDef;
		public float baseHitChance;
		public float baseDodChance;
		public float baseCrtChance;
		public int finalHP;
		public int finalAtk;
		public int finalDef;
		public float finalHitChance;
		public float finalDodChance;
		public float finalCrtChance;


	}

	public class Character : CharBase {
		public int item_atk;
		public int item_def;
		public int item_accessories_1;
		public int item_accessories_2;
	}

	public class Monster : CharBase {

	}

}
