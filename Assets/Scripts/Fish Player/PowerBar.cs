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

    /**
    * Rate at which the power bar increases, this is modified by the acceleration and deacceleration values
    */
    private float rate_of_change;
    private float acceleration;
    private float deacceleration;

    private float MAX_CHARGE;
    private float MINIMUM_CHARGE;
    private float STARTING_RATE;

    private bool isBarIncreasing;

    /**
     * Determinges whether the slider should be updated with every frame
     */
    private bool isCharging;

    public Slider chargeSlider;
    public Image sliderBackground;

    public Color maxChargeColor;
    public Color minChargeColor;

    // Use this for initialization
    void Start()
    {
        MAX_CHARGE = 2f;
        MINIMUM_CHARGE = 0.0f;
        STARTING_RATE = 2f;

        currentCharge = MINIMUM_CHARGE;
        rate_of_change = STARTING_RATE;
        acceleration = 1.05f;
        deacceleration = 0.95f;

        isBarIncreasing = true;
        isCharging = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (isCharging)
        {
            UpdateCharge();
        }
    }

    /**
     * Starts the isCharging of the power bar by allowing the UpdateCharge() method to be called in the Update() method
    **/
    public void StartCharge()
    {
        isCharging = true;
    }

    /**
     * Stops the isCharging of the power bar by reseting all values
    **/
    public void StopCharge()
    {
        isCharging = false;
        chargeSlider.value = 0;
        currentCharge = 0;
        rate_of_change = STARTING_RATE;
    }

    /**
     * The update charge method is responsible for updating the state of the power bar.
     * The power bar has two states:
     * - Increasing, this is when the power bar is increasing via the rate_of_change variable
     * - Decreasing, this is when the power bar is decresing via the rate_of_change variable
    **/
    private void UpdateCharge()
    {
        if (isBarIncreasing)
        {
            float newCharge = currentCharge + (Time.deltaTime * rate_of_change);
            if (newCharge <= MAX_CHARGE)
            {
                currentCharge = newCharge;
                chargeSlider.value = currentCharge * 100;
                rate_of_change *=acceleration;

            }
            else//If the new charge is going to be greater than max then set currentCharge to max
            {
                currentCharge = MAX_CHARGE;
                chargeSlider.value = currentCharge * 100;
                isBarIncreasing = false;
            }

        }
        else
        {
            float newCharge = currentCharge - (Time.deltaTime * rate_of_change);

            if (newCharge >= MINIMUM_CHARGE)
            {
                currentCharge = newCharge;
                rate_of_change *= deacceleration;
                chargeSlider.value = currentCharge * 100;

            }
            else//If the new charge is going to be less than zero then set currentCharge to zero
            {
                currentCharge = MINIMUM_CHARGE;
                chargeSlider.value = 0;
                rate_of_change = 2f;
                isBarIncreasing = true;
            }

        }
        SetColor();
    }

    private void SetColor()
    {
        sliderBackground.color = Color.Lerp(minChargeColor, maxChargeColor, (currentCharge / MAX_CHARGE));
    }

    public float GetCurrentPower()
    {
        return currentCharge;
    }
}
