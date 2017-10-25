using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GlobalDefine;
public class TeamInfoManager : MonoBehaviour
{
    public static TeamInfoManager instance = null;
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
    private Character character;
    private PlayerData playerData;
    private int currentEquipmentType;
    private int[] atkArray = new int[] { 4, 6, 8, 10, 12 };
    private int[] defArray = new int[] { 5, 7, 9, 11, 13 };
    private int[] accessoryArray = new int[] { 14, 15, 16 };


    List<Character> playerTeam;
    private enum EquipmentType
    {
        ATK = 0,
        DEF = 1,
        ACCESSORY1 = 2,
        ACCESSORY2 = 3
    }
    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
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

    public void InitTeamInfo()
    {
        InitTeamData();
        Cancel(characterIfo);
        Cancel(equipmentList);
    }

    public void InitTeamData()
    {
        playerTeam = new List<Character>();
        for (int i = 0; i < teamParent.transform.childCount; i++)
        {
            GameObject go = teamParent.transform.GetChild(i).gameObject;
            Destroy(go);
        }


        playerData = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));
        int teamMemberCount = playerData.teamData;
        Debug.Log("===teamMemberCount===:" + teamMemberCount);
        for (int i = 1; i <= teamMemberCount; i++)
        {
            string dataPath = "character" + i;
            character = (Character)PlayerDataManager.instance.Load(dataPath, typeof(Character));
            playerTeam.Add(character);
            GameObject gobj = GameObject.Instantiate(characterItem);

            gobj.name = i.ToString();// id
            gobj.transform.SetParent(teamParent);
            gobj.transform.localScale = teamParent.transform.localScale;
            Button btn = gobj.GetComponentInChildren<Button>();
            btn.onClick.AddListener(delegate ()
            {
                this.OnCharacterInformation(gobj);
            });
            Transform child = btn.transform;
            updateTeamInfo(child);

        }

    }


    private void updateTeamInfo(Transform child)
    {
        child.Find("name").GetComponent<Text>().text = character.chrName;
        child.Find("lvContent").GetComponent<Text>().text = character.level.ToString();
        child.Find("hpContent").GetComponent<Text>().text = character.baseVarHP.ToString() + "/" + (character.baseFixHP + character.equipHP).ToString();
        child.Find("atkContent").GetComponent<Text>().text = (character.baseAtk + character.equipAtk).ToString();
        child.Find("defContent").GetComponent<Text>().text = (character.baseDef + character.equipDef).ToString();
        updateCharacterIfo();

    }

    private void updateCharacterIfo()
    {
        characterIfo.transform.Find("name").GetComponent<Text>().text = character.chrName;
        characterIfo.transform.Find("lvContent").GetComponent<Text>().text = character.level.ToString();
        characterIfo.transform.Find("hpContent").GetComponent<Text>().text = character.baseVarHP.ToString() + "/" + (character.baseFixHP + character.equipHP).ToString();
        characterIfo.transform.Find("atkContent").GetComponent<Text>().text = (character.baseAtk + character.equipAtk).ToString();
        characterIfo.transform.Find("defContent").GetComponent<Text>().text = (character.baseDef + character.equipDef).ToString();
        characterIfo.transform.Find("expContent").GetComponent<Text>().text = character.currentExp.ToString() + "/" + character.currentMaxExp.ToString();

    }


    public void OnCharacterInformation(GameObject sender)
    {
        Debug.Log("sender:" + sender.name);
        // string dataPath = "character" + sender.name;
        currentCharacter = int.Parse(sender.name);
        // = (Character)PlayerDataManager.instance.Load(dataPath, typeof(Character));
        Debug.Log("currentCharacter:" + currentCharacter);//1-4
        string dataPath = "character" + currentCharacter;

        // playerTeam[0];
        character = playerTeam[currentCharacter - 1];
        // character = (Character)PlayerDataManager.instance.Load(dataPath, typeof(Character));

        characterIfo.SetActive(true);
        characterIfo.transform.Find("name").GetComponent<Text>().text = character.chrName;
        characterIfo.transform.Find("lvContent").GetComponent<Text>().text = character.level.ToString();
        Debug.Log("baseFixHP:" + character.baseFixHP);
        Debug.Log("equipHP:" + character.equipHP);

        characterIfo.transform.Find("hpContent").GetComponent<Text>().text = character.baseVarHP.ToString() + "/" + (character.baseFixHP + character.equipHP).ToString();
        characterIfo.transform.Find("atkContent").GetComponent<Text>().text = (character.baseAtk + character.equipAtk).ToString();
        characterIfo.transform.Find("defContent").GetComponent<Text>().text = (character.baseDef + character.equipDef).ToString();

        List<ItemData> ownItemDataList = playerData.ownItemData;
        ItemData itemData;
        Debug.Log("item_atk:" + character.item_atk);
        Debug.Log("item_def:" + character.item_def);
        Debug.Log("item_accessories_1:" + character.item_accessories_1);
        Debug.Log("item_accessories_2:" + character.item_accessories_2);
        //設定角色已安裝的裝備
        if (character.item_atk == 0)
        {
            equiptmentItems[0].name = "empty";
            equiptmentItems[0].transform.GetComponent<Text>().text = "+";
        }
        else
        {//該物品名稱 換圖

            //非武器return 4 6 8 10
            if (character.item_atk == 4 || character.item_atk == 6 || character.item_atk == 8 || character.item_atk == 10 || character.item_atk == 12)
            {
                itemData = ownItemDataList[character.item_atk - 1];
                equiptmentItems[0].name = itemData.name;
                equiptmentItems[0].transform.GetComponent<Text>().text = itemData.name;
            }

        }
        if (character.item_def == 0)
        {
            equiptmentItems[1].name = "empty";
            equiptmentItems[1].transform.GetComponent<Text>().text = "+";
        }
        else
        {//該物品名稱 換圖     
            if (character.item_def == 5 || character.item_def == 7 || character.item_def == 9 || character.item_def == 11 || character.item_def == 13)
            {
                itemData = ownItemDataList[character.item_def - 1];
                equiptmentItems[1].name = itemData.name;
                equiptmentItems[1].transform.GetComponent<Text>().text = itemData.name;
            }
        }

        if (character.item_accessories_1 == 0)
        {
            equiptmentItems[2].name = "empty";
            equiptmentItems[2].transform.GetComponent<Text>().text = "+";
        }
        else
        {//該物品名稱 換圖
            if (character.item_accessories_1 > 13 && character.item_accessories_1 < 17)
            {
                itemData = ownItemDataList[character.item_accessories_1 - 1];
                equiptmentItems[2].name = itemData.name;
                equiptmentItems[2].transform.GetComponent<Text>().text = itemData.name;
            }
        }
        if (character.item_accessories_2 == 0)
        {
            equiptmentItems[3].name = "empty";
            equiptmentItems[3].transform.GetComponent<Text>().text = "+";
        }
        else
        {//該物品名稱 換圖
            if (character.item_accessories_2 > 13 && character.item_accessories_2 < 17)
            {
                itemData = ownItemDataList[character.item_accessories_2 - 1];
                equiptmentItems[3].name = itemData.name;
                equiptmentItems[3].transform.GetComponent<Text>().text = itemData.name;
            }
        }


    }

    public void Cancel(GameObject obj)
    {
        Debug.Log("obj:" + obj.name);
        switch (obj.name)
        {
            case "Scroll View":
                CanvasManager.instance.ShowCanvas(GlobalDefine.GCanvas.BattleMenu);
                characterIfo.SetActive(false);
                equipmentList.SetActive(false);
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
                currentEquipmentType = (int)EquipmentType.ATK;
                showEquipment(atkArray);
                break;
            case "item_def"://5 7 9 11 13
                Debug.Log("item_def:" + equiptmentItems[1].name);
                currentEquipmentType = (int)EquipmentType.DEF;
                showEquipment(defArray);
                break;
            case "item_accessories_1":// 14 15 16
                Debug.Log("item_accessories_1:" + equiptmentItems[2].name);
                currentEquipmentType = (int)EquipmentType.ACCESSORY1;
                showEquipment(accessoryArray);
                break;
            case "item_accessories_2":// 14 15 16
                Debug.Log("item_accessories_1:" + equiptmentItems[3].name);
                currentEquipmentType = (int)EquipmentType.ACCESSORY2;
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

            if (ownCount == 0 || ownCount <= equipmentCount)// 或是擁有數小於已裝備數量
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
            btn.onClick.AddListener(delegate ()
            {
                this.OnEquipment(gobj, itemData);
            });

            btn.transform.Find("title").GetComponent<Text>().text = itemData.name;
            //依ID顯示加成文字
            string t = "";
            switch (id)
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


            btn.transform.Find("content").GetComponent<Text>().text = t + itemData.attr.ToString();





        }
    }


    public void OnEquipment(GameObject sender, ItemData itemD)
    {
        Debug.Log("id:" + sender.name);
        //詢問要不要裝備
        equipmentDialog.SetActive(true);
        equipId.name = sender.name;
        equipmentDialog.transform.Find("itemName").GetComponent<Text>().text = itemD.name;

    }

    public void ConfirmEquipment()
    {

        string dataPath = "character" + currentCharacter.ToString();
        List<ItemData> ownItemDataList = playerData.ownItemData;
        Debug.Log("id:" + equipId.name);
        //裝備ID
        int id = int.Parse(equipId.name);
        ItemData currentItemData = ownItemDataList[id - 1];
        Debug.Log("equipmentCount:" + currentItemData.equipmentCount);
        //查看現有裝備數 夠的話裝備
        int ownCount = currentItemData.ownCount;
        int equipmentCount = currentItemData.equipmentCount;
        /*
        Debug.Log("currrent_id:" + id);
        Debug.Log("old_id:" + character.item_atk);
         * */
        if (ownCount > equipmentCount)//character.item_atk != id
        {

            Debug.Log("currentEquipmentType:" + currentEquipmentType);


            switch (currentEquipmentType)
            {
                case (int)EquipmentType.ATK:

                    if (character.item_atk == id)
                        return;
                    if (character.item_atk != 0)//已經有裝備了
                    {
                        ItemData oldItemData = ownItemDataList[character.item_atk - 1];
                        Debug.Log("oldItemData:" + oldItemData.name);
                        oldItemData.equipmentCount -= 1;
                        ownItemDataList[character.item_atk - 1] = oldItemData;
                        //減掉原本裝備能力值
                        character.equipAtk = 0;

                    }

                    equiptmentItems[0].name = currentItemData.name;
                    equiptmentItems[0].transform.GetComponent<Text>().text = currentItemData.name;
                    character.item_atk = id;
                    character.equipAtk = currentItemData.attr;
                    break;
                case (int)EquipmentType.DEF:
                    if (character.item_def == id)
                        return;
                    if (character.item_def != 0)//已經有裝備了
                    {
                        ItemData oldItemData = ownItemDataList[character.item_def - 1];
                        Debug.Log("oldItemData:" + oldItemData.name);
                        oldItemData.equipmentCount -= 1;
                        ownItemDataList[character.item_def - 1] = oldItemData;
                        //減掉原本裝備能力值
                        character.equipDef = 0;
                    }
                    equiptmentItems[1].name = currentItemData.name;
                    equiptmentItems[1].transform.GetComponent<Text>().text = currentItemData.name;
                    character.item_def = id;
                    character.equipDef = currentItemData.attr;
                    break;
                case (int)EquipmentType.ACCESSORY1:
                    if (character.item_accessories_1 == id)
                        return;
                    if (character.item_accessories_1 != 0)
                    {
                        ItemData oldItemData = ownItemDataList[character.item_accessories_1 - 1];
                        Debug.Log("oldItemData:" + oldItemData.name);
                        oldItemData.equipmentCount -= 1;
                        ownItemDataList[character.item_accessories_1 - 1] = oldItemData;
                        //減掉原本裝備能力值
                        character.equipHP = 0;
                    }
                    equiptmentItems[2].name = currentItemData.name;
                    equiptmentItems[2].transform.GetComponent<Text>().text = currentItemData.name;
                    character.item_accessories_1 = id;
                    character.equipHP = currentItemData.attr;
                    break;
                case (int)EquipmentType.ACCESSORY2:
                    if (character.item_accessories_2 == id)
                        return;
                    if (character.item_accessories_2 != 0)
                    {
                        ItemData oldItemData = ownItemDataList[character.item_accessories_2 - 1];
                        Debug.Log("oldItemData:" + oldItemData.name);
                        oldItemData.equipmentCount -= 1;
                        ownItemDataList[character.item_accessories_2 - 1] = oldItemData;
                        character.equipHP = 0;
                    }
                    equiptmentItems[3].name = currentItemData.name;
                    equiptmentItems[3].transform.GetComponent<Text>().text = currentItemData.name;
                    character.item_accessories_2 = id;
                    character.equipHP = currentItemData.attr;
                    break;

            }
            //遊戲紀錄該物品 已裝備數量加1
            currentItemData.equipmentCount += 1;
            ownItemDataList[id - 1] = currentItemData;



            // Debug.Log("Count:" + ownItemDataList.Count);
            Debug.Log("item_atk:" + character.item_atk);
            playerData.ownItemData = ownItemDataList;
            PlayerDataManager.instance.Save("playerdata", playerData);
            PlayerDataManager.instance.Save(dataPath, character);
        }
        Debug.Log("currentCharacter:" + currentCharacter.ToString());

        Transform curCharacter = teamParent.GetChild(currentCharacter - 1).GetComponentInChildren<Button>().transform;

        //刷新UI
        updateTeamInfo(curCharacter);
        equipmentDialog.SetActive(false);
        Cancel(equipmentList);
    }


    //

    [ContextMenu("addExp")]
    void addExp()
    {
        ObtainExp(300);
    }
    public void ObtainExp(int totalExp)
    {
        int characterCount = playerTeam.Count;
        //獲得的總經驗 均分給所有傭兵
        int exp = totalExp / characterCount;

        //
        for (int i = 0; i < characterCount; i++)
        {
            Character character = playerTeam[i];
            //查詢目前等級
            int curLevel = character.level;
            //查詢目前經驗
            int curExp = character.currentExp;
            //查詢升級所需總經驗
            int curMaxExp = character.currentMaxExp;
            //升級所需經驗
            int needExp = curMaxExp - curExp;
            string dataPath = "character" +( i + 1);

            checkExp(dataPath, character, exp, needExp);
        }

    }

    private void checkExp(string dataPath, Character character, int exp, int needExp)
    {
        if (exp > needExp)//經驗超過所需經驗
        {
            int curLevel = character.level;
            curLevel += 1;
            //升級加一次能力值 HP15-20 atk2-5 def
            character.baseFixHP += UnityEngine.Random.Range(15, 21);
            character.baseAtk += UnityEngine.Random.Range(2, 6);
            character.baseDef += UnityEngine.Random.Range(2, 6);
    
            int lastExp = exp - needExp;//剩下的經驗
            Debug.Log("curLevel:" + curLevel);
            //找尋下一級所需經驗
            int lv = playerData.expDataList[curLevel - 1].lv;
            Debug.Log("lv:" + lv);
            needExp = playerData.expDataList[curLevel - 1].needExp;
            Debug.Log("needExp:" + needExp);
            character.level = curLevel;

            checkExp(dataPath, character, lastExp, needExp);
        }
        else
        {//經驗 小於 所需經驗 直接設定
            character.currentExp = exp;
            character.currentMaxExp = needExp;
            updateCharacterIfo();
            characterIfo.transform.Find("expContent").GetComponent<Text>().text = exp.ToString() + "/" + needExp.ToString();
            //存檔
             PlayerDataManager.instance.Save(dataPath, character);
            return;
        }
    }




}
