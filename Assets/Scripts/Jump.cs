using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
	// https://answers.unity.com/questions/1020197/can-someone-help-me-make-a-simple-jump-script.html
	public Vector3 jump;
	public float jumpForce = 2.0f;

	public bool isGrounded;
	Rigidbody rb;
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		jump = new Vector3(0.0f, 2.0f, 2.0f);
	}

	void OnCollisionStay()
	{
		isGrounded = true;
	}

	void FixedUpdate()
	{
		if(Input.GetMouseButtonDown(0) && isGrounded)
		{
			rb.AddForce(jump * jumpForce, ForceMode.Impulse);
			isGrounded = false;
		}
	}
}