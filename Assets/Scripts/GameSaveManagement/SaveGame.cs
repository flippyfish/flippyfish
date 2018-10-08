using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveGame : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        save(sceneName);
        Debug.Log("game saved at: " + Application.persistentDataPath + "/saved_game/saved_game.save");
    }

    /*
    public static SaveGame instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
    }
    */
    public bool isDirectory()
    {
        return Directory.Exists(Application.persistentDataPath + "/saved_game");
    }

    public void save(string sceneName)
    {
        if (!isDirectory()) {
            Directory.CreateDirectory(Application.persistentDataPath + "/saved_game");
        }
        // These are for the case of saving object, We only need to save String as a file
        //BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Create(Application.persistentDataPath + "/saved_game/saved_game.save");
        //var json = JsonUtility.ToJson(sceneName);
        //bf.Serialize(file, json);
        //file.Close();
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/saved_game/saved_game.save", false);
        writer.WriteLine(sceneName);
        writer.Close();
    }
}
