using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanicBar : MonoBehaviour
{

    public Slider slider;
    [SerializeField]
    Image sliderImage;
    [SerializeField]
    Color healthColor;
    [SerializeField]
    Color scaredColor;
    [SerializeField]
    Color dangerColor;

    private int threshold1 = 0;
    private int threshold2 = 0;

    public void SetMaxHealth(int maxPanicLevel, int threshold1, int threshold2)
    {
        slider.maxValue = maxPanicLevel;
        slider.value = 0;

        this.threshold1 = threshold1;
        this.threshold2 = threshold2;
    }

    public void SetPanicLevel(int panicLevel)
    {
        slider.value = panicLevel;
        if (panicLevel >= 0 && panicLevel < threshold1)
        {
            sliderImage.color = healthColor;
        } else if (panicLevel < threshold2)
        {
            sliderImage.color = scaredColor;
        } else if(panicLevel >= threshold2)
        {
            sliderImage.color = dangerColor;
        }
    }
}
