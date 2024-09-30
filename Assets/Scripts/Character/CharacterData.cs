using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [SerializeField] private Transform _characterTransform;
    [SerializeField] private CharacterController _characterController;
    public float speed;
    public int baseHealth;
    public float baseDamage;


    public Transform CharacterTransform => _characterTransform;
    public CharacterController CharacterController => _characterController;
}
