using UnityEngine;

public class CharacterControllerMovementComponent : IMovementComponent
{
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
    

    public void Initialize(CharacterData characterData)
    {
        _characterData = characterData;
    }
    
    public void Move(Vector3 direction)
    {
        // Если направление движения равно нулю (персонаж не должен двигаться), выходим из метода
        if (direction == Vector3.zero)
            return;

        // Рассчитываем угол поворота в градусах на основе направления движения
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // Создаем вектор движения в локальной системе координат с учетом поворота персонажа
        Vector3 move = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

        // Перемещаем персонажа с использованием CharacterController, умножая вектор движения на скорость и deltaTime
        _characterData.CharacterController.Move(move * Speed * Time.deltaTime);
    }

    public void Rotation(Vector3 direction)
    {
        // Если направление вращения равно нулю (персонаж не должен поворачиваться), выходим из метода
        if (direction == Vector3.zero)
            return;

        // Время плавного поворота (чем меньше, тем быстрее поворот)
        float turnSmoothTime = 0.1f;


        // Рассчитываем угол поворота в градусах на основе направления
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        // Плавно поворачиваем персонажа к целевому углу
        float angle = Mathf.SmoothDampAngle(_characterData.CharacterTransform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        // Присваиваем новый угол поворота персонажу
        _characterData.CharacterTransform.rotation = Quaternion.Euler(0, angle, 0);
    }
}