public interface IAttackComponent
{
    public float Damage { get; }
    public float AttackRange { get; }


    void Initialize(CharacterData characterData);
    
    public void MakeAttack();
}
