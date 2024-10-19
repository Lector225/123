using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class WeaponAttackComponent : IAttackComponent
{
    private Character character;
    private WeaponData currentWeaponData;
    private float timeBetweenAttack;


    public float Damage => currentWeaponData.WeaponDamage;
    public float AttackRange => currentWeaponData.AttackRange;
    

    public void MakeAttack()
    {
        if (timeBetweenAttack > 0
            || character.TargetTransform == null)
            return;

        float distance = Vector3.Distance(
            character.CharacterData.CharacterTransform.position,
            character.TargetTransform.CharacterData.CharacterTransform.position);

        if (distance > currentWeaponData.AttackRange)
            return;

        character.TargetTransform.HealthComponent.Health -= Damage;
        timeBetweenAttack = currentWeaponData.TimeBetweenAttack;
    }

    public void OnUpdate()
    {
        if (timeBetweenAttack > 0)
        {
            timeBetweenAttack -= Time.deltaTime;
        }
    }

    public void Initialize(Character character)
    {
        this.character = character;
        currentWeaponData = character.CharacterData.StarterWeaponData;
        timeBetweenAttack = 0;
    }
}
