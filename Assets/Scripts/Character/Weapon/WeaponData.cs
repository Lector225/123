using UnityEngine;
using UnityEngine.Serialization;

public abstract class WeaponData : ScriptableObject
{
    [SerializeField]
    private float weaponDamage;
    [SerializeField]
    private float timeBetweenAttack;
    [FormerlySerializedAs("attackDistance")] [SerializeField]
    private float attackRange;


    public float WeaponDamage => weaponDamage;
    public float TimeBetweenAttack => timeBetweenAttack;
    public float AttackRange => attackRange;
}
