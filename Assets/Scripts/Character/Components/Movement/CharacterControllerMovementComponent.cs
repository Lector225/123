using UnityEngine;

public class CharacterControllerMovementComponent : IMovementComponent
{
    private Character character;
    private CharacterData _characterData;
    // Переменная для хранения текущей скорости плавного поворота
    private float turnSmoothVelocity = 0.1f; 


    public float Speed
    { 
        get
        {
            return _characterData.speed;
        }
        set
        {
            if (value >= 0)
                _characterData.speed = value;
        } 
    }

    public Vector3 Position =>
        _characterData.CharacterTransform.position;
    
    
    public void Move(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        Vector3 move = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        _characterData.CharacterController.Move(move * Speed * Time.deltaTime);
    }

    public void Rotation(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        float turnSmoothTime = 0.1f;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(_characterData.CharacterTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        _characterData.CharacterTransform.rotation = Quaternion.Euler(0, angle, 0);
    }

    public void Initialize(Character character)
    {
        this.character = character;
        _characterData = character.CharacterData;
    }
}