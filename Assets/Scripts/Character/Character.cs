using UnityEngine;

public abstract class Character : MonoBehaviour
{
	[SerializeField] protected CharacterType characterType;
	[SerializeField] protected CharacterData characterData;

	public CharacterType CharacterType => characterType;
	public CharacterData CharacterData => characterData;
	
	public IMovementComponent MovementComponent { get; protected set; }
	public IHealthComponent HealthComponent { get; protected set; }
	public IAttackComponent AttackComponent { get; protected set; }
	
	public abstract Character TargetTransform { get; }


	public virtual void Initialize()
	{
		MovementComponent = new CharacterControllerMovementComponent();
		MovementComponent.Initialize(this);

		HealthComponent = new CharacterHealthComponent();
		HealthComponent.Initialize(this);
	}
	
	protected abstract void Update();
}