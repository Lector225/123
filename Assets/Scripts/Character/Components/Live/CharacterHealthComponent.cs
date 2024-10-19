using System;
using UnityEngine;

public class CharacterHealthComponent : IHealthComponent
{
    private Character character;
    private float _health;
    private int _healthMax;


    public event Action<Character> OnCharacterHealthChange;
    public event Action<Character> OnCharacterDeath;

    
    public float Health
    {
        get => _health;
        set
        {
            _health = Mathf.Clamp(value, 0, _healthMax);
            OnCharacterHealthChange?.Invoke(character);

            if (_health > 0)
                return;
            
            _health = 0;
            OnCharacterDeath?.Invoke(character);
            Debug.LogError("Character death");
        }
    }

    public int HealthMax =>
        _healthMax;
    
    public bool IsAlive =>
        _health > 0;
    

    public void Initialize(Character character)
    {
        this.character = character;
        _healthMax = character.CharacterData.baseHealth;
        _health = character.CharacterData.baseHealth;
    }
}
