using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrackPlayer : MonoBehaviour {

	public GameObject player;
    private FishMovement fishMovement;		// other fish script

	public float yOffset;		// suggest 3
	public float zOffset;		// suggest -4
    public float zoomSpeed;		// suggest 1

    void Start ()
    {
    	fishMovement = player.GetComponent<FishMovement>();
    }

	void Update ()
	{
		// don't update camera position while fish is not moving
		// this eliminates shaking when charging a jump
		if (!fishMovement.isGrounded)
		{
	        yOffset -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
	        zOffset += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

			float x = player.transform.position.x;
			float y = player.transform.position.y + yOffset;
			float z = player.transform.position.z + zOffset;
			transform.position = new Vector3(x, y, z);
		}
	}
}
