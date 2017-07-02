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
        XmlReaderSettings set = new XmlReaderSettings();
        set.IgnoreComments = true;
        xml.Load(XmlReader.Create((Application.dataPath + "/Items.xml"), set));

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
            itemDataList.Add(item);
        }
        playerData.ownItemData = itemDataList;
        PlayerDataManager.instance.Save("playerdata", playerData);
    }
}
