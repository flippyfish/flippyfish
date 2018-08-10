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
		rb.velocity = transform.forward * speed * Time.deltaTime;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//rb.AddForce(0.0f, 0.0f, 1.0f, ForceMode.Force);
	}
}
