using Unity.Entities;

public struct EnemySpawnerComponent : IComponentData
{
    public Entity EnemyPrefab;
    public int NumberToSpawnPerSecond;
    public int NumberToSpawnIncrement;
    public int MaxNumberToSpawnPerSecond;
    public float EnemySpawnRadius;
    public float MinimumDistanceFromPlayer;
    public float TimeBeforeNextSpawn;
    public float CurrentTimeBeforeNextSpawn;
}
