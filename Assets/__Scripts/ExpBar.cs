using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    // Sets current value for experience
    public void setExp(int exp)
    {
        // Sets the slider value to the paramaterized experience
        slider.value = exp;
    }

    // Sets value for max experience
    public void setMaxExp(int exp)
    {
        // Sets max value of experience to the paramaterized experience
        slider.maxValue = exp;
        // Sets slider value to zero
        slider.value = 0;
    }



}
