using UnityEngine;

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


    [ContextMenu("Initial")]
    void Initial()
    {
        PlayerPrefs.DeleteAll();
    }
}
