using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCar : MonoBehaviour {

	public GameObject car1;
	public GameObject car2;
	public GameObject car3;
	public float carInterval;

	public Vector3 spawnPosition;
	public float rotationY;
	//public float rotationX;
	//public float rotationZ;
	private Quaternion spawnRotation;

	private float rand;

	// Use this for initialization
	void Start ()
	{
		spawnRotation = Quaternion.Euler(0.0f, rotationY, 0.0f);
		StartCoroutine(SpawnCars());
	}

	IEnumerator SpawnCars()
	{
        while (true)
        {
        	rand = Random.Range(0.0f, 3.0f);
            // spawn car
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
