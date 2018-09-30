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
    public float chargeSpeed;                   // charge speed i.e. value charge bar increases/decreases by
    public float chargeAcceleration;            // rate at which charge speed changes

	public bool isGrounded;						// can only charge a leap while grounded
	public bool inControl;						// set to false upon level completion, or when about to respawn
    public bool isIncreasing;                   // determines whether power bar is increasing or decreasing
    public bool canceledClick;                  // determines whether a held click   is to be canceled or not

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
        chargeSpeed = 0.1f;
        chargeAcceleration = 0.1f;
        isGrounded = false;
		inControl = true;
        isIncreasing = true;
        canceledClick = false;
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
			
			if (Input.GetKey("z") || canceledClick)		// If Z button is hit then cancel current click
			{
                canceledClick = true;
                ResetSliderAndFish();
                return;
			}

            UpdateCharge();

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
			if (canceledClick)//player wants to cancel current jump so we ignore the current click
			{
                canceledClick = false;
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
    public void ResetSliderAndFish()
    {
        SetCharge(0);
        transform.rotation = prevRotation;
        transform.position = prevPosition;
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        return;
    }
    public void UpdateCharge()
    {
        if (isIncreasing)
        {
            if ((charge + Time.deltaTime * (1.5f + chargeSpeed)) <= MAX_CHARGE)
            {
                AddCharge(Time.deltaTime * (1.5f + chargeSpeed));
                if (charge >= ((CHARGE_TO_CANCEL / 4) * 3))
                {
                    chargeSpeed += chargeAcceleration * 2;
                }
                else
                {
                    chargeSpeed += chargeAcceleration;
                }
            }
            else
            {
                isIncreasing = false;
                AddCharge(Time.deltaTime * (-1.5f - chargeSpeed));
                chargeSpeed -= chargeAcceleration * 2;
            }
        }
        else
        {
            if ((charge + Time.deltaTime * (-1.5f - chargeSpeed)) >= 0)
            {
                AddCharge(Time.deltaTime * (-1.5f - chargeSpeed));
                if (charge >= ((MAX_CHARGE / 4) * 3))
                {
                    chargeSpeed -= chargeAcceleration * 2;
                }
                else
                {
                    chargeSpeed -= chargeAcceleration;
                }
            }
            else
            {
                isIncreasing = true;
                AddCharge(Time.deltaTime * (1.5f + chargeSpeed));
                chargeSpeed = 0.1f;//At 0 so we reset charge speed and acceleration to eliminate speed leak (bar continually gains speed)
                chargeAcceleration = 0.1f;
                chargeSpeed += chargeAcceleration;
            }
        }
    }
}
