using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GlobalDefine;
public class TeamInfoManager : MonoBehaviour {
   public Transform teamParent;
    public GameObject characterItem;
    public GameObject characterIfo;
    // Use this for initialization
    void Start () {
        initTeamData();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void initTeamData()
    {
        PlayerData playerData = (PlayerData)PlayerDataManager.instance.Load("playerdata", typeof(PlayerData));
       int teamMemberCount= playerData.teamData;
        for(int i =1; i <= teamMemberCount; i++)
        {
            string dataPath = "character" +i;
            Character character = (Character)PlayerDataManager.instance.Load(dataPath, typeof(Character));
            GameObject gobj = GameObject.Instantiate(characterItem);

            gobj.name = i.ToString();// id
            gobj.transform.SetParent(teamParent);
            gobj.transform.localScale = teamParent.transform.localScale;
            Button btn = gobj.GetComponentInChildren<Button>();
            btn.onClick.AddListener(delegate ()
            {
                this.OnCharacterInformation(gobj);
            });
            Transform child= btn.transform;
            child.FindChild("lvContent").GetComponent<Text>().text = character.level.ToString();
            child.FindChild("hpContent").GetComponent<Text>().text = character.baseVarHP.ToString() + "/" + character.baseFixHP.ToString();
            child.FindChild("atkContent").GetComponent<Text>().text = character.baseAtk.ToString();
            child.FindChild("defContent").GetComponent<Text>().text = character.baseDef.ToString();
    
        }

    }


    public void OnCharacterInformation(GameObject sender) {
        Debug.Log("sender:" + sender.name);
        string dataPath = "character" + sender.name;
        Character character = (Character)PlayerDataManager.instance.Load(dataPath, typeof(Character));
        characterIfo.SetActive(true);
        characterIfo.transform.FindChild("lvContent").GetComponent<Text>().text = character.level.ToString();
        characterIfo.transform.FindChild("hpContent").GetComponent<Text>().text = character.baseVarHP.ToString() + "/" + character.baseFixHP.ToString();
        characterIfo.transform.FindChild("atkContent").GetComponent<Text>().text = character.baseAtk.ToString();
        characterIfo.transform.FindChild("defContent").GetComponent<Text>().text = character.baseDef.ToString();

    }

    public void Cancel(GameObject obj)
    {
        obj.SetActive(false);
    }

}
