using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

	public GameObject loadingImage;
	public GameObject instructions;

	public void LoadScene(string level)
	{
		loadingImage.SetActive(true);
		SceneManager.LoadScene(level);
	}

	public void ShowInstructions()
	{
		instructions.SetActive(true);
	}

	public void HideInstructions()
	{
		instructions.SetActive(false);
	}
}
