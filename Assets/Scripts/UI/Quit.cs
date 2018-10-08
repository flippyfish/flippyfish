using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour {

    public void quitGame()
    {
        Debug.Log("Quit the game");
        Application.Quit();
    }
}
