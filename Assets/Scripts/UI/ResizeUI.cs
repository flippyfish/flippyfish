using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResizeUI : MonoBehaviour {

    public float ratioWidth;
    public float ratioHeight;
    Text m_Text;
    RectTransform m_RectTransform;
    // Use this for initialization
    void Start () {
        //Fetch the Text and RectTransform components from the GameObject
        m_Text = GetComponent<Text>();
        //m_RectTransform = GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        int newFontSize = (int)(screenWidth /10);
        //screenWidth = screenWidth * ratioWidth;
        //screenHeight = screenHeight * ratioHeight;
        m_Text.fontSize = newFontSize;
        //m_RectTransform.sizeDelta = new Vector3(screenWidth, screenHeight, 0);
    }
}
