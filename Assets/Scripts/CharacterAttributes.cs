
using System;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    public float maxHealth = 100.0f;
    public float maxStamina = 100.0f;
    
    public float Health { get; private set; }
    public float Stamina { get; private set; }

    private void Awake()
    {
        Health = maxHealth;
        Stamina = maxStamina;
    }

    public void ApplyHealthChange(float delta)
    {
        Health += delta;
        if (Health < 0.0f)
            Health = 0.0f;
        else if (Health > maxHealth)
            Health = maxHealth;
    }
    
    public void ApplyStaminaChange(float delta)
    {
        Stamina += delta;
        if (Stamina < 0.0f)
            Stamina = 0.0f;
        else if (Stamina > maxStamina)
            Stamina = maxStamina;
    }
}