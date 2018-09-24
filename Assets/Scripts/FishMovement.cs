using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
*	Hold left-click to charge a leap, and release left-click to jump.
	The fish will turn toward the mouse when charging a leap.
*/
public class FishMovement : MonoBehaviour
{
	public Text jumpCounter;

	public Slider chargeSlider;
	public float charge;
	public float MAX_CHARGE = 2;				// 2 is max jump multiplier
	public float CHARGE_TO_CANCEL = 2.25f;
	public int variance;						// maximum random euler angle applied to a jump

	public bool isGrounded;						// can only charge a leap while grounded
	public bool inControl;						// set to false upon level completion, or when about to respawn

	public Quaternion prevRotation;
	public Vector3	  prevPosition;
	public Quaternion respawnRotation;
	public Vector3	  respawnPosition;

	public int jumps;
	
	Rigidbody rb;
	
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		respawnRotation = transform.rotation;
		respawnPosition = transform.position;
		prevRotation = transform.rotation;
		prevPosition = transform.position;
		isGrounded = false;
		inControl = true;
		SetCharge(0);
		SetJump(0);
	}

	void Update()
	{
		if (inControl)
			MouseBehavior();
	}

	/**
	*	If the left mouse button is held down, the fish will rotate toward the cursor and charge.
	*	If the left mouse button is released and the fish is charged, it will leap toward the cursor.
	*/
	void MouseBehavior()
	{
		int layerMask = 1 << 9;		// we will only raycast onto layer 9
		//layerMask = ~layerMask;

		if (Input.GetMouseButton(0) && isGrounded)	// IF HOLDING DOWN THE MOUSE
		{
			if (charge > CHARGE_TO_CANCEL)		// if we already held the mouse too long, do nothing
				return;
			AddCharge(Time.deltaTime * 1.5f);
			if (charge > CHARGE_TO_CANCEL)		// reset fish if mouse was held too long
			{
				chargeSlider.value = 0;
				transform.rotation = prevRotation;
				transform.position = prevPosition;
				rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
				return;
			}

			// face the cursor
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
			{
				// don't allow jump direction to be behind the player
				float lookZ = hit.point.z;
				if (lookZ < transform.position.z)
					lookZ = transform.position.z;
				
				Vector3 lookAt = new Vector3(hit.point.x, transform.position.y, lookZ);	// the fish will look on its own y level
				transform.LookAt(lookAt);
				//transform.position = prevPosition;
				transform.position = new Vector3(prevPosition.x, prevPosition.y + 0.15f, prevPosition.z);
			}
		}
		if (Input.GetMouseButtonUp(0) && isGrounded)	// WHEN THE MOUSE IS RELEASED
		{
			if (charge > CHARGE_TO_CANCEL)	// the fish is already in its old state, so don't reset again
			{
				SetCharge(0);
				return;
			}
			else if (charge < 0.25)			// prevent overcharged clicks from executing a jump
			{
				SetCharge(0);
				transform.rotation = prevRotation;
				transform.position = prevPosition;
				rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
				return;
			}
			else
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
				{
					// don't allow jump direction to be behind the player
					float lookZ = hit.point.z;
					if (lookZ < transform.position.z)
						lookZ = transform.position.z;
					Vector3 lookAt = new Vector3(hit.point.x, transform.position.y, lookZ);
					transform.LookAt(lookAt);
					transform.position = new Vector3(prevPosition.x, prevPosition.y + 0.15f, prevPosition.z);

					// apply small random rotation, ensuring the overall angle isn't backward
					if (charge > 1)
						transform.rotation = transform.rotation * Quaternion.Euler(0, Random.Range(-variance * charge, variance * charge), 0);
					if (transform.rotation.eulerAngles.y > 180 && transform.rotation.eulerAngles.y < 270)
						transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 270, transform.rotation.eulerAngles.z);
					else if (transform.rotation.eulerAngles.y < 180 && transform.rotation.eulerAngles.y > 90)
						transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 90, transform.rotation.eulerAngles.z);

					if (charge > MAX_CHARGE)
						charge = MAX_CHARGE;

					// apply the jump force!
					// note that the x, y, and z values of the jump vector are the strength in each direction
					Vector3 jump = transform.forward;
					jump = new Vector3(jump.x, 1.3f, jump.z);
					float str = (charge + 1.0f) * 3.0f;
					jump = jump * str;
					rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
					rb.AddForce(jump, ForceMode.Impulse);

					AddJump(1);
					SetCharge(0);
					isGrounded = false;
				}
				else	// if no valid raycast -- eg. if cursor is in the sky
				{
					SetCharge(0);
					transform.rotation = prevRotation;
					transform.position = prevPosition;
					rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
				}
			}
		}
	}

	public void AddCharge(float val)
	{
		charge += val;
		chargeSlider.value = charge * 100;
	}
	public void SetCharge(float val)
	{
		charge = val;
		chargeSlider.value = charge * 100;
	}

	public void AddJump(int val)
	{
		jumps += val;
		jumpCounter.text = "Jumps: " + jumps.ToString();
	}
	public void SetJump(int val)
	{
		jumps = val;
		jumpCounter.text = "Jumps: " + jumps.ToString();
	}
}
