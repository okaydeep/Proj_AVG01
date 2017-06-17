using UnityEngine;
using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using GlobalDefine;
public class PlayerDataManager
{

    private static PlayerDataManager _instance;


    public static PlayerDataManager instance
    {
        get
        {
            if (_instance == null)
                _instance = new PlayerDataManager();
            return _instance;
        }
    }

    public List<Character> teamData;

    public Boolean firstEnterGame() {   
        if(PlayerPrefs.GetInt("first", 0)==0)
        {
            PlayerPrefs.SetInt("first", 1);
            return true;
        }
        return false;
    }

    public string SerializeObject(object obj)
    {
        string serializedString = string.Empty;
        serializedString = JsonUtility.ToJson(obj);
      //  Debug.Log("serializedString:" + serializedString);
        return serializedString;
    }

    public object DeserializeObject(string str,Type type)
    {
        object deserializeObject = null;
        deserializeObject = JsonUtility.FromJson(str,type);
       // Debug.Log("deserializeObject:" + deserializeObject);
        return deserializeObject;
    }

    public void CreateFile(string filePath,string content)
    {
        filePath = DataPath() + filePath;
        Debug.Log("filePath:" + filePath);
        StreamWriter streamWriter = File.CreateText(filePath);
        Debug.Log("content:" + content);
        streamWriter.Write(content);
        streamWriter.Close();
    }

    private string DataPath()
    {
        return Application.persistentDataPath + "/";
    }

    public void Save(string filePath, object obj) {
        string savePath = DataPath() + "/" + filePath;
        string saveString = SerializeObject(obj);
        Debug.Log("saveString:" + saveString);
        CreateFile(filePath, saveString);
    }

    public object Load(string filePath,Type type)
    {
        string loadPath = DataPath() + "/" + filePath;
        StreamReader streamReader = File.OpenText(loadPath);
        string data = streamReader.ReadToEnd();
        streamReader.Close();
        return DeserializeObject(data, type);
    }
 

}
