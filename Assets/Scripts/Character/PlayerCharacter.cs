using UnityEngine;

public class PlayerCharacter : Character
{
    public override void Initialize()
    {
        MovementComponent = new CharacterControllerMovementComponent();
        MovementComponent.Initialize(characterData);
    }

    public void Start()
    {
        Initialize();
    }

    protected  override void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(x, 0, z).normalized;
        
        MovementComponent.Move(moveDirection);
        MovementComponent.Rotation(moveDirection);
    }
}