using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrackPlayer : MonoBehaviour {

	public GameObject player;
    private FishMovement fishMovement;		// other fish script

	public float xOffset;		// suggest -4
	public float yOffset;		// suggest 3
	public float zOffset;		// suggest -4
    public float zoomSpeed;		// suggest 1

    private float previousRotationY;

    void Start ()
    {
    	fishMovement = player.GetComponent<FishMovement>();
    	previousRotationY = transform.rotation.eulerAngles.y;
    }

	void Update ()
	{
		// only update camera position if fish is moving, or if the camera has been rotated or zoomed
		// this eliminates shaking when charging a jump
		if (!fishMovement.isGrounded || !Mathf.Approximately(transform.rotation.eulerAngles.y, previousRotationY)
									 || !Mathf.Approximately(Input.GetAxis("Mouse ScrollWheel"), 0.0f))
		{
			xOffset += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
	        yOffset -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
	        zOffset += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

			float radians = transform.rotation.eulerAngles.y * Mathf.Deg2Rad;

			float x = player.transform.position.x;
			x += xOffset * Mathf.Sin(radians);
			float z = player.transform.position.z;
			z += zOffset * Mathf.Cos(radians);
			float y = player.transform.position.y;
			y += yOffset;

			transform.position = new Vector3(x, y, z);
			previousRotationY = transform.rotation.eulerAngles.y;
		}
	}
}
