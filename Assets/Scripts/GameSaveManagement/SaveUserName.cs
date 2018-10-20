using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveUserName : MonoBehaviour {
    /*
     * Save current user's name so it can be used for updating scores.
     * 
     * It is stored in C:\Users\"UserName"\AppData\LocalLow\Flippy Fish Group
     * 
     */
    public void SaveName() {
        string name = GameObject.Find("UserName").GetComponent<UnityEngine.UI.Text>().text;
        if (!isDirectory())
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/user");
        }
        StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/user/user", false);
        writer.WriteLine(name);
        writer.Close();
    }

    public bool isDirectory()
    {
        return Directory.Exists(Application.persistentDataPath + "/user");
    }
}
