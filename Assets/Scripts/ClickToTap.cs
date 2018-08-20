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
	private float MAX_CHARGE = 2;				// 2 is max jump multiplier
	private float CHARGE_TO_CANCEL = 2.25f;
	public int variance;						// maximum random euler angle applied to a jump

	private bool isGrounded;					// can only charge a leap while grounded
	private bool inControl;						// set to false upon level completion, or when about to respawn

	private Quaternion prevRotation;
	private Vector3    prevPosition;
	private Quaternion respawnRotation;
	private Vector3    respawnPosition;

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

	// for solid objects the fish hits, eg. a car
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            if (inControl)
            {
                inControl = false;
                StartCoroutine(Respawn());
            }
        }
    }

    // for objects the fish can move through, eg. a pond
    void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Obstacle")	// hit an obstacle, respawn at start
		{
            if (inControl)
            {
                inControl = false;
                StartCoroutine(Respawn());
            }
		}
		if (other.tag == "Pond")		// update respawn
		{
			Vector3 pondPos = other.transform.position;
			respawnPosition = new Vector3(pondPos.x, pondPos.y + 0.5f, pondPos.z);
		}
		if (other.tag == "Goal")		// end of level
		{
			inControl = false;			// disable jumping after beating the level
			winScreen.SetActive(true);
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Boundary")
		{
			StartCoroutine(Respawn());
		}
	}

	// detect when the fish has landed and come to a stop; record its state
	void OnCollisionStay()
	{
        if (!isGrounded && rb.velocity.magnitude < 0.05)
        {
            rb.velocity = new Vector3(0, 0, 0);
            prevPosition = transform.position;
			prevRotation = transform.rotation;
            isGrounded = true;	// now we can charge another jump
        }
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
				transform.position = new Vector3(prevPosition.x, prevPosition.y + 0.2f, prevPosition.z);
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
					transform.position = new Vector3(prevPosition.x, prevPosition.y + 0.2f, prevPosition.z);

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

    IEnumerator Respawn()
	{
        inControl = false;
        yield return new WaitForSeconds(1);

		rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
		transform.rotation = respawnRotation;
		transform.position = respawnPosition;
		prevRotation = transform.rotation;
		prevPosition = transform.position;

		SetCharge(0);
        inControl = true;
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