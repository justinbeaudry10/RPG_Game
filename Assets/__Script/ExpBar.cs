using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpBar : MonoBehaviour
{
    public Slider slider;
    public Image fill;

    public void setExp(int exp)
    {
        slider.value = exp;
    }

    public void setMaxExp(int exp)
    {
        slider.maxValue = exp;
        slider.value = 0;
    }



}
