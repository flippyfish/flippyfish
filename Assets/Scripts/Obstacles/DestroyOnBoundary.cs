using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Put this on a Boundary trigger box, NOT anything else!
 */
public class DestroyOnBoundary : MonoBehaviour
{

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Obstacle")
		{
			// collider object may not be highest in hierarchy -- get root gameobject to destroy
			GameObject obstacle = other.gameObject.transform.root.gameObject;
			Destroy(obstacle);
		}
	}
}
