using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour {


    public void ShowTotalJump()
    {
        StreamReader reader = new StreamReader(Application.persistentDataPath + "/user/user");
        string username = reader.ReadLine();
        reader.Close();
        GameObject.Find("Jumps").SetActive(false);
        string jumpNum = GameObject.Find("Fish_Player").GetComponent<FishMovement>().jumps.ToString();
        GameObject.Find("WinMSG").GetComponent<UnityEngine.UI.Text>().text = "Well done " + username + "! You jumped " + jumpNum + " times!";
    }

    /*
     * Save the score in the list to show scores in scoreboard.
     */ 
    public void saveScore() {
        if (!isUserDirectory()){ Directory.CreateDirectory(Application.persistentDataPath + "/user"); }
        if (!isScoreDirectory()) { Directory.CreateDirectory(Application.persistentDataPath + "/score"); }
        string sceneName = SceneManager.GetActiveScene().name;
        int jumps = GameObject.Find("Fish_Player").GetComponent<FishMovement>().jumps;
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/user/user") && File.Exists(Application.persistentDataPath + "/score/" + sceneName + ".score"))
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/user/user");
            string username = reader.ReadLine();
            reader.Close();
            reader = new StreamReader(Application.persistentDataPath + "/score/" + sceneName + ".score");
            string json = (string) bf.Deserialize(reader.BaseStream);
            reader.Close();
            Players players = JsonUtility.FromJson<Players>(json);
            players.players.Add(new Player(username, sceneName, jumps));
            players.players.Sort();
            if (players.players.Count > 30) {
                for (int i  = players.players.Count; i > 30; i--) {
                    players.players.RemoveAt(i - 1);
                }
            }
            json = JsonUtility.ToJson(players);
            StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/score/" + sceneName + ".score", false);
            bf.Serialize(writer.BaseStream, json);
            writer.Close();
        } else {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/user/user");
            string username = reader.ReadLine();
            reader.Close();
            Players players = new Players();
            players.players.Add(new Player(username, sceneName, jumps));
            string json = JsonUtility.ToJson(players);
            StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/score/" + sceneName + ".score", false);
            bf.Serialize(writer.BaseStream, json);
            writer.Close();
        }
        Debug.Log("Failed to load score/user");
    }

    public void loadScore() {
        string sceneName = SceneManager.GetActiveScene().name;
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/user/user") && File.Exists(Application.persistentDataPath + "/score/" + sceneName + ".score"))
        {
            StreamReader reader = new StreamReader(Application.persistentDataPath + "/user/user");
            string username = reader.ReadLine();
            reader.Close();
            reader = new StreamReader(Application.persistentDataPath + "/score/" + sceneName + ".score");
            string json = (string)bf.Deserialize(reader.BaseStream);
            reader.Close();
            Players players = JsonUtility.FromJson<Players>(json);
            int count = 1;
            string boardMsg1 = "";
            string boardMsg2 = "";
            string boardMsg3 = "";
            foreach (Player p in players.players) {
                if (count > 30) { break; }
                if (count < 11)
                {
                    boardMsg1 = boardMsg1 + count + ". " + p.playerName + " - " + p.jumps + " jumps" + System.Environment.NewLine;
                }
                else if (count > 10 && count < 21)
                {
                    boardMsg2 = boardMsg2 + count + ". " + p.playerName + " - " + p.jumps + " jumps" + System.Environment.NewLine;
                }
                else {
                    boardMsg3 = boardMsg3 + count + ". " + p.playerName + " - " + p.jumps + " jumps" + System.Environment.NewLine;
                }
                count++;
            }
            GameObject.Find("ScoreBoard1").GetComponent<UnityEngine.UI.Text>().text = boardMsg1;
            GameObject.Find("ScoreBoard2").GetComponent<UnityEngine.UI.Text>().text = boardMsg2;
            GameObject.Find("ScoreBoard3").GetComponent<UnityEngine.UI.Text>().text = boardMsg3;
        }
        Debug.Log("Failed to load score/user");
    }

    public bool isScoreDirectory()
    {
        return Directory.Exists(Application.persistentDataPath + "/score");
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
