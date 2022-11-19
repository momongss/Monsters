using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider slider;

    public Text hpText;

    public void SetMaxHP(float hp)
    {
        slider.maxValue = hp;
        slider.value = hp;

        hpText.text = $"{(int)hp}";
    }

    public float BackBarDelay = 0.2f;

    public void SetHP(float hp)
    {
        slider.value = hp;

        hpText.text = $"{(int)hp}";
    }
}