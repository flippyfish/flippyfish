using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 *	Loads a scene, displaying a loading screen in the process.
 */
public class LoadSceneScript : MonoBehaviour {

    public GameObject loadingImage;

	public void LoadScene(string level)
	{
		loadingImage.SetActive(true);
		SceneManager.LoadScene(level);
	}
}
