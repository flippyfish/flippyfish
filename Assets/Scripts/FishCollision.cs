using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 *	This script handles interactions between the player fish and collider/trigger objects.
 *	Because it detects collisions with obstacles, it also contains the respawn script.

 *	The FishOxygen script relies on this script for pond and water detection.
 */
public class FishCollision : MonoBehaviour
{

	private FishMovement fishMovement;		// other fish script
	private FishOxygen   fishOxygen;		// other fish script
	public GameObject winScreen;

	Rigidbody rb;

	private bool respawning;		// to avoid simultaneous respawn calls
	public float respawnTime;

	void Start()
	{
		fishMovement = GetComponent<FishMovement>();
		fishOxygen   = GetComponent<FishOxygen>();
		rb = GetComponent<Rigidbody>();
		respawning = false;
	}

	public bool isRespawning()
	{
		return respawning;
	}

	/**
	 *	For the respawn duration, set the fish's position to the given seagull's position.
	 *	When using this, call at the same time as Respawn().
	 */
	public IEnumerator TakenBySeagull(GameObject seagull)
	{
		float elapsedTime = 0;
		rb.isKinematic = true;
		rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
		while (elapsedTime < respawnTime)
		{
			transform.position = seagull.transform.position;
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		rb.isKinematic = false;
	}

	/**
	 *	Disables charging and releasing jumps for the respawn duration.
	 *	Respawns the fish after the duration has passed.
	 */
	public IEnumerator Respawn()
	{
		if (!respawning)
		{
			respawning = true;
			fishMovement.inControl = false;
			yield return new WaitForSeconds(respawnTime);

			rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
			transform.rotation = fishMovement.respawnRotation;
			transform.position = fishMovement.respawnPosition;

			fishMovement.ResetSliderAndFish();
			fishMovement.inControl = true;
			fishMovement.isGrounded = false;

			fishOxygen.SetOxygen(fishOxygen.OXYGEN_MAX);
			fishOxygen.timeLastInWater = Time.time;

			respawning = false;
		}
	}
	
	// for solid objects the fish hits, eg. a car
	void OnCollisionEnter(Collision collision)
	{
		if (!fishOxygen.fishInWater())
		{
        	GetComponent<FishSound>().playFishSound();
        }
        if (collision.gameObject.tag == "Obstacle")
		{
            if (fishMovement.inControl)
			{
				fishMovement.isGrounded = false;
				StartCoroutine(Respawn());
			}
		}
	}

	// for objects the fish can move through, eg. a pond
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Obstacle")	// hit an obstacle, respawn
		{
			if (fishMovement.inControl)
			{
				StartCoroutine(Respawn());
			}
		}
		if (other.tag == "Seagull")		// respawn, but the seagull picks up the fish!!
		{
			if (fishMovement.inControl)
			{
				StartCoroutine(TakenBySeagull(other.gameObject));
				StartCoroutine(Respawn());
			}
		}
		if (other.tag == "Water")		// update oxygen, but NOT respawn point
		{
			GetComponent<FishSound>().playSplashSound();
			fishOxygen.EnterWater();
		}
		if (other.tag == "Pond")		// update oxygen and respawn point
		{
			GetComponent<FishSound>().playSplashSound();
			fishOxygen.EnterWater();

			// respawn point
			Vector3 pondPos = other.transform.position;
			float yOffset = 2.0f;	// keep the fish out of any colliders beneath it
			fishMovement.respawnPosition = new Vector3(pondPos.x, pondPos.y + yOffset, pondPos.z);
		}
		if (other.tag == "Goal")		// end of level
		{
			fishMovement.inControl = false;		// disable jumping after beating the level
			fishOxygen.EnterWater();
			winScreen.SetActive(true);
			WinScreen winScreenScript = GameObject.Find("New Canvas").GetComponent<WinScreen>();
			winScreenScript.ShowTotalJump();
			winScreenScript.saveScore();
			winScreenScript.loadScore();
		}
	}

	// for objects the fish can move through, eg. a pond
	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Water")
		{
			fishOxygen.ExitWater();
		}
		if (other.tag == "Pond")
		{
			fishOxygen.ExitWater();
		}
		if (other.tag == "Boundary")
		{
			StartCoroutine(Respawn());
		}
	}

	/**
	 *	Detect when the fish has landed and come to a near-stop, so we can charge another jump.
	 */
	void OnCollisionStay()
	{
		float threshold = 1.5f;	// check if magnitude of fish velocity is less than this value
        if (fishMovement.inControl && !fishMovement.isGrounded && rb.velocity.magnitude < threshold)
		{
            fishMovement.isGrounded = true;
        }
	}
}
