
using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Sword : MonoBehaviour
{
    private bool detect = false;
    [SerializeField] private Character target;
    private void OnTriggerEnter(Collider other)
    {
        if (!detect)
            return;
        if (target.characterState == CharacterState.DEFENDING)
        {
            if (other.CompareTag("Shield"))
            {
                detect = false;
                return;
            }
        }
        detect = false;
    }

    public void Detect()
    {
        detect = true;
    }
    
}