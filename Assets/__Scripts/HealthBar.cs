using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    /// <summary>
    /// Declaring varliables
    /// </summary>
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    /// <summary>
    /// Sets health bar value
    /// </summary>
    /// <param name="health"></param>
    public void SetHealth(int health)
    {
        //Sets slider value to health paramater
        slider.value = health;

        // Evaluates gradient at current health, normalizedValue converts health to a scale from 0 to 1
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    /// <summary>
    /// Sets max value for health bar
    /// </summary>
    /// <param name="health"></param>
    public void SetMaxHealth(int health)
    {
        // Sets slider's max value to the paramaterized health value
        slider.maxValue = health;
        // Sets the slider value to the health passed as well
        slider.value = health;

        // Evaluates gradient when bar is full (gradient is on a 0 to 1 scale)
        fill.color = gradient.Evaluate(1f);
    }
}
