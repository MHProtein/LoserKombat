using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public enum AIState
{
    IDLE,
    PATROLLING,
    CHASING,
    ATTACKING,
    DEFENDING,
    COMBAT,
    DEFAULT
}

public class Enemy : Character
{

    public float combatDistance = 5.0f;
    public float attackCD = 2.0f; //cool down
    public float minDefenseTime;
    public float maxDefenseTime;
    [Range(0, 1)]public float attackProbability = .1f;
    [Range(0, 1)]public float defendProbability = .1f;
    public AIState State { get; private set; }

    private bool isDestSet = false;
    private Vector3 destination;
    private NavMeshAgent agent;
    private float attackTimer;
    private bool attackTimerOn;
    private bool attackable = true;
    private float defenseTimer;
    private float defenseTime;
    private float turningAngle;
    private Vector3 offset;
    private Vector3 lastPos;
    private AIState nextState = AIState.DEFAULT;
    [SerializeField] private Player player;

    private void Awake()
    {
        State = AIState.IDLE;
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        agent.speed = maxSpeed;
        agent.stoppingDistance = 2.0f;
        agent.updatePosition = true;
        ChangeEnemyState(AIState.PATROLLING);
    }

    private void OnEnable()
    {
        player.OnPlayerStateChanged += OnPlayerStateChanged;
    }

    private void OnDisable()
    {
        player.OnPlayerStateChanged -= OnPlayerStateChanged;
    }

    private void OnPlayerStateChanged(CharacterState newState)
    {
        if (State == AIState.COMBAT)
        {
            if (newState == CharacterState.ATTACKING)
            {
                if (Random.value < defendProbability)
                {
                    Invoke("Defend", Random.Range(0.2f, 0.25f));
                }
            }
        }
    }

    void Defend()
    {
        ChangeEnemyState(AIState.DEFENDING);
    }
    
    void Update()
    {
        if (attackTimerOn)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackCD)
            {
                attackTimerOn = false;
                attackTimer = 0.0f;
                attackable = true;   
            }
        }

        switch (State)
        {
            case AIState.IDLE:
            {
                if (nextState != AIState.DEFAULT)
                {
                    Invoke("Attack", .3f);
                    nextState = AIState.DEFAULT;
                }
                break;
            }
            case AIState.PATROLLING:
            {
                if (agent.remainingDistance < .1f)
                {
                    SetNewPatrolDest();
                }

                break;
            }
            case AIState.CHASING:
            {
                agent.SetDestination(player.transform.position);
                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    if (nextState != AIState.DEFAULT)
                    {
                        ChangeEnemyState(AIState.IDLE);
                    }
                    else
                        ChangeEnemyState(AIState.COMBAT);
                }
                break;
            }
            case AIState.COMBAT:
            {
                offset = player.transform.position - transform.position;
                agent.SetDestination(player.transform.position);
                if (agent.remainingDistance > combatDistance)
                {
                    animator.SetBool(IsWalking, true);
                }
                else
                {
                    animator.SetBool(IsWalking, false);
 
                    turningAngle = Mathf.Atan2(offset.x, offset.z) * Mathf.Rad2Deg;
                    turningAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, turningAngle, 
                        ref turnSmoothVelocity, turnSmoothTime);
                    transform.rotation = Quaternion.Euler(0.0f, turningAngle, 0.0f);
                }
                if (attackable)
                {
                    if (Random.value < attackProbability)
                    {
                        nextState = AIState.ATTACKING;
                        ChangeEnemyState(AIState.CHASING);
                    }
                    else
                    {
                        attackable = false;
                        attackTimerOn = true;
                    }
                }
                break;
            }
            case AIState.DEFENDING:
            {
                defenseTimer += Time.deltaTime;
                if (defenseTimer > defenseTime)
                {
                    ChangeEnemyState(AIState.COMBAT);
                }
                break;
            }
        }
    }

    void Attack()
    {
        ChangeEnemyState(AIState.ATTACKING);
    }

    public void ChangeEnemyState(AIState newState)
    {
        if (State == newState)
            return;
        switch (State)
        {
            case AIState.PATROLLING:
            {
                animator.SetBool(IsWalking, false);
                break;
            }
            case AIState.CHASING:
            {
                animator.SetBool(IsWalking, false);
                animator.speed = 1.0f;
                break;
            }
            case AIState.COMBAT:
            {
                animator.SetBool(IsWalking, false);
                break;
            }
            case AIState.ATTACKING:
                animator.SetBool(IsAttack, false);
                break;
            case AIState.DEFENDING:
            {
                animator.SetBool(Defense, false);
                defenseTimer = 0.0f;
                break;
            }
        }

        switch (newState)
        {
            case AIState.PATROLLING:
            {
                characterState = CharacterState.WALKING;
                agent.stoppingDistance = .1f;
                InitialActionState();
                SetNewPatrolDest();
                agent.speed = maxSpeed;
                animator.SetBool(IsWalking, true);
                break;
            }
            case AIState.CHASING:
            {
                characterState = CharacterState.WALKING;
                if(nextState == AIState.ATTACKING)
                    agent.stoppingDistance = 1.5f;
                else
                    agent.stoppingDistance = combatDistance;
                InitialActionState();
                animator.speed = 2.0f;
                agent.SetDestination(player.transform.position);
                agent.speed = sprintMaxSpeed;
                animator.SetBool(IsWalking, true);
                break;
            }
            case AIState.COMBAT:
            {
                characterState = CharacterState.IDLE;
                agent.stoppingDistance = combatDistance;
                InitialActionState();
                animator.speed = 1.0f;
                agent.SetDestination(player.transform.position);
                agent.speed = maxSpeed;
                animator.SetBool(IsWalking, false);
                break;
            }
            case AIState.ATTACKING:
            {
                characterState = CharacterState.ATTACKING;
                animator.SetBool(IsAttack, true);
                agent.stoppingDistance = 1.5f;
                attackable = false;
                attackTimerOn = true;
                sword.Detect();
                break;
            }
            case AIState.DEFENDING:
            {
                characterState = CharacterState.DEFENDING;
                defenseTime = Random.Range(minDefenseTime, maxDefenseTime);
                animator.SetBool(Defense, true);
                break;
            }
        }

        State = newState;
    }

    void SetNewPatrolDest()
    {
        agent.SetDestination(
            new Vector3(Random.Range(-10.0f, 10.0f), transform.position.y, Random.Range(-10.0f, 10.0f)));
    }

    void InitialActionState()
    {
        if (State == AIState.ATTACKING)
        {
            animator.SetBool(IsAttack, false);
        }
        else if (State == AIState.DEFENDING)
        {
            animator.SetBool(Defense, false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeEnemyState(AIState.CHASING);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(State == AIState.PATROLLING)
            if (other.CompareTag("Player"))
            {
                ChangeEnemyState(AIState.CHASING);
            }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeEnemyState(AIState.PATROLLING);
        }
    }

}
