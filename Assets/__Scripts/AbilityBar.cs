using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityBar : MonoBehaviour
{
    public GameObject barGO;
    public Slider slider;
    public Image fill;

    public void SetValue(int value)
    {
        slider.value = value;
    }

    public void ShowBar()
    {
        barGO.SetActive(true);
    }

    public void HideBar()
    {
        barGO.SetActive(false);
    }
}
