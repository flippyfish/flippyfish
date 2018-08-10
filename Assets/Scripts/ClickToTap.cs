using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
*	Hold left-click to charge a leap, and release left-click to jump.
	The fish will turn toward the mouse when charging a leap.
	
	Sometimes the fish will backflip or not move very far. I'll figure that out.
*/
public class ClickToTap : MonoBehaviour
{
	public Slider chargeSlider;

	private float charge;
	private float MAX_CHARGE = 2;

	public bool isGrounded;

	private Quaternion prevRotation;
	private Vector3 prevPosition;
	private Vector3 spawn;
	
	Rigidbody rb;
	
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		spawn = transform.position;
		prevRotation = transform.rotation;
		prevPosition = transform.position;
		SetCharge(0);
	}

	// hit an obstacle, respawn at start
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Obstacle")
		{
			Respawn();
		}
	}
	
	void OnCollisionEnter()
	{
		//prevPosition = transform.position;
	}

	void OnCollisionStay()
	{
        float currentSpeed = rb.velocity.magnitude;
        if (currentSpeed < 0.1 && !isGrounded)
        {
            rb.velocity = new Vector3(0, 0, 0);
            prevPosition = transform.position;
            isGrounded = true;
        }
	}

	void OnCollisionExit()
	{
		isGrounded = false;
	}

	void FixedUpdate()
	{
        
		MouseBehavior();
	}

	// if I rotate the fish first, I can just leap forward
	void MouseBehavior()
	{
		int layerMask = 1 << 9;		// we will only raycast onto layer 9
		//layerMask = ~layerMask;
		if (Input.GetMouseButton(0) && isGrounded)	// if holding down the mouse
		{
			if (charge > MAX_CHARGE)		// if we already held the mouse too long, do nothing
				return;
			AddCharge(Time.deltaTime);
			if (charge > MAX_CHARGE)		// reset fish if mouse was held too long
			{
				chargeSlider.value = 0;
				transform.rotation = prevRotation;
				transform.position = prevPosition;
				return;
			}
			// face the mouse, if the cursor is over the ground
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
			{
				transform.position = prevPosition;
				transform.LookAt(hit.point);
                print(transform.rotation);
			}
			//print(charge);
		}
		if (Input.GetMouseButtonUp(0) && isGrounded)	// when the mouse is released
		{
			if (charge < 0.25 || charge > MAX_CHARGE)	// prevent overcharged clicks and quick clicks from executing a jump
			{
				SetCharge(0);
				transform.rotation = prevRotation;
				transform.position = prevPosition;
				return;
			}
			else
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

				

				if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
				{
					transform.LookAt(hit.point);	// face the mouse click
					//print(transform.rotation);
					prevRotation = transform.rotation;
					// note that the x, y, and z values of the jump are the strength in each direction

					//Vector3 dir = new Vector3(5.0f, 10.0f, 0.0f);
					Vector3 dir = transform.forward;
					dir = new Vector3(dir.x, 2.0f, dir.z);
					float leapStr = (charge + 1.0f) * 2.0f;
					dir = dir * leapStr;	// make the leap bigger
					//print(dir);
					rb.AddForce(dir, ForceMode.Impulse);
					SetCharge(0);
					isGrounded = false;
				}
				else
				{
					SetCharge(0);
				}
			}
		}
	}

	void Respawn()
	{
		rb = GetComponent<Rigidbody>();
		rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
		transform.rotation = Quaternion.identity;
		transform.position = spawn;
		prevRotation = transform.rotation;
		prevPosition = transform.position;
		SetCharge(0);
	}

	void AddCharge(float val)
	{
		charge += val;
		chargeSlider.value = charge * 100;
	}
	void SetCharge(float val)
	{
		charge = val;
		chargeSlider.value = charge * 100;
	}
}