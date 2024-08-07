using Unity.Entities;
using Unity.Mathematics;

public struct PlayerInputComponent : IComponentData
{
    public float2 MovementDirection;
    public float2 AimDirection;
}
