using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class EnemyCharacter : Character
{
    [SerializeField] private AiState _aiState;
    [SerializeField] private float targetCheckingDistance = 0.8f;


    public override Character Target =>
        GameManager.Instance.CharacterFactory.PlayerCharacter;

    public override void Initialize()
    {
        base.Initialize();
        AttackComponent = new EnemyHandedAttackComponent();
        AttackComponent.Initialize(this);
    }
    
    protected override void Update()
    {
        if (!HealthComponent.IsAlive)
            return;
        
        AttackComponent.OnUpdate();
        
        if (Target == null || !Target.gameObject.activeSelf)
            return;
        
        Vector3 direction = Target.transform.position - characterData.CharacterTransform.position;
        switch (_aiState)
        {
            case AiState.Idle:

                return;
            
            case AiState.MovementToTarget:
                direction = direction.normalized;
                MovementComponent.Move(direction);
                MovementComponent.Rotation(direction);
                if (Vector3.Distance(Target.transform.position, characterData.CharacterTransform.position) <=
                    targetCheckingDistance)
                    _aiState = AiState.Attack;
                return;
            
            case AiState.Attack:
                MovementComponent.Move(Vector3.zero);
                
                direction = direction.normalized;
                MovementComponent.Rotation(direction);
                
                AttackComponent.MakeAttack();
                
                if (Vector3.Distance(Target.transform.position, characterData.CharacterTransform.position) >
                    targetCheckingDistance)
                    _aiState = AiState.MovementToTarget;
                return;
        }
    }
}