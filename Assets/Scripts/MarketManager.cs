using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using GlobalDefine;
public class MarketManager : MonoBehaviour
{
    public GameObject[] MarketObjs;
    public GameObject[] MarketBtns;
    public Transform parent;
    public GameObject ShopList;
    public GameObject PurchaseDialog;
    public GameObject SellDialog;
    public GameObject item;
    public GameObject ShopStoreObj;
    public GameObject TavenObj;
    public GameObject BackObj;
    public GameObject charcterDialog;
    public GameObject hireDialog;
    public static MarketManager instance = null;

    private int currentPage;
    private int ownTeamMember = 0;
    private int ownMoney = 0;
    private String hirePrice = "500";

    public enum Market_Page
    {

        Main = 0,//商店、傭兵選擇頁
        ShopStore = 1,//商店
        Taven = 2,//傭兵

    }



    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 20, 180, 30), "add 1000 Money"))
        {
            addMoney(1000);
            //tet();

        };
        /*
        if (GUI.Button(new Rect(300, 20, 180, 30), "xml"))
        {
            LoadXML();
        };*/

        //if (GUI.Button(new Rect(500, 20, 180, 30), "init Item"))
        //{
        //    initItemJson();
        //};
    }
    // Use this for initialization
    void Start()
    {
        //if (PlayerDataManager.instance.firstEnterGame())
        //    initItemJson();


    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }

    public void Purchase()
    {

        for (int i = 0; i < MarketObjs.Length; i++)
            MarketBtns[i].SetActive(false);
        BackObj.SetActive(false);
        ShopList.SetActive(true);
        GeneralUIManager.instance.ShowMoneyInfo(true);
        PlayerData playerData = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));

        ownMoney = playerData.money;

        GeneralUIManager.instance.SetMoneyInfo(ownMoney.ToString());

        List<ItemData> ownItemDataList = playerData.ownItemData;
        int itemCount = ownItemDataList.Count;
        for (int i = 1; i <= itemCount; i++)
        {
            GameObject gobj = GameObject.Instantiate(item);

            gobj.name = i.ToString();// id
            gobj.transform.SetParent(parent);
            gobj.transform.localScale = parent.transform.localScale;
            
          //  gobj.GetComponentInChildren<Text>().text = ownItemDataList[i - 1].name;
            gobj.GetComponentInChildren<Button>().transform.FindChild("title").GetComponent<Text>().text = ownItemDataList[i - 1].name;
          //依ID顯示加成文字
            string t="";
            switch (i)
            {
                case 1://補HP
                case 2:
                case 3:
                    t = "回復HP";
                    break;
                case 4://武
                case 6:
                case 8:
                case 10:
                case 12:
                    t = "ATK+";
                    break;
                case 5://防
                case 7:
                case 9:
                case 11:
                case 13:
                    t = "DEF+";
                    break;
                case 14://HP
                case 15:
                case 16:
                    t = "HP+";
                    break;
            }

            
            gobj.GetComponentInChildren<Button>().transform.FindChild("content").GetComponent<Text>().text =t+ ownItemDataList[i - 1].attr.ToString();


            Button btn = gobj.GetComponentInChildren<Button>();
            btn.onClick.AddListener(delegate()
            {
                this.OnBuyInformation(gobj);
            });
        }
    }

    public void Sell()
    {
        for (int i = 0; i < MarketObjs.Length; i++)
            MarketBtns[i].SetActive(false);
        BackObj.SetActive(false);
        ShopList.SetActive(true);

        GeneralUIManager.instance.ShowMoneyInfo(true);

        PlayerData playerData = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));
        ownMoney = playerData.money;

        GeneralUIManager.instance.SetMoneyInfo(ownMoney.ToString());


        List<ItemData> ownItemDataList = playerData.ownItemData;
        int itemCount = ownItemDataList.Count;

        for (int i = 1; i <= itemCount; i++)
        {
            int ownCount = ownItemDataList[i - 1].ownCount;
            int equipmentCount = ownItemDataList[i - 1].equipmentCount;
            Debug.Log("i:" + i + "  ownCount:" + ownCount);
            if (ownCount == 0 || ownCount <= equipmentCount)
                continue;

            GameObject gobj = GameObject.Instantiate(item);
            gobj.name = i.ToString();// id
            gobj.transform.SetParent(parent);
            gobj.transform.localScale = parent.transform.localScale;


            string name = ownItemDataList[i - 1].name;
            Debug.Log("name:" + name);
            gobj.GetComponentInChildren<Text>().text = name;


            Button btn = gobj.GetComponentInChildren<Button>();
            btn.onClick.AddListener(delegate()
            {
                this.OnSellInformation(gobj);
            });



        }

    }

    public void OnBuyInformation(GameObject sender)
    {
        Debug.Log("Test:" + sender.name);

        PurchaseDialog.SetActive(true);
        int id = int.Parse(sender.name);

        PlayerData playerData = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));
        List<ItemData> ownItemDataList = playerData.ownItemData;
        PurchaseDialog.transform.FindChild("title").GetComponent<Text>().text = ownItemDataList[id - 1].name;
        PurchaseDialog.transform.FindChild("price").GetComponent<Text>().text = ownItemDataList[id - 1].price.ToString();
        PurchaseDialog.transform.FindChild("ID").GetChild(0).gameObject.name = sender.name;
        PurchaseDialog.transform.FindChild("holdNum").GetComponent<Text>().text = ownItemDataList[id - 1].ownCount.ToString();
    }

    public void OnSellInformation(GameObject sender)
    {
        Debug.Log("Test:" + sender.name);
        SellDialog.SetActive(true);

        PlayerData playerData = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));
        List<ItemData> ownItemDataList = playerData.ownItemData;
        int id = int.Parse(sender.name);
        ItemData itemData = ownItemDataList[id - 1];
        int price = itemData.price / 2;

        SellDialog.transform.FindChild("title").GetComponent<Text>().text = itemData.name;
        SellDialog.transform.FindChild("price").GetComponent<Text>().text = price.ToString();
        SellDialog.transform.FindChild("ID").GetChild(0).gameObject.name = sender.name;
        //顯示未裝備的數量
        int remainCount = itemData.ownCount - itemData.equipmentCount;
        SellDialog.transform.FindChild("holdNum").GetComponent<Text>().text = remainCount.ToString();
    }

    public void Buy(int num)
    {
        //價格
        int price = int.Parse(PurchaseDialog.transform.FindChild("price").GetComponent<Text>().text);
        int id = int.Parse(PurchaseDialog.transform.FindChild("ID").GetChild(0).gameObject.name);
        Debug.Log("price:" + price);

        PlayerData playerData = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));
        List<ItemData> ownItemDataList = playerData.ownItemData;

        //目前數量+num
        int count = ownItemDataList[id - 1].ownCount;

        bool moneyEnough = reduceMoney(price * num);
        if (!moneyEnough)
            return;
        Text hold = PurchaseDialog.transform.FindChild("holdNum").GetComponent<Text>();
        count += num;
        hold.text = count.ToString();
        ownItemDataList[id - 1].ownCount = count;
        PlayerDataManager.instance.Save("playerdata", playerData);
    }

    public void Sell(int num)
    {
        //價格
        int price = int.Parse(SellDialog.transform.FindChild("price").GetComponent<Text>().text);
        Debug.Log("price:" + price);
        //目前數量+num
        int id = int.Parse(SellDialog.transform.FindChild("ID").GetChild(0).gameObject.name);
        PlayerData playerData = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));
        List<ItemData> ownItemDataList = playerData.ownItemData;
        int count = ownItemDataList[id - 1].ownCount;

        if (num > count)
        {
            Debug.Log("沒那多物品:" + count);
            return;
        }
        else if (num == count)
        {
            Destroy(parent.transform.Find(id.ToString()).gameObject);
        }

        Text hold = SellDialog.transform.FindChild("holdNum").GetComponent<Text>();
        count -= num;
        Debug.Log("剩餘數量:" + count);
        hold.text = count.ToString();
        ownItemDataList[id - 1].ownCount = count;
        PlayerDataManager.instance.Save("playerdata", playerData);
        addMoney(price * num);
    }

    public void CloseDialog(GameObject obj)
    {
        obj.SetActive(false);
        Debug.Log(obj.name);

        switch (obj.name)
        {
            case "Scroll View":
                for (int i = 0; i < MarketBtns.Length; i++)
                    MarketBtns[i].SetActive(true);
                BackObj.SetActive(true);

                GeneralUIManager.instance.ShowMoneyInfo(false);
                //清除ScrollView裡的item
                for (int i = 0; i < parent.transform.childCount; i++)
                {
                    GameObject go = parent.transform.GetChild(i).gameObject;
                    Destroy(go);
                }
                break;
            case "CharacterDialog":
                Cancel();
                break;

        }


        /*
        if (obj.name == "Scroll View")
        {
            Debug.Log("1");
            for (int i = 0; i < MarketBtns.Length; i++)
                MarketBtns[i].SetActive(true);
            BackObj.SetActive(true);

            GeneralUIManager.instance.ShowMoneyInfo(false);
            //清除ScrollView裡的item
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                GameObject go = parent.transform.GetChild(i).gameObject;
                Destroy(go);
            }

        }
        else
            Debug.Log("2");*/
    }


    public void Back()
    {
        // 
        Debug.Log("Back:" + currentPage);
        switch (currentPage)
        {
            case (int)Market_Page.Main:
                CanvasManager.instance.ShowCanvas(GlobalDefine.GCanvas.BattleMenu);

                break;
            case (int)Market_Page.ShopStore:
                // CanvasManager.instance.ShowCanvas(GlobalDefine.GCanvas.Market);
                for (int i = 0; i < MarketObjs.Length; i++)
                    MarketObjs[i].SetActive(true);
                ShopStoreObj.SetActive(false);

                currentPage = (int)Market_Page.Main;
                break;
            case (int)Market_Page.Taven:
                currentPage = (int)Market_Page.Main;
                break;

            default:

                break;
        }

    }


    public void ShopStore()
    {
        currentPage = (int)Market_Page.ShopStore;
        for (int i = 0; i < MarketObjs.Length; i++)
            MarketObjs[i].SetActive(false);
        ShopStoreObj.SetActive(true);
    }

    public void Taven()
    {
        hireDialog.SetActive(true);
        PlayerData pd = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));

        ownTeamMember = pd.teamData;

        switch (ownTeamMember)
        {
            case 0:
                hirePrice = "500";
                break;
            case 1:
                hirePrice = "1500";
                break;
            case 2:
                hirePrice = "4500";
                break;
            case 3:
                hirePrice = "10000";
                break;
        }

        hireDialog.transform.FindChild("hirePrice").GetComponent<Text>().text = hirePrice;
        currentPage = (int)Market_Page.Taven;


        for (int i = 0; i < MarketObjs.Length; i++)
            MarketObjs[i].SetActive(false);
        BackObj.SetActive(false);
        TavenObj.SetActive(true);
    }


    public void setPage(int page)
    {
        currentPage = page;
    }

    public void Hire()
    {

        bool moneyEnough = reduceMoney(int.Parse(hirePrice));
        if (!moneyEnough)
            return;
        /*
        UnityEngine.Random.InitState(System.Guid.NewGuid().GetHashCode());
        int HP = UnityEngine.Random.Range(200, 241);
        int ATK = UnityEngine.Random.Range(10, 16);
        int DEF = UnityEngine.Random.Range(10, 16);
        Debug.Log("HP:" + HP);
        Debug.Log("ATK:" + ATK);
        Debug.Log("DEF:" + DEF);
        */
        PlayerData pd = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));

        int ownCount = ownTeamMember + 1;
        Debug.Log("ownCount:" + ownCount);
        if (ownCount > 4)
        {
            Debug.Log("傭兵滿了");
            return;
        }
        string dataPath = "character" + ownCount.ToString();
        Character character = new Character();

        UnityEngine.Random.InitState(System.Guid.NewGuid().GetHashCode());
        character.level = 1;
        character.baseFixHP = UnityEngine.Random.Range(200, 241);
        character.baseVarHP = character.baseFixHP;
        character.baseAtk = UnityEngine.Random.Range(10, 16);
        character.baseDef = UnityEngine.Random.Range(10, 16);


        pd.teamData = ownCount;
        PlayerDataManager.instance.Save(dataPath, character);
        PlayerDataManager.instance.Save("playerdata", pd);

        charcterDialog.SetActive(true);
        showHireDialog(character.baseFixHP, character.baseAtk, character.baseDef);
        Debug.Log("===InitTeamData===");
        TeamInfoManager.instance.InitTeamData();
        Debug.Log("===InitTeamData===");
    }

    public void Cancel()
    {
        for (int i = 0; i < MarketObjs.Length; i++)
            MarketObjs[i].SetActive(true);
        BackObj.SetActive(true);
        TavenObj.SetActive(false);
    }


    private void addMoney(int price)
    {
        GlobalDefine.PlayerData playerData = (GlobalDefine.PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(GlobalDefine.PlayerData));
        ownMoney = playerData.money;
        ownMoney += price;
        playerData.money = ownMoney;
        GeneralUIManager.instance.SetMoneyInfo(ownMoney.ToString());
        PlayerDataManager.instance.Save("playerdata", playerData);
    }

    private bool reduceMoney(int price)
    {
        Debug.Log("reduceMoney:" + price);
        PlayerData playerData = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));

        if (playerData != null)
        {
            ownMoney = playerData.money;
        }


        if (price > ownMoney)
        {
            Debug.Log("餘額不足");
            return false;
        }

        ownMoney -= price;


        GeneralUIManager.instance.SetMoneyInfo(ownMoney.ToString());
        playerData.money = ownMoney;
        PlayerDataManager.instance.Save("playerdata", playerData);
        return true;
    }
    //初始化PlayerData
    //private void initItemJson()
    //{
    //    XmlDocument xml = new XmlDocument();
    //    XmlReaderSettings set = new XmlReaderSettings();
    //    set.IgnoreComments = true;
    //    xml.Load(XmlReader.Create((Application.dataPath + "/Items.xml"), set));

    //    XmlNodeList xmlNodeList = xml.SelectSingleNode("objects").ChildNodes;
    //    Debug.Log(xmlNodeList.Count);

    //    PlayerData playerData = new PlayerData();
    //    playerData.teamData = 0;
    //    List<ItemData> itemDataList = new List<ItemData>();
    //    foreach (XmlElement xl in xmlNodeList)
    //    {
    //        ItemData item = new ItemData();
    //        item.id = Int32.Parse(xl.GetAttribute("id"));
    //        item.name = xl.GetAttribute("name");
    //        item.price = Int32.Parse(xl.GetAttribute("price"));
    //        item.ownCount = 0;
    //        itemDataList.Add(item);
    //    }
    //    playerData.ownItemData = itemDataList;
    //    PlayerDataManager.instance.Save("playerdata", playerData);
    //}

    private void showHireDialog(int hp, int atk, int def)
    {
        hireDialog.SetActive(false);
        charcterDialog.transform.FindChild("name").GetComponent<Text>().text = "測試員";
        charcterDialog.transform.FindChild("hpContent").GetComponent<Text>().text = hp.ToString();
        charcterDialog.transform.FindChild("atkContent").GetComponent<Text>().text = atk.ToString();
        charcterDialog.transform.FindChild("defContent").GetComponent<Text>().text = def.ToString();
    }

    /*
    void CreateXML()
    {
        string path = Application.dataPath + "/Items.xml";
        if (!File.Exists(path))
        {
            XmlDocument xml = new XmlDocument();
            XmlElement root = xml.CreateElement("objects");

            XmlElement main = xml.CreateElement("items");
            main.SetAttribute("id", "1");
            main.SetAttribute("name", "初級藥水");
            main.SetAttribute("price", "100");
            root.AppendChild(main);

            XmlElement main2 = xml.CreateElement("items");
            main2.SetAttribute("id", "2");
            main2.SetAttribute("name", "中級藥水");
            main2.SetAttribute("price", "300");
            root.AppendChild(main2);

            xml.AppendChild(root);
            xml.Save(path);
        }
    }

    void LoadXML()
    {
        XmlDocument xml = new XmlDocument();
        XmlReaderSettings set = new XmlReaderSettings();
        set.IgnoreComments = true;
        xml.Load(XmlReader.Create((Application.dataPath + "/Items.xml"), set));

        XmlNodeList xmlNodeList = xml.SelectSingleNode("objects").ChildNodes;
        foreach (XmlElement xl1 in xmlNodeList)
        {

            Debug.Log("id:" + xl1.GetAttribute("id"));
            Debug.Log("name:" + xl1.GetAttribute("name"));
            Debug.Log("price:" + xl1.GetAttribute("price"));
        }

    }
    */

}
