using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

[BurstCompile]
partial struct BulletSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityManager entityManager = state.EntityManager;
        NativeArray<Entity> allEntities = entityManager.GetAllEntities();

        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();

        foreach (var entity in allEntities)
        {
            if (entityManager.HasComponent<BulletComponent>(entity))
            {
                LocalTransform bulletTransform = entityManager.GetComponentData<LocalTransform>(entity);
                BulletComponent bulletComponent = entityManager.GetComponentData<BulletComponent>(entity);

                bulletTransform.Position += bulletComponent.Speed * SystemAPI.Time.DeltaTime * bulletTransform.Up();

                bulletComponent.RemainingLifeTime -= SystemAPI.Time.DeltaTime;
                if (bulletComponent.RemainingLifeTime <= 0)
                {
                    entityManager.DestroyEntity(entity);
                    continue;
                }
                NativeList<ColliderCastHit> hits = new NativeList<ColliderCastHit>(Allocator.Temp);
                float3 point1 = new float3(bulletTransform.Position - bulletTransform.Right() * 0.15f);
                float3 point2 = new float3(bulletTransform.Position + bulletTransform.Right() * 0.15f);
                
                entityManager.SetComponentData(entity, bulletTransform);
                entityManager.SetComponentData(entity, bulletComponent);

                uint layerMask = LayerMaskHelper.GetLayerMaskFromTwoLayers(CollisionLayer.Wall, CollisionLayer.Enemy);
                physicsWorld.CapsuleCastAll(point1, point2, bulletComponent.Size / 2, float3.zero, 1f, ref hits, new CollisionFilter()
                {
                    BelongsTo = (uint)CollisionLayer.Default,
                    CollidesWith = layerMask
                });

                if (hits.Length > 0)
                {
                    for (int i = 0; i < hits.Length; i++)
                    {
                        Entity hitEntity = hits[i].Entity;
                        if(!entityManager.HasComponent<EnemyControlComponent>(hitEntity))
                            continue;

                        EnemyControlComponent enemyControlComponent = entityManager.GetComponentData<EnemyControlComponent>(hitEntity);
                        enemyControlComponent.CurrentHealth -= bulletComponent.Damage;
                        entityManager.SetComponentData(hitEntity, enemyControlComponent);
                        
                        if(enemyControlComponent.CurrentHealth <= 0f)
                            entityManager.DestroyEntity(hitEntity);
                    }
                    
                    entityManager.DestroyEntity(entity);
                }

                hits.Dispose();
            }
        }
    }
}
