using Unity.Entities;

public struct BulletComponent : IComponentData
{
    public float Speed { get; set; }
    public float Size;
    public float Damage;
    public float RemainingLifeTime;
}
