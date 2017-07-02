using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GlobalDefine;
public class TeamInfoManager : MonoBehaviour
{
    enum EquipmentType{
        EMPTY=0,
        ATK=1,
        DEF=2,
        ACCESSORY=3
    }



    public Transform teamParent;
    public Transform parent;
    public GameObject characterItem;
    public GameObject characterIfo;
    public GameObject equipmentList;
    public GameObject equipmentDialog;
    public GameObject[] equiptmentItems;

    public GameObject item;
    public GameObject equipId;
    private int currentCharacter;
    Character character;
    PlayerData playerData;
    int currentEquipmentType;
 

    int[] atkArray = new int[] { 4, 6, 8, 10, 12 };
    int[] defArray = new int[] { 5, 7, 9, 11, 13 };
    int[] accessoryArray = new int[] { 14, 15, 16 };
    // Use this for initialization
    void Start()
    {
        initTeamData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void initTeamData()
    {
        playerData = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));
        int teamMemberCount = playerData.teamData;
        for (int i = 1; i <= teamMemberCount; i++)
        {
            string dataPath = "character" + i;
            character = (Character)PlayerDataManager.instance.Load(dataPath, typeof(Character));
            GameObject gobj = GameObject.Instantiate(characterItem);

            gobj.name = i.ToString();// id
            gobj.transform.SetParent(teamParent);
            gobj.transform.localScale = teamParent.transform.localScale;
            Button btn = gobj.GetComponentInChildren<Button>();
            btn.onClick.AddListener(delegate()
            {
                this.OnCharacterInformation(gobj);
            });
            Transform child = btn.transform;
            child.FindChild("lvContent").GetComponent<Text>().text = character.level.ToString();
            child.FindChild("hpContent").GetComponent<Text>().text = character.baseVarHP.ToString() + "/" + character.baseFixHP.ToString();
            child.FindChild("atkContent").GetComponent<Text>().text = character.baseAtk.ToString();
            child.FindChild("defContent").GetComponent<Text>().text = character.baseDef.ToString();

        }

    }

    //去傭兵所雇用傭兵時刷新
    public void UpdateTeamData()
    {

    }


    public void OnCharacterInformation(GameObject sender)
    {
        Debug.Log("sender:" + sender.name);
       // string dataPath = "character" + sender.name;
        currentCharacter = int.Parse(sender.name);
        // = (Character)PlayerDataManager.instance.Load(dataPath, typeof(Character));
        characterIfo.SetActive(true);
        characterIfo.transform.FindChild("lvContent").GetComponent<Text>().text = character.level.ToString();
        characterIfo.transform.FindChild("hpContent").GetComponent<Text>().text = character.baseVarHP.ToString() + "/" + character.baseFixHP.ToString();
        characterIfo.transform.FindChild("atkContent").GetComponent<Text>().text = character.baseAtk.ToString();
        characterIfo.transform.FindChild("defContent").GetComponent<Text>().text = character.baseDef.ToString();
        //設定角色已安裝的裝備
        if (character.item_atk == 0)
        {
            equiptmentItems[0].name = "empty";
        }
        else
        {//該物品名稱 換圖
            //equiptmentItems[0].name
        }
        if (character.item_def == 0)
        {
            equiptmentItems[1].name = "empty";
        }
        else
        {//該物品名稱 換圖
            //equiptmentItems[0].name
        }

        if (character.item_accessories_1 == 0)
        {
            equiptmentItems[2].name = "empty";
        }
        else
        {//該物品名稱 換圖
            //equiptmentItems[0].name
        }
        if (character.item_accessories_2 == 0)
        {
            equiptmentItems[3].name = "empty";
        }
        else
        {//該物品名稱 換圖
            //equiptmentItems[0].name
        }


    }

    public void Cancel(GameObject obj)
    {
        switch (obj.name)
        {
            case "Scroll View":
                CanvasManager.instance.ShowCanvas(GlobalDefine.GCanvas.BattleMenu);
        
                break;

            case "equipmentList":
                obj.SetActive(false);
                  for (int i = 0; i < parent.transform.childCount; i++)
                {
                    GameObject go = parent.transform.GetChild(i).gameObject;
                    Destroy(go);
                }
                break;
            default:
                obj.SetActive(false);
                break;
        }
    }

    public void SetEquipment(GameObject sender)
    {

        Debug.Log("sender:" + sender.name);
        switch (sender.name)
        {
            case "item_atk":
                //顯示裝備列 4 6 8 10 12
                Debug.Log("item_atk:" + equiptmentItems[0].name);
                showEquipment(atkArray);
                break;
            case "item_def"://5 7 9 11 13
                Debug.Log("item_def:" + equiptmentItems[1].name);
                showEquipment(defArray);
                break;
            case "item_accessories_1":// 14 15 16
                Debug.Log("item_accessories_1:" + equiptmentItems[2].name);
                showEquipment(accessoryArray);
                break;
            case "item_accessories_2":// 14 15 16
                Debug.Log("item_accessories_1:" + equiptmentItems[3].name);
                showEquipment(accessoryArray);
                break;
        }

        /*
        Debug.Log("item_atk:" + character.item_atk);
        Debug.Log("item_def:" + character.item_def);
        Debug.Log("item_accessories_1:" + character.item_accessories_1);
        Debug.Log("item_accessories_2:" + character.item_accessories_2);*/
    }

    private void showEquipment(int[] array)
    {
        equipmentList.SetActive(true);
        playerData = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));
        List<ItemData> ownItemDataList = playerData.ownItemData;
        for (int i = 0; i < array.Length; i++)
        {
            int id = array[i];
            // Debug.Log("id:" + id);


            ItemData itemData = ownItemDataList[id - 1];
            int ownCount = itemData.ownCount;
            int equipmentCount = itemData.equipmentCount;

            if (ownCount == 0 || ownCount < equipmentCount)// 或是擁有數小於已裝備數量
                continue;
         
            string name = itemData.name;
        
            Debug.Log("id:" + id + "  name:" + name + " ownCount:" + ownCount + " equipmentCount:" + equipmentCount);

     
            GameObject gobj = GameObject.Instantiate(item);
            gobj.name = id.ToString();// id
            gobj.transform.SetParent(parent);
            gobj.transform.localScale = parent.transform.localScale;
            gobj.GetComponentInChildren<Text>().text = itemData.name;

            //目前裝備的裝備也會顯示 
            Button btn = gobj.GetComponentInChildren<Button>();
            btn.onClick.AddListener(delegate()
            {
                this.OnEquipment(gobj, itemData);
            });
        }
    }


    public void OnEquipment(GameObject sender,ItemData itemD)
    {
        Debug.Log("id:" + sender.name);
        //詢問要不要裝備
        equipmentDialog.SetActive(true);
        equipId.name = sender.name;
        equipmentDialog.transform.FindChild("itemName").GetComponent<Text>().text = itemD.name;

    }

    public void ConfirmEquipment()
    {
        List<ItemData> ownItemDataList = playerData.ownItemData;
        Debug.Log("id:" + equipId.name);
        //裝備ID
        int id = int.Parse(equipId.name);
       ItemData itemData= ownItemDataList[id - 1];
        Debug.Log("equipmentCount:" +itemData.equipmentCount);
        //查看現有裝備數 夠的話裝備
        int ownCount=itemData.ownCount;
        int equipmentCount=itemData.equipmentCount;

        if (ownCount >equipmentCount)
        {
            //此角色該欄位設定物品ID
            character.item_atk = id;
            
            string dataPath = "character" + currentCharacter.ToString();
            PlayerDataManager.instance.Save(dataPath, character);
            //遊戲紀錄該物品 已裝備數量加1
            itemData.equipmentCount+=1;
            ownItemDataList[id - 1]=itemData;
            playerData.ownItemData = ownItemDataList;
            PlayerDataManager.instance.Save("playerdata", playerData);
        }
    }

    

}
