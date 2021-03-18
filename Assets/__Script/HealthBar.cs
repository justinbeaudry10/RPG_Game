using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetHealth(int health)
    {
        slider.value = health;

        // Evaluates gradient at current health, normalizedValue converts health to a scale from 0 to 1
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        // Evaluates gradient when bar is full (gradient is on a 0 to 1 scale)
        fill.color = gradient.Evaluate(1f);
    }
}
