using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using GlobalDefine;
using System.Xml;
public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

    void OnGUI()
    {
        if (GUI.Button(new Rect(500, 20, 180, 30), "init Item"))
        {
            initItemJson();
        };
    }

	void Awake () {
		instance = this;
	}

	// Use this for initialization
	void Start () {
        if (PlayerDataManager.instance.firstEnterGame())
            initItemJson();
        GlobalDefine.PlayerData playerData = (GlobalDefine.PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(GlobalDefine.PlayerData));
        GeneralUIManager.instance.SetMoneyInfo(playerData.money.ToString());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// TODO
	void PlayerTeamDataInit()
	{
		List<Character> team = PlayerDataManager.instance.teamData;
		team = new List<Character> ();

		Character ch = new Character ();
		ch.SetLevel (20);
		ch.UpdateBaseStatus ();
	}

    private void initItemJson()
    {
        XmlDocument xml = new XmlDocument();
        TextAsset textAsset = (TextAsset)Resources.Load("Items", typeof(TextAsset));
        xml.Load(new System.IO.StringReader(textAsset.text));
 
      
        XmlNodeList xmlNodeList = xml.SelectSingleNode("objects").ChildNodes;
        Debug.Log(xmlNodeList.Count);

        PlayerData playerData = new PlayerData();
        playerData.teamData = 0;
        List<ItemData> itemDataList = new List<ItemData>();
        foreach (XmlElement xl in xmlNodeList)
        {
            ItemData item = new ItemData();
            item.id = Int32.Parse(xl.GetAttribute("id"));
            item.name = xl.GetAttribute("name");
            item.price = Int32.Parse(xl.GetAttribute("price"));
            item.ownCount = 0;
            item.attr= Int32.Parse(xl.GetAttribute("attr"));
            itemDataList.Add(item);
        }
        playerData.ownItemData = itemDataList;
        PlayerDataManager.instance.Save("playerdata", playerData);

        
    
        //
        List<GlobalDefine.CharacterInfo> characterInfoList = new List<GlobalDefine.CharacterInfo>();
        XmlDocument xml2 = new XmlDocument();
        TextAsset textAsset2 = (TextAsset)Resources.Load("CharacterName", typeof(TextAsset));
        xml2.Load(new System.IO.StringReader(textAsset2.text));
       


        xmlNodeList = xml2.SelectSingleNode("objects").ChildNodes;
        foreach (XmlElement x2 in xmlNodeList)
        {
            GlobalDefine.CharacterInfo characterInfo = new GlobalDefine.CharacterInfo();
            characterInfo.id = Int32.Parse(x2.GetAttribute("id"));
            characterInfo.name = x2.GetAttribute("name");
            characterInfoList.Add(characterInfo);
        }
        playerData.characterInfoList = characterInfoList;

     //
        playerData.money = 15000;
        //
        List<ExpData> expDataList = new List<ExpData>();
        XmlDocument xml3 = new XmlDocument();
        TextAsset textAsset3 = (TextAsset)Resources.Load("Exp", typeof(TextAsset));
        xml3.Load(new System.IO.StringReader(textAsset3.text));
     
        xmlNodeList = xml3.SelectSingleNode("objects").ChildNodes;
        foreach (XmlElement x3 in xmlNodeList)
        {
            ExpData expItem = new ExpData();
            expItem.lv = Int32.Parse(x3.GetAttribute("lv"));
            expItem.needExp = Int32.Parse(x3.GetAttribute("needExp"));
            expDataList.Add(expItem);
        }
        playerData.expDataList = expDataList;
        PlayerDataManager.instance.Save("playerdata", playerData);
    }


    [ContextMenu("Initial")]
    void Initial()
    {
        PlayerPrefs.DeleteAll();
    }
}
