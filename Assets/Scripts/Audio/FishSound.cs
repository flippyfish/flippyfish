using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSound : MonoBehaviour {

    public AudioClip fishSound;
    public AudioClip jumpSound;
    public AudioClip enterWaterSound;
    public AudioClip exitWaterSound;
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

    public void playEnterWaterSound() {
        soundSource.clip = enterWaterSound;
        soundSource.Play();
    }

    public void playExitWaterSound() {
        soundSource.clip = exitWaterSound;
        soundSource.Play();
    }
}
