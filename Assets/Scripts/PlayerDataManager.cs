using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GlobalDefine;

public class PlayerDataManager
{
    private static PlayerDataManager _instance;    
    public static PlayerDataManager instance
    {
        get
        {
            if (_instance == null)
                _instance = new PlayerDataManager();
            return _instance;
        }
    }

	public List<Character> teamData;
	Dictionary<string, int> ownItemDic = new Dictionary<string, int>();

    public void GetData()
    {

    } 

    public int GetItemCount(string item)
    {
        return PlayerPrefs.GetInt(item, 0);

    }
    public void SetItemCount(string item, int count)
    {
        PlayerPrefs.SetInt(item, count);
        ownItemDic[item] = count;
        PlayerPrefs.Save();
    }


    public void AllItemCountInit()
    {
        for (int i = 1; i <= 16; i++)
        {
            PlayerPrefs.SetInt(i.ToString(), 0);
            ownItemDic.Add(i.ToString(), 0);
        }
    }

    public Dictionary<string, int> GetAllItemCount()
    {
        ownItemDic.Clear();
        for (int i = 1; i <= 16; i++)
            ownItemDic.Add(i.ToString(), PlayerPrefs.GetInt(i.ToString(), 0));

        return ownItemDic;
    }
}
