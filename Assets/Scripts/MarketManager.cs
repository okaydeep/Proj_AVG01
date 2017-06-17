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
    private Dictionary<string, Dictionary<string, string>> itemDic = new Dictionary<string, Dictionary<string, string>>();
    public static MarketManager instance = null;
    public enum Market_Page
    {

        Main = 0,//商店、傭兵選擇頁
        ShopStore = 1,//商店
        Taven = 2,//傭兵

    }

    private int currentPage;

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 20, 180, 30), "add 1000 Money"))
        {
            addMoney(1000);
            //tet();

        };

        if (GUI.Button(new Rect(300, 20, 180, 30), "xml"))
        {
            LoadXML();
        };

        if (GUI.Button(new Rect(500, 20, 180, 30), "init Item"))
        {
            initItemJson();
        };
    }
    // Use this for initialization
    void Start()
    {
        if (PlayerDataManager.instance.firstEnterGame())
            initItemJson();
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
    //初始化PlayerData
    private void initItemJson()
    {
        XmlDocument xml = new XmlDocument();
        XmlReaderSettings set = new XmlReaderSettings();
        set.IgnoreComments = true;
        xml.Load(XmlReader.Create((Application.dataPath + "/Items.xml"), set));

        XmlNodeList xmlNodeList = xml.SelectSingleNode("objects").ChildNodes;
        Debug.Log(xmlNodeList.Count);

        string dataPath;

        PlayerData playerData = new PlayerData();
        playerData.teamData = new List<Character>();
        List<ItemData> itemDataList = new List<ItemData>();
        foreach (XmlElement xl in xmlNodeList)
        {
            dataPath = "item" + xl.GetAttribute("id");
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

    public void Purchase()
    {

        for (int i = 0; i < MarketObjs.Length; i++)
            MarketBtns[i].SetActive(false);
        BackObj.SetActive(false);
        ShopList.SetActive(true);
        GeneralUIManager.instance.ShowMoneyInfo(true);
        PlayerData playerData = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));
        int ownMoney = 0;

        ownMoney = playerData.money;

        GeneralUIManager.instance.SetMoneyInfo(ownMoney.ToString());

        List<ItemData> ownItemDataList = playerData.ownItemData;
        int itemCount = ownItemDataList.Count;
        for (int i = 1; i <= itemCount; i++)
        {
            string dataPath = "item" + i;

            GameObject gobj = GameObject.Instantiate(item);

            gobj.name = i.ToString();// id
            gobj.transform.SetParent(parent);
            gobj.transform.localScale = parent.transform.localScale;
            gobj.GetComponentInChildren<Text>().text = ownItemDataList[i - 1].name;


            Button btn = gobj.GetComponentInChildren<Button>();
            btn.onClick.AddListener(delegate ()
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
        int ownMoney = 0;
        ownMoney = playerData.money;

        GeneralUIManager.instance.SetMoneyInfo(ownMoney.ToString());


        List<ItemData> ownItemDataList = playerData.ownItemData;
        int itemCount = ownItemDataList.Count;

        for (int i = 1; i <= itemCount; i++)
        {
            string dataPath = "item" + i;
            int ownCount = ownItemDataList[i - 1].ownCount;
            Debug.Log("i:" + i + "  ownCount:" + ownCount);
            if (ownCount == 0)
                continue;

            GameObject gobj = GameObject.Instantiate(item);

            gobj.name = i.ToString();// id
            gobj.transform.SetParent(parent);
            gobj.transform.localScale = parent.transform.localScale;


            string name = ownItemDataList[i - 1].name;
            Debug.Log("name:" + name);
            gobj.GetComponentInChildren<Text>().text = name;


            Button btn = gobj.GetComponentInChildren<Button>();
            btn.onClick.AddListener(delegate ()
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
        string dataPath = "item" + sender.name;

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
        int price = ownItemDataList[id - 1].price / 2;

        SellDialog.transform.FindChild("title").GetComponent<Text>().text = ownItemDataList[id - 1].name;
        SellDialog.transform.FindChild("price").GetComponent<Text>().text = price.ToString();
        SellDialog.transform.FindChild("ID").GetChild(0).gameObject.name = sender.name;
        SellDialog.transform.FindChild("holdNum").GetComponent<Text>().text = ownItemDataList[id - 1].ownCount.ToString();
    }


    public void Buy(int num)
    {
        //品項
        string title = PurchaseDialog.transform.FindChild("title").GetComponent<Text>().text;
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
        Debug.Log("after count:" + count);
        hold.text = count.ToString();
        ownItemDataList[id - 1].ownCount = count;
        PlayerDataManager.instance.Save("playerdata", playerData);
    }


    public void Sell(int num)
    {
        //品項
        string title = SellDialog.transform.FindChild("title").GetComponent<Text>().text;
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
            Debug.Log("2");
    }


    private void addMoney(int price)
    {
        GlobalDefine.PlayerData playerData = (GlobalDefine.PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(GlobalDefine.PlayerData));
        int ownMoney = 0;

        ownMoney = playerData.money;



        ownMoney += price;

        playerData.money = ownMoney;


        GeneralUIManager.instance.SetMoneyInfo(ownMoney.ToString());


        PlayerDataManager.instance.Save("playerdata", playerData);
    }

    private bool reduceMoney(int price)
    {
        Debug.Log("reduceMoney:" + price);
        GlobalDefine.PlayerData playerData = (GlobalDefine.PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(GlobalDefine.PlayerData));
        int ownMoney = 0;
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
        PlayerDataManager.instance.Save("playerdata", playerData);
        return true;
    }

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

    //TAVEN

    public void Hire()
    {

        UnityEngine.Random.InitState(System.Guid.NewGuid().GetHashCode());
        int HP = UnityEngine.Random.Range(200, 241);
        int ATK = UnityEngine.Random.Range(10, 16);
        int DEF = UnityEngine.Random.Range(10, 16);
        Debug.Log("HP:" + HP);
        Debug.Log("ATK:" + ATK);
        Debug.Log("DEF:" + DEF);

        PlayerData pd = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));
        Debug.Log("DEF:" + DEF);
        int ownCount = pd.teamData.Count + 1;

        if (ownCount > 4)
        {
            Debug.Log("傭兵滿了");
            return;
        }
        string dataPath = "character" + ownCount.ToString();
        Character character = new Character();
        pd.teamData.Add(character);
        PlayerDataManager.instance.Save(dataPath, character);
        hireDialog.SetActive(false);
        charcterDialog.SetActive(true);
    }

    public void Cancel()
    {
        for (int i = 0; i < MarketObjs.Length; i++)
            MarketObjs[i].SetActive(true);
        BackObj.SetActive(true);
        TavenObj.SetActive(false);
    }


}
