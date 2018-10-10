using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour {

	public GameObject car;
	public float carInterval;
	public float spawnX;
	public float spawnY;
	public float spawnZ;
	public float spawnRot;

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
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);
            Quaternion spawnRotation = Quaternion.LookRotation(new Vector3(spawnRot, 0.0f, 0.0f));
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
