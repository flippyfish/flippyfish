using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{

	public Quaternion forward;

	// Use this for initialization
	void Start ()
	{
		forward = Quaternion.LookRotation(new Vector3(0.0f, 0.0f, 1.0f));
	}

	void OnCollisionStay(Collision collision)
	{
		MoveObjects(collision);
	}

	void MoveObjects (Collision collision)
	{
		GameObject o = collision.gameObject;
		if (o.tag == "Player")
		{
			o.transform.position += new Vector3(0.0f, 0.0f, 0.05f);
		}
	}
}
