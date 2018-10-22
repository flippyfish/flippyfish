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
    public GameObject fishKinematic;            // "ghost" fish that faces the cursor while charging a jump
    public GameObject distanceMeasure;          // measuring tape that shows the player how far the fish can go
    public GameObject distanceBar;              // moving bar that shows how far the fish would jump
    public Text jumpCounter;

    public int variance;						// maximum random euler angle applied to a jump
    public int layerMask = 1 << 9;              // we will only raycast onto layer 9

    public bool isGrounded;                     // can only charge a leap while grounded
    public bool inControl;						// set to false upon level completion, or when about to respawn

    private bool canceledClick;                  // determines whether a held click   is to be canceled or not

    public Quaternion respawnRotation;
    public Vector3 respawnPosition;

    public int jumps;

    private PowerBar powerBar;                  // other fish script

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        respawnRotation = transform.rotation;
        respawnPosition = transform.position;
        isGrounded = false;
        inControl = true;
        canceledClick = false;
        SetJump(0);
        fishKinematic.SetActive(false);
        distanceMeasure.SetActive(false);
        distanceBar.SetActive(false);
        powerBar = gameObject.AddComponent<PowerBar>();

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
        if (Input.GetMouseButtonDown(1) || canceledClick)       // If Z button is hit then cancel current click
        {
            canceledClick = true;
            ResetSliderAndFish();
            return;
        }

        powerBar.StartCharge();
        PointJumpIndicators();
    }

    // Called during a frame when the mouse is released (ie. it was held the previous frame) and the fish is grounded.
    public void ReleaseJump()
    {
        if (canceledClick)//player wants to cancel current jump so we ignore the current click
        {
            canceledClick = false;
            return;
        }
        else if (false)//(charge < 0.25)			// prevent overcharged clicks from executing a jump
        {
            ResetSliderAndFish();
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
            doJump(powerBar.GetCurrentPower());
        }
    }


    /*
     * I am adding this here becuase i think it is really a waste of time if i write new code to do the jump in other script when i can use already built in functions and variables.
     * 
     * I want the functions to be more dynamically callable/separated into multiple scripts/functions.
     * 
     * Jump function from Jon & Chanil
     * 
     * - Chail Park -
     */
    public void JumpFromBP(float charge)
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
        jump = new Vector3(jump.x, 1.3f, jump.z);
        float str = (charge + 1.0f) * 3.0f;
        jump = jump * str;
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        rb.AddForce(jump, ForceMode.Impulse);

        AddJump(1);
        ResetSliderAndFish();
        isGrounded = false;
    }


    /*
     * Jump function from Jon & James.
     *
     */
    public void doJump(float charge)
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
        jump = new Vector3(jump.x, 1.3f, jump.z);
        float str = (powerBar.GetCurrentPower() + 1.0f) * 3.0f;
        jump = jump * str;
        rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
        rb.AddForce(jump, ForceMode.Impulse);

        AddJump(1);
        ResetSliderAndFish();
        isGrounded = false;
    }


    /*
     * Moves and rotates the ghost fish, the distance tape measure, and the moving distance bar.
     *  - Jonathan
     */
    public void PointJumpIndicators()
    {
        // if objects are hidden, make them visible
        if (fishKinematic.gameObject.activeSelf == false)
        {
            fishKinematic.gameObject.SetActive(true);
            distanceMeasure.gameObject.SetActive(true);
            distanceBar.gameObject.SetActive(true);
        }

        // start with ghost fish
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            // move the kinematic fish to where the real fish is
            fishKinematic.transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z);

            // now make it look toward the point
            float lookY = fishKinematic.gameObject.transform.position.y;
            Vector3 lookAt = new Vector3(hit.point.x, lookY, hit.point.z);  // the kinematic fish will look on its own y level
            fishKinematic.transform.LookAt(lookAt);

            // finally, rotate the fish by 90 degrees so its head faces forward
            Quaternion rotate90 = Quaternion.Euler(0, 90, 0);
            fishKinematic.transform.rotation *= rotate90;


            // do the distance measure
            distanceMeasure.transform.position = transform.position;
            distanceMeasure.transform.LookAt(lookAt);


            // do the moving distance bar
            Vector3 fishPosition = transform.position;
            float degrees = distanceMeasure.gameObject.transform.rotation.eulerAngles.y;
            float radians = degrees * Mathf.Deg2Rad;
            float distance = powerBar.GetCurrentPower() * 8.0f;

            float x = fishPosition.x + Mathf.Sin(radians) * distance;
            float y = fishPosition.y;
            float z = fishPosition.z + Mathf.Cos(radians) * distance;

            distanceBar.transform.position = new Vector3(x, y, z);
            distanceBar.transform.rotation = Quaternion.Euler(0, degrees, 0);
        }
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

    public void resetPowerBar()
    {
        this.powerBar.StopCharge();
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
        fishKinematic.gameObject.SetActive(false);
        distanceMeasure.gameObject.SetActive(false);
        distanceBar.gameObject.SetActive(false);
        return;
    }
}
