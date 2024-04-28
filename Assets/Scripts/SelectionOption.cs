using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SelectionOption : MonoBehaviour
{
    public List<string> options;
    private string selected;
    [SerializeField] private TMP_Text text;
    private int index = 0;

    private void Start()
    {
        text.text = options[0];
    }

    public void OnLeftClicked()
    {
        index--;
        if (index < 0)
            index = 0;
        text.text = options[index];
    }

    
    public void OnRightClicked()
    {
        index++;
        if (index >= options.Count)
            index = options.Count - 1;
        text.text = options[index];
    }
    
}
