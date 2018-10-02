using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour {

	private Rigidbody rb;
	public float speed;

	// Use this for initialization
	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		rb.velocity = transform.forward * speed;
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Player")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
