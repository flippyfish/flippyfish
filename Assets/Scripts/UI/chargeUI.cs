using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chargeUI : MonoBehaviour {
    /*
     * Need charge bar to be next to fish
     * 
     */
    private GameObject chargeBar;
    private FishMovement fishMovement;  // fish script

    // Use this for initialization
    void Start () {
		chargeBar = GameObject.Find("ChargeSlider");
        chargeBar.SetActive(false);
        fishMovement = GameObject.Find("Fish_Player").GetComponent<FishMovement>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButton(0) && fishMovement.isGrounded && fishMovement.inControl && !fishMovement.canceledClick) {
            chargeBar.SetActive(true);
        } else {
            chargeBar.SetActive(false);
        }
            float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float newWidth, newHeight, newX, newY;
        if (screenHeight < screenWidth)
        {
            newWidth = screenWidth / 15;
            newHeight = screenHeight / 5;
            newX = screenWidth / 2 - screenWidth / 6;
            newY = screenHeight / 2 - screenHeight / 5;
        }
        else {
            newWidth = screenWidth / 5;
            newHeight = screenHeight / 10;
            newX = screenWidth / 2 - (screenWidth / 3 + screenWidth / 8);
            newY = screenHeight / 2 - screenHeight / 8;
        }
        if (newWidth > 80) { newWidth = 80.0f; }
        else if (newWidth < 61) { newWidth = 60.0f; }
        if (newHeight > 160) { newHeight = 160.0f; }
        else if (newHeight < 121) { newHeight = 120.0f; }
        chargeBar.GetComponent<RectTransform>().sizeDelta = new Vector2(newWidth, newHeight);
        chargeBar.transform.position = new Vector3(newX, newY, 0.0f);
    }
}
