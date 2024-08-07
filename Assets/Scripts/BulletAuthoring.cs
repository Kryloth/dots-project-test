using Unity.Entities;
using UnityEngine;

class BulletAuthoring : MonoBehaviour
{
    [field:SerializeField]
    public float Speed { get; set; }
}

class BulletBaker : Baker<BulletAuthoring>
{
    public override void Bake(BulletAuthoring authoring)
    {
        // AddComponent(entity:);
    }
}
