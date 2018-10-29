using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSound : MonoBehaviour {

    public AudioClip fishSound;
    public AudioClip jumpSound;
    // don't touch. It should stay in public.
    public AudioSource soundSource;

	// Use this for initialization
	void Start () {
		soundSource = GetComponent<AudioSource>();
        soundSource.clip = fishSound;
	}

    public void playFishSound() {
        soundSource.clip = fishSound;
        soundSource.Play();
    }

    public void playJumpSound() {
        soundSource.clip = jumpSound;
        soundSource.Play();
    }

	// Update is called once per frame
	void Update () {
	}
}
