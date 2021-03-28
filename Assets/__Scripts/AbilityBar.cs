using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBar : MonoBehaviour
{
    // Declaring variables
    public GameObject barGO;
    public Slider slider;
    public Image fill;

    // Method to set value of ability bar
    public void SetValue(int value)
    {
        slider.value = value;
    }

    // Shows bar on HUD
    public void ShowBar()
    {
        barGO.SetActive(true);
    }

    // Hides bar on HUD
    public void HideBar()
    {
        barGO.SetActive(false);
    }
}
