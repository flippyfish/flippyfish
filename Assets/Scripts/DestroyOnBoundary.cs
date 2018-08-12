using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnBoundary : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Obstacle")
		{
			Destroy(other.gameObject);
		}
	}
}
