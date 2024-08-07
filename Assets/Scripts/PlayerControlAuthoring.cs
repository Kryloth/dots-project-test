using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class PlayerControlAuthoring : MonoBehaviour
{
    public float MoveSpeed = 5f;
    public GameObject BulletPrefab;
    public int BulletToSpawn = 10;
    [Range(0f, 10f)]public float BulletSpread = 5f;
    public float AimSpeed = 5f;
    
    class PlayerControlAuthoringBaker : Baker<PlayerControlAuthoring>
    {
        public override void Bake(PlayerControlAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new PlayerControlComponent()
            {
                MoveSpeed = authoring.MoveSpeed,
                BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.None),
                BulletToSpawn = authoring.BulletToSpawn,
                BulletSpread = authoring.BulletSpread,
                AimSpeed = authoring.AimSpeed,
                AimDirection = float2.zero
            });
        }
    }
}


