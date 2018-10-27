using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    private float currentCharge;
    private bool isIncreasing;
    private float acceleration;
    private float MAX_CHARGE;
    //private float MAX_ACCELERATION;
    private float MINIMUM_CHARGE;
    private bool charging;
    
    public Slider chargeSlider;
    public Image sliderBackground;

    // Use this for initialization
    void Start()
    {
        currentCharge = 0.0f;
        isIncreasing = true;
        acceleration = 2f;
        MAX_CHARGE = 2f;
        //MAX_ACCELERATION = 1.5f;
        MINIMUM_CHARGE = 0.0f;
        charging = false;
        //Debug.Log("In start");
        //sliderBackground = GameObject.FindWithTag("SliderImage").GetComponent<Image>();
        //chargeSlider = GameObject.FindWithTag("ChargeSlider").GetComponent<Slider>();

    }

    // Update is called once per frame
    void Update()
    {
        if (charging)
        {
            UpdateCharge();
        }
    }

    public void StartCharge()
    {
        charging = true;
    }

    public void StopCharge()
    {
        charging = false;
        chargeSlider.value = 0;
        currentCharge = 0;
    }

    public float GetCurrentPower()
    {
        return currentCharge;
    }

    public void UpdateCharge()
    {
        if (isIncreasing)
        {
            float newCharge = currentCharge + (Time.deltaTime * acceleration);
            if (newCharge <= MAX_CHARGE)
            {
                currentCharge = newCharge;
                chargeSlider.value = currentCharge * 100;
                acceleration *=1.05f;//1.1
                //sliderBackground.color = new Color(255,0,0);
                //sliderBackground.fillAmount = 0.2f;

            }
            else//If new charge is going to be greater than max then set currentCharge to max
            {
                currentCharge = MAX_CHARGE;
                chargeSlider.value = currentCharge * 100;
                isIncreasing = false;
            }

        }
        else
        {
            float newCharge = currentCharge - (Time.deltaTime * acceleration);

            if (newCharge >= MINIMUM_CHARGE)
            {
                currentCharge = newCharge;
                acceleration *= 0.95f;//0.9
                chargeSlider.value = currentCharge * 100;
                //sliderBackground.color = new Color(252, 146, 0, 255);

            }
            else//If new charge is going to be less than zero then set currentCharge to zero
            {
                currentCharge = MINIMUM_CHARGE;
                chargeSlider.value = 0;
                acceleration = 2f;
                isIncreasing = true;
            }

        }
    }
}
