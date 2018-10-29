using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Attach this script to an object in the Main Menu scene. Requires an Audio Source component.
 *
 * https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager-sceneLoaded.html
 * https://answers.unity.com/questions/11314/audio-or-music-to-continue-playing-between-scene-c.html
 */
public class BGM : MonoBehaviour
{

	private static BGM instance = null;		// maintain ONE instance of the script/parent object

	public AudioClip bgmMainMenu;
	public AudioClip bgmFactory;
	public AudioClip bgmCity;
	public AudioClip bgmForest;
	public AudioClip bgmBeach;
	private AudioSource audioSource;

	private int zone;		// -1	No BGM
							//  0	main menu
							//  1	Factory
							//  2	City
							//  3	Forest
							//  4	Beach

	public static BGM GetInstance()
	{
		return instance;
	}

	// called zero
	void Awake()
	{
		//Debug.Log("Awake");
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
			return;
		}
		else
		{
			instance = this;
		}
		DontDestroyOnLoad(gameObject);

		audioSource = GetComponent<AudioSource>();
		audioSource.loop = true;
		zone = -1;
	}

	// called first
	void OnEnable()
	{
		//Debug.Log("OnEnable called");
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	// called second
	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		//Debug.Log("OnSceneLoaded: " + scene.name);
		//Debug.Log(mode);

		int newZone = GetZone(scene);
		if (zone != newZone)
		{
			zone = newZone;
			PlayBGM(zone);
		}
	}

	// called third
	void Start()
	{
		//Debug.Log("Start");
	}

	// called when the game is terminated
	void OnDisable()
	{
		//Debug.Log("OnDisable");
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	/**
	 * Plays BGM according to the zone, automatically stopping any previous BGM.
	 */
	void PlayBGM(int zone)
	{
		if (zone == 0)
		{
			audioSource.clip = bgmMainMenu;
		}
		else if (zone == 1)
		{
			audioSource.clip = bgmFactory;
		}
		else if (zone == 2)
		{
			audioSource.clip = bgmCity;
		}
		else if (zone == 3)
		{
			audioSource.clip = bgmForest;
		}
		else if (zone == 4)
		{
			audioSource.clip = bgmBeach;
		}
		audioSource.Play();
	}

	/**
	 * Returns an integer according to the name of the scene.
	 */
	int GetZone(Scene scene)
	{
		if (scene.name == "MainMenu" || scene.name == "StartMenu" || scene.name == "TypeUserName" || scene.name == "LevelSelection")
		{
			return 0;
		}
		else if (scene.name == "Factory_Lv0" || scene.name == "Factory_Lv1" || scene.name == "Factory_Lv2")
		{
			return 1;
		}
		else if (scene.name == "City_Lv1" || scene.name == "City_lv2" || scene.name == "City_Lv3")
		{
			return 2;
		}
		else if (scene.name == "Forest_lv1" || scene.name == "Forest_lv2" || scene.name == "Forest_lv3")
		{
			return 3;
		}
		else if (scene.name == "beach level")
		{
			return 4;
		}
		else
		{
			return -1;
		}
	}
}
