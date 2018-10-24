using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishIndicators : MonoBehaviour
{

	private GameObject fishKinematic;		// "ghost" fish that faces the cursor while charging a jump
	private GameObject distanceMeasure;		// measuring tape that shows the player how far the fish can go
	private GameObject distanceBar;			// moving bar that shows how far the fish would jump

	private PowerBar powerBar;

	public bool active;

	// Use this for initialization
	void Start ()
	{
		fishKinematic   = GameObject.Find("Fish_Player kinematic");
		distanceMeasure = GameObject.Find("Distance measure");
		distanceBar     = GameObject.Find("Distance bar");
		//powerBar        = GetComponent<PowerBar>();	// PowerBar script is added to fish at runtime, so must get it later

		StopIndicators();
	}

	public bool IndicatorsActive ()
	{
		return active;
	}
	
	public void StartIndicators ()
	{
		fishKinematic.SetActive(true);
		distanceMeasure.SetActive(true);
		distanceBar.SetActive(true);
		active = true;
	}

	public void StopIndicators ()
	{
		fishKinematic.SetActive(false);
		distanceMeasure.SetActive(false);
		distanceBar.SetActive(false);
		active = false;
	}

	/*
     * Moves and rotates the ghost fish, the distance tape measure, and the moving distance bar.
     *  - Jonathan
     */
    public void PointJumpIndicators(int layerMask)
    {
        // if objects are hidden, make them visible
        if (!active)
        {
            StartIndicators();
        }

        if (powerBar == null)
        {
            powerBar = GetComponent<PowerBar>();
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
}
