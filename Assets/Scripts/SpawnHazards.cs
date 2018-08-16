using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHazards : MonoBehaviour {

	public GameObject car;
    public GameObject car2;
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
            // spawn car 1
            Vector3 spawnPosition = new Vector3(-40.0f, 1.644637f, 22.0f);
            Quaternion spawnRotation = Quaternion.LookRotation(new Vector3(1.0f, 0.0f, 0.0f));
            Instantiate(car, spawnPosition, spawnRotation);

            // spawn car 2
            Vector3 spawnPosition2 = new Vector3(30.42976f, 1.5f, 22.0f);
            Quaternion spawnRotation2 = Quaternion.LookRotation(new Vector3(1.0f, 0.0f, 0.0f));
            Instantiate(car2, spawnPosition2, spawnRotation2);

            // time between cars
            yield return new WaitForSeconds(carInterval);
        }
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
