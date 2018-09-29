using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	This script keeps the raycast planes at a position relative to the player fish.
	This makes the fish face the cursor consistently.
*/
public class RaycastPlanes : MonoBehaviour
{

	GameObject player;

	// Use this for initialization
	void Start ()
	{
		player = GameObject.Find("Fish_Player");
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.position = player.transform.position;
	}
}
