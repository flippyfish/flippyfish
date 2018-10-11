using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour {

	public GameObject car1;
	public GameObject car2;
	public GameObject car3;
	public float carInterval;
	public float spawnX;
	public float spawnY;
	public float spawnZ;
	public float spawnRot;

	private float rand;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(SpawnCars());
	}

	IEnumerator SpawnCars()
	{
        while (true)
        {
        	rand = Random.Range(0.0f, 3.0f);
            // spawn car
            Vector3 spawnPosition = new Vector3(spawnX, spawnY, spawnZ);
            Quaternion spawnRotation = Quaternion.LookRotation(new Vector3(spawnRot, 0.0f, 0.0f));
            if(rand<1.0)
            {
            	Instantiate(car1, spawnPosition, spawnRotation);
            }
            if(rand<2.0 && rand>1.0)
            {
            	Instantiate(car2, spawnPosition, spawnRotation);
            }
            if(rand>2.0)
            {
            	Instantiate(car3, spawnPosition, spawnRotation);
            }

            // time between cars
            yield return new WaitForSeconds(carInterval);

        }
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
