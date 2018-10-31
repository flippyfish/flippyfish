using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePadAngled : MonoBehaviour
{

	public float charge;
	public float angle;				// 0 to 360
	private Quaternion rotation;

	// Use this for initialization
	void Start()
	{
		rotation = Quaternion.Euler(0, angle, 0);
	}

	private void OnCollisionEnter(Collision collision)
	{

		if (collision.gameObject.name == "Fish_Player")
		{
			Debug.Log("Bounce Pad, Fish on me!");
			collision.gameObject.transform.rotation = rotation;
			collision.gameObject.GetComponent<FishMovement>().DoJump(charge);
		}
	}
}
