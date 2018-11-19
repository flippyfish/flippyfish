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

    public bool isDirectory()
    {
        return Directory.Exists(Application.persistentDataPath + "/saved_game");
    }

    public void save(string sceneName)
    {
        if (!isDirectory()) {
            Directory.CreateDirectory(Application.persistentDataPath + "/saved_game");
        }
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/saved_game/saved_game.save", false);
        writer.WriteLine(sceneName);
        writer.Close();
    }
}
