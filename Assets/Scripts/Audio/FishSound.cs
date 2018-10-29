﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSound : MonoBehaviour {

    public AudioClip fishSound;
    public AudioSource soundSource;

	// Use this for initialization
	void Start () {
        soundSource.clip = fishSound;
	}

    public void playFishSound() {
        soundSource.Play();
    }

	// Update is called once per frame
	void Update () {
	}
}
