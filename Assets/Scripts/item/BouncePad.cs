﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour {
    public float charge;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name == "Fish_Player")
        {
            Debug.Log("Bounce Pad, Fish on me!");
            Vector3 fishPos = GameObject.Find("Fish_Player").transform.position;
            Vector3 pondPos = GameObject.Find("Pond").transform.position;
            Vector3 newDirection = pondPos - fishPos.normalized;
            Quaternion newRotation = Quaternion.LookRotation(newDirection, Vector3.up);
            GameObject.Find("Fish_Player").transform.rotation = newRotation;
            GameObject.Find("Fish_Player").GetComponent<FishMovement>().doJump(charge);
        }
    }
}
