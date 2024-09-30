using UnityEngine;

public class EnemyHandedAttackComponent : IAttackComponent
{
    private const float ATTACK_RANGE = 0.8f;
    private const float ATTACK_DURATION_MAX = 1f;
    
    
    private CharacterData _characterData;
    private Character _playerCharacter;

    private float _attackDuration;

    public float Damage => _characterData.baseDamage;
    public float AttackRange => ATTACK_RANGE;


    public void Initialize(CharacterData characterData)
    {
        _characterData = characterData;
    }

    public void MakeAttack()
    {
        if (_playerCharacter == null || !_playerCharacter.HealthComponent.IsAlive)
            return;

        if (_attackDuration > 0)
        {
            _attackDuration -= Time.deltaTime;
            return;
        }

        float targetDistance = Vector3.Distance(
            _playerCharacter.MovementComponent.Position,
            _characterData.CharacterTransform.position);
        
        if (targetDistance > AttackRange)
            return;
        
        _playerCharacter.MakeDamage(Damage);
        _attackDuration = ATTACK_DURATION_MAX;
    }
}
