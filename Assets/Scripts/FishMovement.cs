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

    private FishIndicators fishIndicators;      // other fish script
    private PowerBar powerBar;                  // other fish script

    public Text jumpCounter;

    public int variance;						// maximum random euler angle applied to a jump
    public int layerMask = 1 << 9;              // we will only raycast onto layer 9

    public bool isGrounded;                     // can only charge a leap while grounded
    public bool inControl;						// set to false upon level completion, or when about to respawn

    public bool canceledClick;                  // determines whether a held click   is to be canceled or not

    public Quaternion respawnRotation;
    public Vector3 respawnPosition;

    public int jumps;

    Rigidbody rb;

    void Start()
    {
        fishIndicators = GetComponent<FishIndicators>();
        powerBar = GetComponent<PowerBar>();
        rb = GetComponent<Rigidbody>();
        respawnRotation = transform.rotation;
        respawnPosition = transform.position;
        isGrounded = false;
        inControl = true;
        canceledClick = false;
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
        if (Input.GetMouseButton(0) && isGrounded)  // IF HOLDING DOWN THE MOUSE
        {
            ChargeJump();
        }

        if (Input.GetMouseButtonUp(0) && isGrounded)    // WHEN THE MOUSE IS RELEASED
        {
            ReleaseJump();
        }
    }

    // Called during a frame when the mouse is held down and the fish is grounded.
    public void ChargeJump()
    {
        if (Input.GetMouseButtonDown(1) || canceledClick)       // right-click cancels the charging jump
        {
            canceledClick = true;
            ResetSliderAndFish();
            return;
        }

        powerBar.StartCharge();
        fishIndicators.PointJumpIndicators(layerMask);
    }

    // Called during a frame when the mouse is released (ie. it was held the previous frame) and the fish is grounded.
    public void ReleaseJump()
    {
        if (canceledClick)//player wants to cancel current jump so we ignore the current click
        {
            canceledClick = false;
            return;
        }
        else if (false)//(charge < 0.25)
        {
            ResetSliderAndFish(); //Too small of a charge does not trigger jump
            return;
        }
        else
        {
            PointAtCursor();
            float charge = powerBar.GetCurrentPower();
            // apply small random rotation
            if (charge > 1)
            {
                transform.rotation = transform.rotation * Quaternion.Euler(0, Random.Range(-variance * charge, variance * charge), 0);
            }
            DoJump(powerBar.GetCurrentPower());
        }
    }


    /*
     * This jump function assumes the fish is already pointing in the correct direction.
     * Call this after PointToCursor().
     */
    public void DoJump(float charge)
    {
        // give the fish a spin
        // the x y z values of angularVelocity are NOT relative to the current rotation!
        // so we localize these x y z values using sine and cosine functions
        float radians = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        float spinA = 4.0f * Mathf.Cos(radians);
        float spinB = 4.0f * Mathf.Sin(radians);
        int chooseSpin = Random.Range(0, 2);
        if (chooseSpin == 1)
        {   // spin along x axis
            rb.angularVelocity = new Vector3(spinA, 0.0f, -spinB);
        }
        else
        {   // spin along z axis
            rb.angularVelocity = new Vector3(spinB, 0.0f, spinA);
        }
        // apply the jump force!
        // note that the x, y, and z values of the jump vector are the strength in each direction
        Vector3 jump = transform.forward;
        jump = new Vector3(jump.x, 1.5f, jump.z);
        float str = (charge + 1.0f) * 3.0f;
        jump = jump * str;
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        rb.AddForce(jump, ForceMode.Impulse);

        AddJump(1);
        ResetSliderAndFish();
        isGrounded = false;
        GetComponent<FishSound>().playJumpSound();
    }

    public void PointAtCursor()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z);
            Vector3 lookAt = new Vector3(hit.point.x, transform.position.y, hit.point.z);   // the fish will look on its own y level
            transform.LookAt(lookAt);
        }
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
        powerBar.StopCharge();
        fishIndicators.StopIndicators();
    }
}
