
using UnityEngine;

public class Character : MonoBehaviour
{
    public float maxSpeed = 1.0f;
    public float sprintMaxSpeed = 2.0f;
    public float turnSmoothTime = 0.1f;
    
    protected string IsWalking = "IsWalking";
    protected string IsAttack = "Attack";
    protected string Defense = "Defense";
    protected float turnSmoothVelocity;
    [SerializeField] protected CharacterAttributes attributes;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Sword sword;
    public CharacterState characterState { get; protected set; }

    public void ApplyHealthChange(float delta)
    {
        attributes.ApplyHealthChange(delta);
    }
    
}
