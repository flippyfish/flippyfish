using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This seagull script places a target shadow at the fish's exact location.
 */
public class Seagull : MonoBehaviour
{

	private GameObject fish;
	private FishOxygen fishOxygen;		// fish script -- use to check if fish is in a pond "safe zone"
	private GameObject shadow;
	public GameObject shadowPrefab;

	public float shadowYOffset;			// because ground level is relative to the current level
	public float initialDelay;
	public float loopDelay;

	// Use this for initialization
	void Start ()
	{
		fish = GameObject.Find("Fish_Player");
		fishOxygen = fish.gameObject.GetComponent<FishOxygen>();
		transform.position = CreateAirPosition();
		StartCoroutine(SeagullLoop());
	}

	IEnumerator SeagullLoop()
	{
		yield return new WaitForSeconds(initialDelay);	// if there is a pond at start, let the fish enter it first
		while (true)
		{
			yield return new WaitForSeconds(loopDelay);
			if (!isFishSafe())	// don't swoop onto the fish if it's in a pond
			{
				float fishX = fish.transform.position.x;
				float fishZ = fish.transform.position.z;
				Vector3 shadowPosition = new Vector3(fishX, shadowYOffset, fishZ);
				shadow = Instantiate(shadowPrefab, shadowPosition, Quaternion.identity);
				yield return new WaitForSeconds(1.0f);

				// swoop onto the fish's location
				transform.LookAt(shadowPosition);
				StartCoroutine(MoveOverSeconds(shadowPosition, 1.5f));
				yield return new WaitForSeconds(1.5f);

				// fly back into the air
				Destroy(shadow);
				Vector3 nextAirPosition = CreateAirPosition();
				transform.LookAt(nextAirPosition);
				StartCoroutine(MoveOverSeconds(nextAirPosition, 1.5f));
				yield return new WaitForSeconds(1.5f);
			}
			else
			{
				yield return new WaitForSeconds(4.0f);	// equivalent to time of above section
			}
		}
	}

	// https://answers.unity.com/questions/296347/move-transform-to-target-in-x-seconds.html
	public IEnumerator MoveOverSeconds (Vector3 target, float seconds)
	{
		float elapsedTime = 0;
		Vector3 startPosition = transform.position;
		while (elapsedTime < seconds)
		{
			transform.position = Vector3.Lerp(startPosition, target, (elapsedTime / seconds));
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		transform.position = target;
	}

	bool isFishSafe ()
	{
		return fishOxygen.fishInWater();
	}

	Vector3 CreateAirPosition()
	{
		Vector3 pos = fish.transform.position;
		pos += new Vector3(Random.Range(-10.0f, 10.0f),
						   Random.Range(10.0f, 15.0f),
						   Random.Range(-10.0f, 10.0f));
		return pos;
	}
}
