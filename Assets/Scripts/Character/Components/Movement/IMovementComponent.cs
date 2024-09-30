using UnityEngine;

public interface IMovementComponent
{
    float Speed { get; set; }
    
    Vector3 Position { get; }


    void Initialize(CharacterData characterData);
	
    void Move(Vector3 direction);
	
    void Rotation(Vector3 direction);
}