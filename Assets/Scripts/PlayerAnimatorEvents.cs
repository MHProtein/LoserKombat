using UnityEngine;

public class PlayerAnimatorEvents : MonoBehaviour
{ 
    private Player player;
    [SerializeField] private SwordTrail trail;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }
    
    public void OnAttackFinish()
    {
        player.ChangePlayerState(CharacterState.IDLE);
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
