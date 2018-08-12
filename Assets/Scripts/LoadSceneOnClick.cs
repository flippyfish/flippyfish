﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

	public GameObject loadingImage;

	public void LoadScene(string level)
	{
		loadingImage.SetActive(true);
		SceneManager.LoadScene(level);
	}
}
