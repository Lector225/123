using UnityEngine;

public class CharacterHealthComponent : IHealthComponent
{
    private float _health;
    private int _healthMax;


    public float Health
    {
        get => _health;
        set
        {
            if (_health <= 0)
                return;
            
            _health = Mathf.Clamp(value, 0, _healthMax);
        }
    }

    public int HealthMax
    {
        get => _healthMax;
    }

    public bool IsAlive =>
        _health > 0;

    
    public void Initialize(int defautHealth)
    {
        _healthMax = defautHealth;
        _health = defautHealth;
    }
}
