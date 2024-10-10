using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class EnemyCharacter : Character
{
    [SerializeField] private AiState _aiState;
    [SerializeField] private float targetCheckingDistance = 0.8f;


    protected override Character TargetTransform =>
        GameManager.Instance.CharacterFactory.PlayerCharacter;

    public override void Initialize()
    {
        base.Initialize();
        AttackComponent = new EnemyHandedAttackComponent();
        AttackComponent.Initialize(characterData);
    }
    
    protected override void Update()
    {
        if (TargetTransform == null || !TargetTransform.gameObject.activeSelf)
            return;
        
        Vector3 direction = TargetTransform.transform.position - characterData.CharacterTransform.position;
        switch (_aiState)
        {
            case AiState.Idle:

                return;
            
            case AiState.MovementToTarget:
                direction = direction.normalized;
                MovementComponent.Move(direction);
                MovementComponent.Rotation(direction);
                if (Vector3.Distance(TargetTransform.transform.position, characterData.CharacterTransform.position) <=
                    targetCheckingDistance)
                    _aiState = AiState.Attack;
                return;
            
            case AiState.Attack:
                MovementComponent.Move(Vector3.zero);
                
                direction = direction.normalized;
                MovementComponent.Rotation(direction);
                
                AttackComponent.MakeAttack();
                
                if (Vector3.Distance(TargetTransform.transform.position, characterData.CharacterTransform.position) >
                    targetCheckingDistance)
                    _aiState = AiState.MovementToTarget;
                return;
        }
    }
}