using UnityEngine;

public class PlayerCharacter : Character
{
    public override Character Target
    {
        get
        {
            Character target = null;
            float nearest = float.MaxValue;
            var activePool = GameManager.Instance.CharacterFactory.ActivePool;
            foreach (var activeCharacter in activePool)
            {
                if (activeCharacter.CharacterType == CharacterType.DefaultPlayer)
                    continue;
                
                if (!activeCharacter.HealthComponent.IsAlive)
                    continue;
                
                float distance = Vector3.Distance(activeCharacter.transform.position, transform.position);
                if (distance < nearest)
                {
                    nearest = distance;
                    target = activeCharacter;
                }
            }

            return target;
        }
    }
    

    public override void Initialize()
    {
        base.Initialize();

        AttackComponent = new WeaponAttackComponent();
        AttackComponent.Initialize(this);
    }

    protected  override void Update()
    {
        if (!HealthComponent.IsAlive)
            return;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(x, 0, z).normalized;
        MovementComponent.Move(moveDirection);
        
        if (Target == null)
        {
            MovementComponent.Rotation(new Vector3(x, 0, z));
        }
        else
        {
            Vector3 directionToTarget = Target.transform.position - characterData.CharacterTransform.position;
            directionToTarget.Normalize();
            MovementComponent.Rotation(directionToTarget);
        
            AttackComponent.OnUpdate();
            AttackComponent.MakeAttack();
        }
    }
}