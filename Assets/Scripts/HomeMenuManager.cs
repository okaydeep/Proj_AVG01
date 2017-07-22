using UnityEngine;

using System.Xml;
using System;
using System.Collections.Generic;
using GlobalDefine;
using System.IO;

public class HomeMenuManager : MonoBehaviour
{
    public GameObject lastPlay;
    public GameObject newStart;
    public GameObject option;

    public GameObject reStartDialog;
    GlobalDefine.PlayerData playerData;
    // Use this for initialization

    void OnGUI()
    {
        if (GUI.Button(new Rect(500, 20, 180, 30), "init Item"))
        {
            initItemJson();
        };
    }

    void Start()
    {
        if (PlayerDataManager.instance.firstEnterGame())
        {
            initItemJson();
            lastPlay.SetActive(false);
        }
        else
        {

        }

        playerData = (GlobalDefine.PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(GlobalDefine.PlayerData));

        //  Debug.Log("HomeMenuManager playerData:"+ playerData.money.ToString());
        GeneralUIManager.instance.SetMoneyInfo(playerData.money.ToString());
        

    }

    // Update is called once per frame
    void Update()
    {


    }

    public void Resume()
    {
        CanvasManager.instance.ShowCanvas(GlobalDefine.GCanvas.BattleMenu);
    }

    public void NewTravel()
    {//如果已有資料 詢問是否覆蓋
        if (!PlayerDataManager.instance.firstEnterGame())
        {
            reStartDialog.SetActive(true);
        }
        else
            CanvasManager.instance.ShowCanvas(GlobalDefine.GCanvas.BattleMenu);
    }

    public void TurnToSetting()
    {
        CanvasManager.instance.ShowCanvas(GlobalDefine.GCanvas.Setting);
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
            item.attr = Int32.Parse(xl.GetAttribute("attr"));
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


    public void Confirm()
    {
        //覆蓋檔案
        int teamData = playerData.teamData;
        initItemJson();
        reStartDialog.SetActive(false);
        CanvasManager.instance.ShowCanvas(GlobalDefine.GCanvas.BattleMenu);

        String dataPath;
        for(int i=1;i<= teamData; i++)
        {
            dataPath = "character" + i;
            PlayerDataManager.instance.DeleteFile(dataPath);
        }

    }

    public void Cancel()
    {
        reStartDialog.SetActive(false);
    }

}
