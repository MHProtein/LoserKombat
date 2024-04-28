using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderOption : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;
    

    private void Update()
    {
        text.text = slider.value.ToString();
    }
}
