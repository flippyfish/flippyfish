using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnBoundary : MonoBehaviour {

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Obstacle")
		{
			Destroy(other.gameObject);
		}
	}
}
