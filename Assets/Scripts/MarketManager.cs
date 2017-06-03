using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
public class MarketManager : MonoBehaviour
{
    public GameObject[] MarketObjs;
    public Transform parent;
    public GameObject ShopList;
    public GameObject PurchaseDialog;
    public GameObject SellDialog;
    public GameObject item;

    private Dictionary<string, Dictionary<string, string>> itemDic = new Dictionary<string, Dictionary<string, string>>();

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 20, 180, 30), "add 1000 Money"))
        {
            addMoney(1000);

        };

        if (GUI.Button(new Rect(300, 20, 180, 30), "xml"))
        {
            LoadXML();
        };

        if (GUI.Button(new Rect(500, 20, 180, 30), "init Item"))
        {
            PlayerDataManager.instance.AllItemCountInit();
        };
    }
    // Use this for initialization
    void Start()
    {
        initItemDic();
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void initItemDic()
    {
        XmlDocument xml = new XmlDocument();
        XmlReaderSettings set = new XmlReaderSettings();
        set.IgnoreComments = true;
        xml.Load(XmlReader.Create((Application.dataPath + "/Items.xml"), set));

        XmlNodeList xmlNodeList = xml.SelectSingleNode("objects").ChildNodes;
        Debug.Log(xmlNodeList.Count);

        foreach (XmlElement xl in xmlNodeList)
        {
            /*
            Debug.Log("id:" + xl.GetAttribute("id"));
            Debug.Log("name:" + xl.GetAttribute("name"));
            Debug.Log("price:" + xl.GetAttribute("price"));
            */
            string id = xl.GetAttribute("id");
            string name = xl.GetAttribute("name");
            string price = xl.GetAttribute("price");
            Dictionary<string, string> tempDic = new Dictionary<string, string>();
            tempDic.Add("name", name);
            tempDic.Add("price", price);
            itemDic.Add(id, tempDic);
        }
    }

    public void Purchase()
    {

        for (int i = 0; i < MarketObjs.Length; i++)
            MarketObjs[i].SetActive(false);

        ShopList.SetActive(true);
        GeneralUIManager.instance.ShowMoneyInfo(true);

        int ownMoney = PlayerPrefs.GetInt("money", 0);

        GeneralUIManager.instance.SetMoneyInfo(ownMoney.ToString());

        for (int i = 1; i <= itemDic.Count; i++)
        {
            GameObject gobj = GameObject.Instantiate(item);
            gobj.name = i.ToString();// id
            gobj.transform.SetParent(parent);
            gobj.transform.localScale = parent.transform.localScale;
            gobj.GetComponentInChildren<Text>().text = itemDic[i.ToString()]["name"];
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
            MarketObjs[i].SetActive(false);

        ShopList.SetActive(true);

        GeneralUIManager.instance.ShowMoneyInfo(true);
        int ownMoney = PlayerPrefs.GetInt("money", 0);

        GeneralUIManager.instance.SetMoneyInfo(ownMoney.ToString());
        //
        Dictionary<string, int> ownItemDic = PlayerDataManager.instance.GetAllItemCount();
        Debug.Log("ownItemDic:" + ownItemDic.Count);

        for (int i = 1; i <= ownItemDic.Count; i++)
        {
            Debug.Log("KEY:" + i.ToString());
            int ownCount = ownItemDic[i.ToString()];

            Debug.Log("i:" + i + "  ownCount:" + ownCount);
            if (ownCount == 0)
                continue;
            GameObject gobj = GameObject.Instantiate(item);

            gobj.name = i.ToString();// id
            gobj.transform.SetParent(parent);
            gobj.transform.localScale = parent.transform.localScale;


            string name = itemDic[i.ToString()]["name"];
            Debug.Log("name:" + name);
            gobj.GetComponentInChildren<Text>().text = name;

            //  ownItemDic[i].Keys name
            Button btn = gobj.GetComponentInChildren<Button>();
            btn.onClick.AddListener(delegate ()
            {
                this.OnSellInformation(gobj);
            });
        }


        /*
        foreach (KeyValuePair<string, int> item in ownItemDic)
        {
            GameObject gobj = GameObject.Instantiate(item);
          
            gobj.name = xl.GetAttribute("id");// id
            gobj.transform.SetParent(parent);
            gobj.transform.localScale = parent.transform.localScale;
            gobj.GetComponentInChildren<Text>().text = xl.GetAttribute("name"); ;
            Button btn = gobj.GetComponentInChildren<Button>();
            btn.onClick.AddListener(delegate ()
            {
                this.OnEmailInformation(gobj);
            });
        }*/

    }

    public void OnBuyInformation(GameObject sender)
    {
        Debug.Log("Test:" + sender.name);

        PurchaseDialog.SetActive(true);
        Dictionary<string, string> tempDic = itemDic[sender.name];

        PurchaseDialog.transform.FindChild("title").GetComponent<Text>().text = tempDic["name"];
        PurchaseDialog.transform.FindChild("price").GetComponent<Text>().text = tempDic["price"];

        PurchaseDialog.transform.FindChild("ID").GetChild(0).gameObject.name = sender.name;
        //Debug.Log("@@:" + tempDic["name"]);
        int count = PlayerDataManager.instance.GetItemCount(sender.name);
        PurchaseDialog.transform.FindChild("holdNum").GetComponent<Text>().text = count.ToString();
    }

    public void OnSellInformation(GameObject sender)
    {
        Debug.Log("Test:" + sender.name);

        SellDialog.SetActive(true);
        Dictionary<string, string> tempDic = itemDic[sender.name];

        SellDialog.transform.FindChild("title").GetComponent<Text>().text = tempDic["name"];
        int price = int.Parse(tempDic["price"]) / 2;

        SellDialog.transform.FindChild("price").GetComponent<Text>().text = price.ToString();

        SellDialog.transform.FindChild("ID").GetChild(0).gameObject.name = sender.name;
        //Debug.Log("@@:" + tempDic["name"]);
        int count = PlayerDataManager.instance.GetItemCount(sender.name);
        SellDialog.transform.FindChild("holdNum").GetComponent<Text>().text = count.ToString();
    }


    public void Buy(int num)
    {
        //品項
        string title = PurchaseDialog.transform.FindChild("title").GetComponent<Text>().text;
        // string title = PurchaseDialog.transform.FindChild("title").GetComponent<Text>().text;
        //價格
        int price = int.Parse(PurchaseDialog.transform.FindChild("price").GetComponent<Text>().text);

        Debug.Log("price:" + price);
        //目前數量+num
        string id = PurchaseDialog.transform.FindChild("ID").GetChild(0).gameObject.name;
        int count = PlayerDataManager.instance.GetItemCount(id);



        bool moneyEnough = reduceMoney(price * num);
        if (!moneyEnough)
            return;
        Text hold = PurchaseDialog.transform.FindChild("holdNum").GetComponent<Text>();
        count += num;
        hold.text = count.ToString();
        PlayerDataManager.instance.SetItemCount(id, count);

    }


    public void Sell(int num)
    {
        //品項
        string title = SellDialog.transform.FindChild("title").GetComponent<Text>().text;
        // string title = PurchaseDialog.transform.FindChild("title").GetComponent<Text>().text;
        //價格
        int price = int.Parse(SellDialog.transform.FindChild("price").GetComponent<Text>().text);

        Debug.Log("price:" + price);
        //目前數量+num
        string id = SellDialog.transform.FindChild("ID").GetChild(0).gameObject.name;
        int count = PlayerDataManager.instance.GetItemCount(id);


        ////
        if (num > count)
        {
            Debug.Log("沒那多物品:" + count);
            return;
        }

        Text hold = SellDialog.transform.FindChild("holdNum").GetComponent<Text>();
        count -= num;
        Debug.Log("剩餘數量:" + count);
        hold.text = count.ToString();
        PlayerDataManager.instance.SetItemCount(id, count);

        addMoney(price * num);
    }



    public void CloseDialog(GameObject obj)
    {
        obj.SetActive(false);
        Debug.Log(obj.name);
        if (obj.name == "Scroll View")
        {
            Debug.Log("1");
            for (int i = 0; i < MarketObjs.Length; i++)
                MarketObjs[i].SetActive(true);

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
        int ownMoney = PlayerPrefs.GetInt("money", 0);
        ownMoney += price;

        PlayerPrefs.SetInt("money", ownMoney);
        PlayerPrefs.Save();

        GeneralUIManager.instance.SetMoneyInfo(ownMoney.ToString());
    }

    private bool reduceMoney(int price)
    {
        Debug.Log("reduceMoney:" + price);
        int ownMoney = PlayerPrefs.GetInt("money", 0);

        if (price > ownMoney)
        {
            Debug.Log("餘額不足");
            return false;
        }

        ownMoney -= price;

        PlayerPrefs.SetInt("money", ownMoney);
        PlayerPrefs.Save();

        GeneralUIManager.instance.SetMoneyInfo(ownMoney.ToString());
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
        CanvasManager.instance.ShowCanvas(GlobalDefine.GCanvas.BattleMenu);
    }


}
