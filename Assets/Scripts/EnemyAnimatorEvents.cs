using UnityEngine;

public class EnemyAnimatorEvents : MonoBehaviour
{ 
    private Enemy enemy;
    [SerializeField] private SwordTrail trail;
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }
    
    public void OnAttackFinish()
    {
        enemy.ChangeEnemyState(AIState.COMBAT);
    }
    
    public void OnSwordStrike()
    {
        trail.SetTrailEnabled();
    }
    
    public void OnSwordFinishStrike()
    {
        trail.SetTrailDisabled();
    }
}