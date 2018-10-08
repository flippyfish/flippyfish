using System.Collections;
using System.Collections.Generic;
using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour {
    /*
    public static LoadGame instance;

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

    public void Load()
    {
        if (!isDirectory())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saved_game");
        }
        // BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/saved_game/saved_game.save")) {
            //FileStream file = File.Open(Application.persistentDataPath + "/saved_game/saved_game.save", FileMode.Open);
            //JsonUtility.FromJsonOverwrite((string) bf.Deserialize(file), );
            //file.Close();
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/saved_game/saved_game.save");
            string sceneName = reader.ReadLine();
            reader.Close();
            SceneManager.LoadScene(sceneName);
            Debug.Log("Loading...");
        } 
        Debug.Log("Failed to load saved game");
    }
}
