using Unity.Entities;
using UnityEngine;

class EnemyControlAuthoring : MonoBehaviour
{
    public float MoveSpeed;
}

class EnemyControlAuthoringBaker : Baker<EnemyControlAuthoring>
{
    public override void Bake(EnemyControlAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        
        AddComponent(entity, new EnemyControlComponent()
        {
            MoveSpeed = authoring.MoveSpeed
        });
    }
}
