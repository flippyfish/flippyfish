using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * The PowerBar class is responsbile for handling all functionality regarding the in-game power bar
 * which the user interacts with to control the power of a individual fish jump. This class has three core methods which are
 * mainly called from the central fish movement script:
 * - UpdateCharge()
 * - StartCharge()
 * - StopCharge()
**/
public class PowerBar : MonoBehaviour
{
    private float currentCharge;
    private bool isIncreasing;
    private float acceleration;
    private float MAX_CHARGE;
    private float MINIMUM_CHARGE;
    private bool charging;
    
    public Slider chargeSlider;
    public Image sliderBackground;

    public Color maxChargeColor;
    public Color minChargeColor;

    // Use this for initialization
    void Start()
    {
        currentCharge = 0.0f;
        isIncreasing = true;
        acceleration = 2f;
        MAX_CHARGE = 2f;
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

    /**
     * Starts the charging of the power bar by allowing the UpdateCharge() method to be called in the Update() method
    **/
    public void StartCharge()
    {
        charging = true;
    }

    /**
     * Stops the charging of the power bar by reseting all values
    **/
    public void StopCharge()
    {
        charging = false;
        chargeSlider.value = 0;
        currentCharge = 0;
    }

    /**
     * The update charge method is responsible for updating the state of the power bar.
     * The power bar has two states:
     * - Increasing, this is when the power bar is increasing via the acceleration variable
     * - Decreasing, this is when the power bar is decresing via the acceleration variable
    **/
    public void UpdateCharge()
    {
        if (isIncreasing)
        {
            float newCharge = currentCharge + (Time.deltaTime * acceleration);
            if (newCharge <= MAX_CHARGE)
            {
                currentCharge = newCharge;
                chargeSlider.value = currentCharge * 100;
                acceleration *=1.05f;

            }
            else//If the new charge is going to be greater than max then set currentCharge to max
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
                acceleration *= 0.95f;
                chargeSlider.value = currentCharge * 100;

            }
            else//If the new charge is going to be less than zero then set currentCharge to zero
            {
                currentCharge = MINIMUM_CHARGE;
                chargeSlider.value = 0;
                acceleration = 2f;
                isIncreasing = true;
            }

        }
        SetColor();
    }

    public void SetColor()
    {
        sliderBackground.color = Color.Lerp(minChargeColor, maxChargeColor, (currentCharge / MAX_CHARGE));
    }

    public float GetCurrentPower()
    {
        return currentCharge;
    }
}
