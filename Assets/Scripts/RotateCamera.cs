using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
	// adapted from https://answers.unity.com/questions/1189946/click-and-drag-to-rotate-camera-like-a-pan.html

	public float mouseSpeed;	// no need for Time.deltaTime
	public float buttonSpeed;	// will multiply by Time.deltaTime
	// if pulling from Input.GetAxis, Time.deltaTime is unnecessary

	void Update()
	{
		// arrow keys, WASD keys
		transform.Rotate(new Vector3(Input.GetAxis("Vertical") * mouseSpeed, Input.GetAxis("Horizontal") * mouseSpeed, 0));
		// keep x and y rotated, but fix z at 0
		float X = transform.rotation.eulerAngles.x;
		float Y = transform.rotation.eulerAngles.y;
		transform.rotation = Quaternion.Euler(X, Y, 0);

		// drag camera with right-click
		if (Input.GetMouseButton(1)) 
		{
			transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * mouseSpeed, -Input.GetAxis("Mouse X") * mouseSpeed, 0));
			// keep x and y rotated, but fix z at 0
			float X2 = transform.rotation.eulerAngles.x;
			float Y2 = transform.rotation.eulerAngles.y;
			transform.rotation = Quaternion.Euler(X2, Y2, 0);
		}

		// old code
		/*if (Input.GetKey(KeyCode.A))
		{
			transform.RotateAround(transform.position, -Vector3.up, buttonSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.D))
		{
			transform.RotateAround(transform.position, -Vector3.up, -buttonSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.W))
		{
			transform.RotateAround(transform.position, Vector3.left, buttonSpeed * Time.deltaTime);
		}
		if (Input.GetKey(KeyCode.S))
		{
			transform.RotateAround(transform.position, Vector3.right, buttonSpeed * Time.deltaTime);
		}*/
     }
}
