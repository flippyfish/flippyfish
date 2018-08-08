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
	}
	
	// Update is called once per frame
	void Update ()
	{
		rb.velocity = transform.forward * speed * Time.deltaTime;
		//rb.AddForce(0.0f, 0.0f, 1.0f, ForceMode.Force);
	}
}
