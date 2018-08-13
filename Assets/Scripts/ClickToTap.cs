using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
*	Hold left-click to charge a leap, and release left-click to jump.
	The fish will turn toward the mouse when charging a leap.
*/
public class ClickToTap : MonoBehaviour
{
	public GameObject winScreen;
	public Text jumpCounter;

	public Slider chargeSlider;
	private float charge;
	private float MAX_CHARGE = 2;
	private float CHARGE_TO_CANCEL = 2.25f;
	public int variance;		// maximum random angle applied to a jump

	private bool isGrounded;		// can only charge a leap while grounded
	private bool levelOver;		// set to true upon level completion

	private Quaternion prevRotation;
	private Vector3 prevPosition;
	private Vector3 respawn;

	public int jumps;
	
	Rigidbody rb;
	
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		respawn = transform.position;
		prevRotation = transform.rotation;
		prevPosition = transform.position;
		isGrounded = false;
		levelOver = false;
		SetCharge(0);
		SetJump(0);
	}

	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Obstacle")	// hit an obstacle, respawn at start
		{
			Respawn();
		}
		if (other.tag == "Pond")		// update respawn
		{
			Vector3 pondPos = other.transform.position;
			respawn = new Vector3(pondPos.x, pondPos.y + 0.5f, pondPos.z);
		}
		if (other.tag == "Goal")		// disable jumps after beating the level
		{
			levelOver = true;
			winScreen.SetActive(true);
		}
	}

	void OnCollisionStay()
	{
		print(transform.position);
        float currentSpeed = rb.velocity.magnitude;
        if (currentSpeed < 0.1 && !isGrounded)
        {
            rb.velocity = new Vector3(0, 0, 0);
            prevPosition = transform.position;
			prevRotation = transform.rotation;
            isGrounded = true;
        }
	}

	void OnCollisionExit()
	{
		isGrounded = false;
	}

	void Update()
	{
        if (!levelOver)
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
			AddCharge(Time.deltaTime);
			if (charge > CHARGE_TO_CANCEL)		// reset fish if mouse was held too long
			{
				chargeSlider.value = 0;
				transform.rotation = prevRotation;
				transform.position = prevPosition;
				return;
			}

			// face the cursor
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
			{
				Vector3 lookAt = new Vector3(hit.point.x, transform.position.y, hit.point.z);	// the fish will look on its own y level
				transform.position = prevPosition;
				transform.LookAt(lookAt);
			}
		}
		if (Input.GetMouseButtonUp(0) && isGrounded)	// when the mouse is released
		{
			if (charge < 0.25 || charge > CHARGE_TO_CANCEL)	// prevent overcharged clicks and quick clicks from executing a jump
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
					Vector3 lookAt = new Vector3(hit.point.x, transform.position.y, hit.point.z);
					transform.LookAt(lookAt);

					// apply random rotation
					if (charge > 1)
						transform.rotation = transform.rotation * Quaternion.Euler(0, Random.Range(-variance * charge, variance * charge), 0);

					if (charge > MAX_CHARGE)
						charge = MAX_CHARGE;

					// note that the x, y, and z values of the jump are the strength in each direction
					Vector3 dir = transform.forward;
					dir = new Vector3(dir.x, 2.0f, dir.z);
					float leapStr = (charge + 1.0f) * 2.0f;
					dir = dir * leapStr;
					rb.AddForce(dir, ForceMode.Impulse);

					AddJump(1);
					SetCharge(0);
					isGrounded = false;
				}
				else	// if no valid raycast -- should not happen
				{
					SetCharge(0);
				}
			}
		}
	}

	void Respawn()
	{
		rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
		transform.rotation = Quaternion.identity;
		transform.position = respawn;
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

	void AddJump(int val)
	{
		jumps += val;
		jumpCounter.text = "Jumps: " + jumps.ToString();
	}
	void SetJump(int val)
	{
		jumps = val;
		jumpCounter.text = "Jumps: " + jumps.ToString();
	}
}