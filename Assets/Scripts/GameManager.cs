using UnityEngine;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;



	void Awake () {
		instance = this;
	}

	// Use this for initialization
	void Start () {
        Invoke("LoadAD", 5.0f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void LoadAD()
    {
        AdApi.instance.RequestBanner();
    }


    [ContextMenu("Initial")]
    void Initial()
    {
        PlayerPrefs.DeleteAll();
    }
}
