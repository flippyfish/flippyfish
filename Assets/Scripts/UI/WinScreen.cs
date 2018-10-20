using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour {


    public void ShowTotalJump()
    {
        GameObject.Find("Jumps").SetActive(false);
        string jumpNum = GameObject.Find("Fish_Player").GetComponent<FishMovement>().jumps.ToString();
        GameObject.Find("WinMSG").GetComponent<UnityEngine.UI.Text>().text = "Well done! You jumped " + jumpNum + " times!";
    }

    public void saveScore() {
        if (!isUserDirectory()){ Directory.CreateDirectory(Application.persistentDataPath + "/user"); }
        if (!isScoreDirectory()) { Directory.CreateDirectory(Application.persistentDataPath + "/user"); }
        string sceneName = SceneManager.GetActiveScene().name;
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/user/user") && File.Exists(Application.persistentDataPath + "/score/" + sceneName + ".score"))
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/user/user");
            string username = reader.ReadLine();
            reader.Close();


            FileStream file = File.Open(Application.persistentDataPath + "/saved_game/saved_game.save", FileMode.Open);
            //JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), );
            file.Close();
        } else {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/user/user");
            string username = reader.ReadLine();
            reader.Close();
            FileStream file = File.Create(Application.persistentDataPath + "/score/" + sceneName + ".score");
            var json = JsonUtility.ToJson(sceneName);
            bf.Serialize(file, json);
            file.Close();
        }
        Debug.Log("Failed to load score/user");

    }

    public bool isScoreDirectory()
    {
        return Directory.Exists(Application.persistentDataPath + "/Score");
    }

    public bool isUserDirectory()
    {
        return Directory.Exists(Application.persistentDataPath + "/user");
    }

    // Use this for initialization
    void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}
}
