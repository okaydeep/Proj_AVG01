using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GlobalDefine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;

	void Awake () {
		instance = this;
	}

	// Use this for initialization
	void Start () {
		
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
}
