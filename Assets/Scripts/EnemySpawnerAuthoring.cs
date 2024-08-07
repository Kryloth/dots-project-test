using Unity.Entities;
using UnityEngine;

class EnemySpawnerAuthoring : MonoBehaviour
{
    public GameObject EnemyPrefab;
    public int NumberToSpawnPerSecond = 50;
    public int NumberToSpawnIncrement = 2;
    public int MaxNumberToSpawnPerSecond = 200;
    public float EnemySpawnRadius = 40f;
    public float MinimumDistanceFromPlayer = 5f;
    public float TimeBeforeNextSpawn = 2f;
}

class EnemySpawnerAuthoringBaker : Baker<EnemySpawnerAuthoring>
{
    public override void Bake(EnemySpawnerAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);
        
        AddComponent(entity, new EnemySpawnerComponent()
        {
            EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.None),
            NumberToSpawnPerSecond = authoring.NumberToSpawnPerSecond,
            NumberToSpawnIncrement = authoring.NumberToSpawnIncrement,
            MaxNumberToSpawnPerSecond = authoring.MaxNumberToSpawnPerSecond,
            EnemySpawnRadius = authoring.EnemySpawnRadius,
            MinimumDistanceFromPlayer = authoring.MinimumDistanceFromPlayer,
            TimeBeforeNextSpawn = authoring.TimeBeforeNextSpawn,
        });
    }
}
