using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrackPlayer : MonoBehaviour {

	public GameObject player;

	public float xOffset;		// suggest -4
	public float yOffset;		// suggest 3
	public float zOffset;		// suggest -4
	public float zoomSpeed;		// suggest 2
	public float zoomYMin;		// suggest 2
	public float zoomYMax;		// suggest 8

	void Update ()
	{
		float zoomCheck = yOffset - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
		if (zoomCheck < zoomYMin || zoomCheck > zoomYMax)
		{
			return;
		}

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
	}
}
