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
	public GameObject fishKinematic;			// "ghost" fish that faces the cursor while charging a jump
	public Text jumpCounter;
	public Slider chargeSlider;
	public float charge;
	public float MAX_CHARGE = 2;				// 2 is max jump multiplier
	public float CHARGE_TO_CANCEL = 2.25f;
	public int variance;						// maximum random euler angle applied to a jump
    public float chargeSpeed;                   // charge speed i.e. value charge bar increases/decreases by
    public float chargeAcceleration;            // rate at which charge speed changes
    public int layerMask = 1 << 9;				// we will only raycast onto layer 9

	public bool isGrounded;						// can only charge a leap while grounded
	public bool inControl;						// set to false upon level completion, or when about to respawn
    public bool isIncreasing;                   // determines whether power bar is increasing or decreasing
    public bool canceledClick;                  // determines whether a held click   is to be canceled or not

	public Quaternion respawnRotation;
	public Vector3	  respawnPosition;

	public int jumps;
	
	Rigidbody rb;
	
	void Start()
	{
		rb = GetComponent<Rigidbody>();
		respawnRotation = transform.rotation;
		respawnPosition = transform.position;
        chargeSpeed = 0.1f;
        chargeAcceleration = 0.1f;
        isGrounded = false;
		inControl = true;
        isIncreasing = true;
        canceledClick = false;
		SetCharge(0);
		SetJump(0);
		fishKinematic.SetActive(false);
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
		if (Input.GetMouseButton(0) && isGrounded)	// IF HOLDING DOWN THE MOUSE
		{
			ChargeJump();
		}

		if (Input.GetMouseButtonUp(0) && isGrounded)	// WHEN THE MOUSE IS RELEASED
		{
			ReleaseJump();
		}
	}

	// Called during a frame when the mouse is held down and the fish is grounded.
	public void ChargeJump()
	{
		if (Input.GetKey("z") || canceledClick)		// If Z button is hit then cancel current click
		{
			canceledClick = true;
			ResetSliderAndFish();
			return;
		}

		UpdateCharge();
		PointKinematicAtCursor();
	}

	// Called during a frame when the mouse is released (ie. it was held the previous frame) and the fish is grounded.
	public void ReleaseJump()
	{
		if (canceledClick)//player wants to cancel current jump so we ignore the current click
		{
			canceledClick = false;
			return;
		}
		else if (charge < 0.25)			// prevent overcharged clicks from executing a jump
		{
			ResetSliderAndFish();
			return;
		}
		else
		{
			PointAtCursor();

			// apply small random rotation
			if (charge > 1)
			{
				transform.rotation = transform.rotation * Quaternion.Euler(0, Random.Range(-variance * charge, variance * charge), 0);
			}

			if (charge > MAX_CHARGE)
			{
				charge = MAX_CHARGE;
			}

			// apply the jump force!
			// note that the x, y, and z values of the jump vector are the strength in each direction
			Vector3 jump = transform.forward;
			jump = new Vector3(jump.x, 1.3f, jump.z);
			float str = (charge + 1.0f) * 3.0f;
			jump = jump * str;
			rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			rb.AddForce(jump, ForceMode.Impulse);

			AddJump(1);
			ResetSliderAndFish();
			isGrounded = false;
		}
	}

	public void PointKinematicAtCursor()
	{
		if (fishKinematic.gameObject.activeSelf == false)
		{
			fishKinematic.gameObject.SetActive(true);
		}

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
		{
			// move the kinematic fish to where the real fish is
			fishKinematic.transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z);

			// now make it look toward the point
			float lookY = fishKinematic.gameObject.transform.position.y;
			Vector3 lookAt = new Vector3(hit.point.x, lookY, hit.point.z);	// the kinematic fish will look on its own y level
			fishKinematic.transform.LookAt(lookAt);
		}
	}

	public void PointAtCursor()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
		{
			transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z);
			Vector3 lookAt = new Vector3(hit.point.x, transform.position.y, hit.point.z);	// the fish will look on its own y level
			transform.LookAt(lookAt);
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
        fishKinematic.gameObject.SetActive(false);
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
