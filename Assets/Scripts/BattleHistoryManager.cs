using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleHistoryManager : MonoBehaviour {

    public static BattleHistoryManager instance;

    public Text updateLeftTimeText;
    public Text updateCountText;
    public Text msgPrefab;
    public Transform contentRoot;

    private int msgMaxLimit;
    private List<Text> textObjList;
    private List<string> msgList;

    void Awake () {
        instance = this;

        msgMaxLimit = 10;

        textObjList = new List<Text>();
        msgList = new List<string>();

        for (int i=0; i<msgMaxLimit; i++)
        {
            Text obj = GameObject.Instantiate(msgPrefab, contentRoot, false);
            textObjList.Add(obj);
        }
    }

    void OnEnable () {
//        if (BattleHandle.instance != null)
//        {
//            BattleHandle.instance.StartCalculateBattleResult(() => {
//                string[] tmpList = BattleHandle.instance.LoadHistory();
//                for (int i=0; i<tmpList.Length; i++)
//                {
//                    AddMsgToList(tmpList[i]);
//                }
//            });
//        }

        if (BattleHandle.instance != null)
            BattleHandle.instance.BattleStart();
    }

    public void AddMsgToList(string msg)
    {
        if (msgList.Count >= msgMaxLimit)
            msgList.RemoveAt(0);

        msgList.Add(msg);

        updateMsgPosition();
    }

    void updateMsgPosition()
    {
        for (int i = 0; i < msgList.Count; i++)
        {            
            textObjList[i].text = msgList[i];
            ((RectTransform)textObjList[i].gameObject.transform).anchoredPosition = new Vector2(0f, 600f - 110f + (-110f * i));
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
