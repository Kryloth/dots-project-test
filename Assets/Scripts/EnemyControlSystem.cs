using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
partial struct EnemyControlSystem : ISystem
{
    private EntityManager _entityManager;

    private Entity _playerEntity;

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        _entityManager = state.EntityManager;
        _playerEntity = SystemAPI.GetSingletonEntity<PlayerControlComponent>();
        
        LocalTransform playerTransform = _entityManager.GetComponentData<LocalTransform>(_playerEntity);

        NativeArray<Entity> allEntities = _entityManager.GetAllEntities();

        foreach (Entity entity in allEntities)
        {
            if(!_entityManager.HasComponent<EnemyControlComponent>(entity))
                continue;
            
            LocalTransform enemyTransform = _entityManager.GetComponentData<LocalTransform>(entity);
            EnemyControlComponent enemyControlComponent = _entityManager.GetComponentData<EnemyControlComponent>(entity);

            float3 moveDir = math.normalize(playerTransform.Position - enemyTransform.Position);
            enemyTransform.Position += moveDir * enemyControlComponent.MoveSpeed * SystemAPI.Time.DeltaTime;
            
            float3 direction = math.normalize(playerTransform.Position - enemyTransform.Position);
            float angle = math.atan2(direction.y, direction.x);
            angle -= math.radians(90f);
            quaternion lookRot = quaternion.AxisAngle(new float3(0, 0, 1), angle);
            enemyTransform.Rotation = lookRot;
            
            _entityManager.SetComponentData(entity, enemyTransform);
        }
    }
}
