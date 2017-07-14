using UnityEngine;
using System.Collections;

public class BattleHistoryManager : MonoBehaviour {

    void OnEnable () {
        BattleHandle.instance.StartCalculateBattleResult(()=>{
            BattleHandle.instance.LoadHistory();
        });
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
