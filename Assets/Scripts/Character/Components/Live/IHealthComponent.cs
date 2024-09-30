public interface IHealthComponent 
{
    public float Health { get; set; }
    
    public int HealthMax { get; }
    
    public bool IsAlive { get; }


    public void Initialize(int defautHealth);
}
