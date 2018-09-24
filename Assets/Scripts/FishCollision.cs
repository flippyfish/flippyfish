using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishCollision : MonoBehaviour
{

	private FishMovement fishMovement;		// other fish script
	public GameObject winScreen;

	Rigidbody rb;

	void Start()
	{
		fishMovement = GetComponent<FishMovement>();
		rb = GetComponent<Rigidbody>();
	}

	IEnumerator Respawn()
	{
		fishMovement.inControl = false;
		yield return new WaitForSeconds(1);

		rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
		fishMovement.transform.rotation = fishMovement.respawnRotation;
		fishMovement.transform.position = fishMovement.respawnPosition;
		fishMovement.prevRotation = transform.rotation;
		//fishMovement.prevPosition = transform.position;

		fishMovement.SetCharge(0);
		fishMovement.inControl = true;
	}
	
	// for solid objects the fish hits, eg. a car
	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Obstacle")
		{
			if (fishMovement.inControl)
			{
				fishMovement.inControl = false;
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
		if (other.tag == "Pond")		// update respawn
		{
			Vector3 pondPos = other.transform.position;
			fishMovement.respawnPosition = new Vector3(pondPos.x, pondPos.y + 0.5f, pondPos.z);
		}
		if (other.tag == "Goal")		// end of level
		{
			fishMovement.inControl = false;			// disable jumping after beating the level
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
		if (!fishMovement.isGrounded && rb.velocity.magnitude < 0.05)
		{
			rb.velocity = new Vector3(0, 0, 0);
			fishMovement.prevPosition = transform.position;
			fishMovement.prevRotation = transform.rotation;
			fishMovement.isGrounded = true;	// now we can charge another jump
		}
	}
}
