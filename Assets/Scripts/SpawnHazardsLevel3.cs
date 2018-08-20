using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHazardsLevel3 : MonoBehaviour {

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
            // spawn car
            Vector3 spawnPosition = new Vector3(-40.0f, 1.644637f, 22.0f);
            Quaternion spawnRotation = Quaternion.LookRotation(new Vector3(1.0f, 0.0f, 0.0f));
            Instantiate(car, spawnPosition, spawnRotation);

            // time between cars
            yield return new WaitForSeconds(carInterval);
        }
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
