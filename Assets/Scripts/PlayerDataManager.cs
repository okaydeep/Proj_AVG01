using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PlayerDataManager {

	private static PlayerDataManager _instance;
    Dictionary<string, int> ownItemDic = new Dictionary<string, int>();
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

    public void initOwnItems()
    {
        //刪除 PlayerPrefs 中某一個key的值
        //  PlayerPrefs.DeleteKey(“key”);

        //判斷 PlayerPrefs中是否存在這個key
        // bool b = PlayerPrefs.HasKey(“key”);

      //  PlayerPrefs.GetString("testStr", "default");
       // PlayerPrefs.SetString("testStr", "default");
    }
    /*
    public int GetItemData(string item)
    {
      return  PlayerPrefs.GetInt(item, 0);
   
    }
    public void SetItemData(string item,int count)
    {
        PlayerPrefs.SetInt(item, count);
        PlayerPrefs.Save();
    }
    */

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
        PlayerPrefs.SetInt("1", 0);
        PlayerPrefs.SetInt("2", 0);
        PlayerPrefs.SetInt("3", 0);
        PlayerPrefs.SetInt("4", 0);
        PlayerPrefs.SetInt("5", 0);
        PlayerPrefs.SetInt("6", 0);
        PlayerPrefs.SetInt("7", 0);
        PlayerPrefs.SetInt("8", 0);
        PlayerPrefs.SetInt("9", 0);
        PlayerPrefs.SetInt("10", 0);
        PlayerPrefs.SetInt("11", 0);
        PlayerPrefs.SetInt("12", 0);
        PlayerPrefs.SetInt("13", 0);
        PlayerPrefs.SetInt("14", 0);
        PlayerPrefs.SetInt("15", 0);
        PlayerPrefs.SetInt("16", 0);
        PlayerPrefs.Save();
    }

    public Dictionary<string, int> GetAllItemCount()
    {
  
       
      

        ownItemDic.Add("1", PlayerPrefs.GetInt("1", 0));

        ownItemDic.Add("2", PlayerPrefs.GetInt("2", 0));
    
        ownItemDic.Add("3", PlayerPrefs.GetInt("3", 0));
      
        ownItemDic.Add("4", PlayerPrefs.GetInt("4", 0));
        
        ownItemDic.Add("5", PlayerPrefs.GetInt("5", 0));
      
        ownItemDic.Add("6", PlayerPrefs.GetInt("6", 0));
      
        ownItemDic.Add("7", PlayerPrefs.GetInt("7", 0));
     
        ownItemDic.Add("8", PlayerPrefs.GetInt("8", 0));
      
        ownItemDic.Add("9", PlayerPrefs.GetInt("9", 0));
  
        ownItemDic.Add("10", PlayerPrefs.GetInt("10", 0));
     
        ownItemDic.Add("11", PlayerPrefs.GetInt("11", 0));
  
        ownItemDic.Add("12", PlayerPrefs.GetInt("12", 0));
    
        ownItemDic.Add("13", PlayerPrefs.GetInt("13", 0));
     
        ownItemDic.Add("14", PlayerPrefs.GetInt("14", 0));
      
        ownItemDic.Add("15", PlayerPrefs.GetInt("15", 0));
     
        ownItemDic.Add("16", PlayerPrefs.GetInt("16", 0));
 
        return ownItemDic;
    }
}
