using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Toggle : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text text;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.StopPlayback();
    }

    public void OnToggled()
    {
        if ((int)slider.value == 1)
        {
            text.text = "ON";
            animator.Play("TurnOn");
        }
        if ((int)slider.value == 0)
        {
            text.text = "OFF";
            animator.Play("TurnOff");
        }
    }
    
}
