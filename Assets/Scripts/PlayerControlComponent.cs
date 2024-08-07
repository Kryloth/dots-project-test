using Unity.Entities;
using Unity.Mathematics;

public struct PlayerControlComponent : IComponentData
{
    public float MoveSpeed;
    public Entity BulletPrefab;
    public int BulletToSpawn;
    public float BulletSpread;
    public float AimSpeed;
    public float2 AimDirection;
}
