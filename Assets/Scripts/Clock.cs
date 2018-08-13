using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Clock : MonoBehaviour {

	public int seconds;
	private int SECONDS_DEFAULT = 120;

	// Use this for initialization
	void Start ()
	{
		Scene scene = SceneManager.GetActiveScene();
		if (scene.name == "Level1")
			seconds = 90;
		else
			seconds = SECONDS_DEFAULT;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
