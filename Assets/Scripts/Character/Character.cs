using System;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
	public event Action<Character> OnCharacterDeath;

	[SerializeField] protected CharacterType characterType;
	[SerializeField] protected CharacterData characterData;

	public CharacterType CharacterType => characterType;
	public IMovementComponent MovementComponent { get; protected set; }
	public IHealthComponent HealthComponent { get; protected set; }
	public IAttackComponent AttackComponent { get; protected set; }
	
	protected abstract Character TargetTransform { get; }


	public virtual void Initialize()
	{
		MovementComponent = new CharacterControllerMovementComponent();
		MovementComponent.Initialize(characterData);

		HealthComponent = new CharacterHealthComponent();
		HealthComponent.Initialize(characterData.baseHealth);
	}
	
	public virtual void MakeDamage(float damage)
	{
		HealthComponent.Health -= damage;
		if (HealthComponent.Health <= 0)
		{
			OnCharacterDeath?.Invoke(this);
		}
	}
	
	protected abstract void Update();
}