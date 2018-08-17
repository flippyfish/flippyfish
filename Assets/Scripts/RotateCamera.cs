using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
	public float speed;			// suggest 1
	public float xTilt;			// suggest 20 or 30

	// if from Input.GetAxis, Time.deltaTime is unnecessary

	void Update()
	{
		// arrow keys, WASD keys
		transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * speed, 0));

		// keep y rotated, but fix x and z
		// Unity euler angles are from 0 to 360, rather than -180 to 180
		float Y = transform.rotation.eulerAngles.y;
		if (Y > 180 && Y < 330)
			Y = 330;
		else if (Y < 180 && Y > 30)
			Y = 30;
		transform.rotation = Quaternion.Euler(xTilt, Y, 0);

		// drag camera with right-click
		if (Input.GetMouseButton(1)) 
		{
			transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * speed, 0));
			// keep y rotated, but fix x and z
			float Y2 = transform.rotation.eulerAngles.y;
			transform.rotation = Quaternion.Euler(xTilt, Y2, 0);
		}
     }
}
