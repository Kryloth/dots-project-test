using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

[BurstCompile]
partial struct PlayerControlSystem : ISystem
{
    private EntityManager _entityManager;

    private Entity _playerEntity;
    private Entity _inputEntity;

    private PlayerControlComponent _playerControlComponent;
    private PlayerInputComponent _playerInputComponent;

    private float _shootCooldown;

    public void OnUpdate(ref SystemState state)
    {
        _entityManager = state.EntityManager;
        _playerEntity = SystemAPI.GetSingletonEntity<PlayerControlComponent>();
        _inputEntity = SystemAPI.GetSingletonEntity<PlayerInputComponent>();

        _playerControlComponent = _entityManager.GetComponentData<PlayerControlComponent>(_playerEntity);
        _playerInputComponent = _entityManager.GetComponentData<PlayerInputComponent>(_inputEntity);
        
        Move(ref state);
        Aim(ref state);
        Shoot(ref state);
    }

    private void Move(ref SystemState state)
    {
        LocalTransform playerTransform = _entityManager.GetComponentData<LocalTransform>(_playerEntity);
        playerTransform.Position += new float3(_playerInputComponent.MovementDirection * _playerControlComponent.MoveSpeed * SystemAPI.Time.DeltaTime, 0);

        _entityManager.SetComponentData(_playerEntity, playerTransform);
    }

    private void Aim(ref SystemState state)
    {
        LocalTransform playerTransform = _entityManager.GetComponentData<LocalTransform>(_playerEntity);
        Vector2 inputMoveDir = _playerInputComponent.MovementDirection;
        Vector2 inputAimDir = _playerInputComponent.AimDirection;
        
        Vector2 actualDir = _playerControlComponent.AimDirection;
        if (inputMoveDir != Vector2.zero)
            actualDir = _playerInputComponent.MovementDirection;

        if (inputAimDir != Vector2.zero)
            actualDir = _playerInputComponent.AimDirection;
        
        if(inputMoveDir == Vector2.zero && inputAimDir == Vector2.zero)
            return;

        _playerControlComponent.AimDirection = actualDir;
        
        var lookRotate = Quaternion.LookRotation(Vector3.forward, (Vector2)_playerControlComponent.AimDirection);
        playerTransform.Rotation = Quaternion.Lerp(playerTransform.Rotation, lookRotate, _playerControlComponent.AimSpeed * SystemAPI.Time.DeltaTime);
        
        _entityManager.SetComponentData(_playerEntity, _playerControlComponent);
        _entityManager.SetComponentData(_playerEntity, playerTransform);
    }

    [BurstCompile]
    private void Shoot(ref SystemState state)
    {
        for (int i = 0; i < _playerControlComponent.BulletToSpawn; i++)
        {
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            Entity bulletEntity = _entityManager.Instantiate(_playerControlComponent.BulletPrefab);
            
            ecb.AddComponent(bulletEntity, new BulletComponent()
            {
                Speed = 15f,
                Size = 0.25f,
                Damage = 10f,
                RemainingLifeTime = 2f
            });
            
            LocalTransform bulletTransform = _entityManager.GetComponentData<LocalTransform>(bulletEntity);
            LocalTransform playerTransform = _entityManager.GetComponentData<LocalTransform>(_playerEntity);

            bulletTransform.Rotation = playerTransform.Rotation;

            float randomOffset = Random.Range(-_playerControlComponent.BulletSpread, _playerControlComponent.BulletSpread);
            bulletTransform.Position = playerTransform.Position + (playerTransform.Up() * 0.5f) + (bulletTransform.Right() * randomOffset);

            ecb.SetComponent(bulletEntity, bulletTransform);
            
            ecb.Playback(_entityManager);
            ecb.Dispose();
        }
    }
}
