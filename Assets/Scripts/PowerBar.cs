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
    private float MAX_ACCELERATION;
    private float MINIMUM_CHARGE;
    private bool charging;
    private Slider chargeSlider;

    // Use this for initialization
    void Start()
    {
        currentCharge = 0.0f;
        isIncreasing = true;
        acceleration = 2f;
        MAX_CHARGE = 2f;
        MAX_ACCELERATION = 1.5f;
        MINIMUM_CHARGE = 0.0f;
        charging = false;
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
        chargeSlider = GameObject.FindWithTag("ChargeSlider").GetComponent<Slider>();
        if (isIncreasing)
        {
            float newCharge = currentCharge + (Time.deltaTime * acceleration);

            if (newCharge <= MAX_CHARGE)
            {
                currentCharge = newCharge;
                chargeSlider.value = currentCharge * 100;
            }
            else
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
                chargeSlider.value = currentCharge * 100;
            }
            else
            {
                currentCharge = MINIMUM_CHARGE;
                chargeSlider.value = 0;
                isIncreasing = true;
            }

        }
    }
}
