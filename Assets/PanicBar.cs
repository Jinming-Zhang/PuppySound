using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanicBar : MonoBehaviour
{

    public Slider slider;

    public void SetMaxHealth(int maxPanicLevel)
    {
        slider.maxValue = maxPanicLevel;
        slider.value = 0;
    }
    public void SetPanicLevel(int panicLevel)
    {
        slider.value = panicLevel;
    }
}
