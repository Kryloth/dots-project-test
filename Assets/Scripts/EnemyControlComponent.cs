using Unity.Entities;

public struct EnemyControlComponent : IComponentData
{
    public float CurrentHealth;
    public float MoveSpeed;
}
