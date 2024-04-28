using System;
using UnityEngine;

public enum CharacterState
{
    IDLE,
    WALKING,
    ATTACKING,
    DEFENDING
}

[RequireComponent(typeof(Rigidbody), typeof(CharacterController))]
public class Player : Character
{ 
    public Transform camera;
    
    private float speed;
    private Rigidbody rigidbody;
    private Vector3 velocity;
    private CharacterController controller;
    private Vector3 moveDir;
    private bool defendAfterAttack = false;

    public delegate void onPlayerStateChanged(CharacterState newState);
    public event onPlayerStateChanged OnPlayerStateChanged;
    
    private void Awake()
    {
        speed = maxSpeed;
        rigidbody = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        moveDir.y = -100.0f;
    }

    private void Start()
    {
        ChangePlayerState(CharacterState.WALKING);
    }

    private void Movement()
    {
        if (characterState != CharacterState.IDLE && characterState != CharacterState.WALKING)
            return;
        var input = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical")).normalized;
        if (input.sqrMagnitude > 0.1f)
        {
            ChangePlayerState(CharacterState.WALKING);
            var angle = Mathf.Atan2(input.x, input.z) * Mathf.Rad2Deg + camera.eulerAngles.y;
            angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, 
                ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

            moveDir = Quaternion.Euler(0.0f, angle, 0.0f) * Vector3.forward;
            moveDir.y += rigidbody.velocity.y;
            controller.Move(speed * Time.deltaTime * moveDir.normalized);
        }
        else
        {
            ChangePlayerState(CharacterState.IDLE);
        }
    }

    void Update()
    {
        Movement();

        if (characterState == CharacterState.IDLE || characterState == CharacterState.WALKING)
        {
            Attack();
            Defend();
        }

        if (characterState == CharacterState.WALKING)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (animator.speed < 2f)
                {
                    animator.speed = 2f;
                    speed = sprintMaxSpeed;
                }
            }
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                animator.speed = 1.0f;
                speed = maxSpeed;
            }
        }

        if (characterState == CharacterState.DEFENDING)
        {
            if(Input.GetMouseButtonUp(1))
                ChangePlayerState(CharacterState.IDLE);
        }

        if (characterState == CharacterState.ATTACKING)
        {
            if(Input.GetMouseButtonDown(1))
                defendAfterAttack = true;
        }

    }
    
    public void ChangePlayerState(CharacterState newState)
    {
        if (characterState == newState)
            return;

        switch (characterState)
        {
            case CharacterState.IDLE:
                break;
            case CharacterState.WALKING:
            {
                animator.speed = 1.0f;
                speed = maxSpeed;
                break;
            }
            case CharacterState.ATTACKING:
            {
                if (defendAfterAttack)
                {
                    InitialActionState();
                    newState = CharacterState.DEFENDING;
                    defendAfterAttack = false;
                }
                break;
            }
            case CharacterState.DEFENDING:
                break;
        }
  
        switch (newState)
        {
            case CharacterState.IDLE:
            {
                InitialActionState();
                animator.SetBool(IsWalking, false);
                break;
            }
            case CharacterState.WALKING:
            {
                InitialActionState();
                animator.SetBool(IsWalking, true);
                break;
            }
            case CharacterState.ATTACKING:
            {
                animator.SetBool(IsAttack, true);
                break;
            }
            case CharacterState.DEFENDING:
            {
                animator.SetBool(Defense, true);
                break;
            }
        }
        characterState = newState;
        OnPlayerStateChanged?.Invoke(newState);
    }

    void InitialActionState()
    {
        if (characterState == CharacterState.ATTACKING)
        {
            animator.SetBool(IsAttack, false);
        }
        else if (characterState == CharacterState.DEFENDING)
        {
            animator.SetBool(Defense, false);
        }
    }

    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangePlayerState(CharacterState.ATTACKING);
            sword.Detect();
        }
    }

    void Defend()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ChangePlayerState(CharacterState.DEFENDING);
        }
    }
    
    
    
}
