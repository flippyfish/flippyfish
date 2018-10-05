using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishCollision : MonoBehaviour
{

	private FishMovement fishMovement;		// other fish script
	private FishOxygen   fishOxygen;		// other fish script
	public GameObject winScreen;

	Rigidbody rb;

	public bool respawning;		// to avoid simultaneous respawn calls

	void Start()
	{
		fishMovement = GetComponent<FishMovement>();
		fishOxygen   = GetComponent<FishOxygen>();
		rb = GetComponent<Rigidbody>();
		respawning = false;
	}

	public IEnumerator Respawn()
	{
		respawning = true;
		fishMovement.inControl = false;
		yield return new WaitForSeconds(1);

		rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
		transform.rotation = fishMovement.respawnRotation;
		transform.position = fishMovement.respawnPosition;

		fishMovement.SetCharge(0);
		fishMovement.inControl = true;
		fishMovement.isGrounded = false;

		fishOxygen.SetOxygen(fishOxygen.OXYGEN_MAX);
		fishOxygen.timeLastInWater = Time.time;

		respawning = false;
	}
	
	// for solid objects the fish hits, eg. a car
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Obstacle")
		{
			if (fishMovement.inControl)
			{
				fishMovement.inControl = false;
				fishMovement.isGrounded = false;
				StartCoroutine(Respawn());
			}
		}
	}

	// for objects the fish can move through, eg. a pond
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Obstacle")	// hit an obstacle, respawn at start
		{
			if (fishMovement.inControl)
			{
				fishMovement.inControl = false;
				StartCoroutine(Respawn());
			}
		}
		if (other.tag == "Pond")		// update oxygen, respawn point
		{
			// oxygen
			fishOxygen.EnterWater();

			// respawn point
			Vector3 pondPos = other.transform.position;
			fishMovement.respawnPosition = new Vector3(pondPos.x, pondPos.y + 0.5f, pondPos.z);
		}
		if (other.tag == "Goal")		// end of level
		{
			fishMovement.inControl = false;		// disable jumping after beating the level
			fishOxygen.EnterWater();
			winScreen.SetActive(true);
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Pond")
		{
			fishOxygen.ExitWater();
		}
		if (other.tag == "Boundary")
		{
			StartCoroutine(Respawn());
		}
	}

	// detect when the fish has landed and come to a stop; record its state
	void OnCollisionStay()
	{
		if (fishMovement.inControl && !fishMovement.isGrounded && rb.velocity.magnitude < 0.05)
		{
			// rb.velocity = new Vector3(0, 0, 0);
			fishMovement.isGrounded = true;	// now we can charge another jump
		}
	}
}
