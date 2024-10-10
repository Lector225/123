using UnityEngine;

public class PlayerCharacter : Character
{
    protected override Character TargetTransform
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
        MovementComponent = new CharacterControllerMovementComponent();
        MovementComponent.Initialize(characterData);
    }

    protected  override void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(x, 0, z).normalized;
        MovementComponent.Move(moveDirection);

        var target = TargetTransform;
        if (target == null)
            return;
        Vector3 directionToTarget = target.transform.position - characterData.CharacterTransform.position;
        directionToTarget.Normalize();
        MovementComponent.Rotation(directionToTarget);
    }
}