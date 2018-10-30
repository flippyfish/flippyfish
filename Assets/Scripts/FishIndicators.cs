using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *	This script displays a jump indicator when called for by the FishMovement script.
 *	
 *	- fishKinematic is a ghost fish that points in the jump direction like an arrow.
 *	- distanceMeasure is a line that points in the jump direction. Its length is the maximum jump distance.
 *	- distanceBar is a moving bar perpendicular to the distanceMeasure. It moves up and down the distanceMeasure according to jump charge.
 *	  distanceBar's location is where the fish is estimated to land.
 */
public class FishIndicators : MonoBehaviour
{

	public GameObject fishKinematic;		// "ghost" fish that faces the cursor while charging a jump
	public GameObject distanceMeasure;		// stick that shows the player the direction the fish will go
	public GameObject distanceBar;			// moving bar that shows how far the fish would jump

	private PowerBar powerBar;				// other fish script

	public float scale;						// calibrate how far the moving distance bar travels
	private bool active;

	// Use this for initialization
	void Start ()
	{
		fishKinematic   = Instantiate(fishKinematic);
		distanceMeasure = Instantiate(distanceMeasure);
		distanceBar     = Instantiate(distanceBar);
		powerBar        = GetComponent<PowerBar>();
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
	 *	Moves and rotates the ghost fish, the distance measure, and the moving distance bar.
	 */
	public void PointJumpIndicators(int layerMask)
	{
		// if objects are hidden, make them visible
		if (!active)
		{
			StartIndicators();
		}

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
		{
			// GHOST FISH
			// move the ghost fish to where the real fish is
			fishKinematic.transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f, transform.position.z);

			// make the ghost fish look toward the point
			float lookY = fishKinematic.gameObject.transform.position.y;
			Vector3 lookAt = new Vector3(hit.point.x, lookY, hit.point.z);  // the ghost fish will look on its own y level
			fishKinematic.transform.LookAt(lookAt);

			// store its current rotation for the distance measure and bar
			float degrees = fishKinematic.gameObject.transform.rotation.eulerAngles.y;
			Quaternion flatRotation = Quaternion.Euler(0, degrees, 0);

			// finally, rotate the fish by 90 degrees so its head faces forward
			Quaternion rotate90 = Quaternion.Euler(0, 90, 0);
			fishKinematic.transform.rotation *= rotate90;


			// DISTANCE MEASURE
			distanceMeasure.transform.position = transform.position;
			distanceMeasure.transform.rotation = flatRotation;


			// DISTANCE BAR
			// must place bar at correct distance from player fish
			Vector3 fishPosition = transform.position;
			float radians = degrees * Mathf.Deg2Rad;
			float distance = powerBar.GetCurrentPower() * scale;

			float x = fishPosition.x + Mathf.Sin(radians) * distance;
			float y = fishPosition.y;
			float z = fishPosition.z + Mathf.Cos(radians) * distance;

			distanceBar.transform.position = new Vector3(x, y, z);
			distanceBar.transform.rotation = flatRotation;
		}
	}
}
