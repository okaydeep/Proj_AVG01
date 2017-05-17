using UnityEngine;
using System.Collections;

public class PlayerDataManager {

	private static PlayerDataManager _instance;
	public static PlayerDataManager instance {
		get
		{
			if (_instance == null)
				_instance = new PlayerDataManager();
			return _instance;
		}
	}

	public void GetData()
	{
		
	}
}
