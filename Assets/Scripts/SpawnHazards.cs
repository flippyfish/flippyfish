using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHazards : MonoBehaviour {

	public GameObject car;
	public float carInterval;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(SpawnCars());
	}

	IEnumerator SpawnCars()
	{
		while (true)
		{
			Vector3 spawnPosition = new Vector3 (-3.0f, 1.0f, 10.0f);
			Quaternion spawnRotation = Quaternion.LookRotation(new Vector3(1.0f, 0.0f, 0.0f));
			Instantiate (car, spawnPosition, spawnRotation);
			yield return new WaitForSeconds(carInterval);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
