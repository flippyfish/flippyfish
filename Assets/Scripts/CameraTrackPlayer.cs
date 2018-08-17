using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrackPlayer : MonoBehaviour {

	public GameObject player;
	public float yOffset;		// suggest 3
	public float zOffset;		// suggest -4
    public float zoomSpeed;		// suggest 1

	// Update is called once per frame
	void Update ()
	{
        yOffset -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        zOffset += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

		float x = player.transform.position.x;
		float y = player.transform.position.y + yOffset;
		float z = player.transform.position.z + zOffset;
		transform.position = new Vector3(x, y, z);
	}
}
