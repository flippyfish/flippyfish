using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishOxygen : MonoBehaviour
{

	private FishCollision fishCollision;		// other fish script
	public Slider oxygenSlider;

	public int oxygen;
	public int OXYGEN_MAX;

	public float timeLastInWater;
	public bool inWater;

	// Use this for initialization
	void Start ()
	{
		fishCollision = GetComponent<FishCollision>();
		timeLastInWater = Time.time;
		SetOxygen(OXYGEN_MAX);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!inWater)
		{
			OxygenUpdate();
		}
	}

	void OxygenUpdate ()
	{
		int newOxygen = OXYGEN_MAX - (int)(Time.time - timeLastInWater);
		if (oxygen != newOxygen)	// only update GUI if the variable changed
		{
			SetOxygen(newOxygen);
		}
		if (oxygen <= 0 && fishCollision.respawning == false)
		{
			StartCoroutine(fishCollision.Respawn());
		}
	}

	// Called by FishCollision script
	public void EnterWater ()
	{
		inWater = true;
		SetOxygen(OXYGEN_MAX);
	}

	// Called by FishCollision script
	public void ExitWater ()
	{
		inWater = false;
		timeLastInWater = Time.time;
	}

	public void SetOxygen (int val)
	{
		oxygen = val;
		oxygenSlider.value = oxygen;
		//print(oxygen);
	}
}
