using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 *	This script maintains the fish's oxygen level. Oxygen acts as a time limit.

 *	While the fish is not in water, its oxygen bar slowly depletes. If the fish runs out of oxygen it respawns.
 *	Water sources instantly fill the oxygen bar.
 */
public class FishOxygen : MonoBehaviour
{

	private FishCollision fishCollision;	// other fish script
	public Slider oxygenSlider;

	public int oxygen;
	public int OXYGEN_MAX;					// suggest 20

	public float timeLastInWater;
	private bool inWater;

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
		if (oxygen <= 0 && !fishCollision.isRespawning())
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

	public bool fishInWater ()
	{
		return inWater;
	}

	public void SetOxygen (int val)
	{
		oxygen = val;
		oxygenSlider.value = oxygen;
	}
}
