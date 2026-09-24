using Unity.Entities;

public struct FireRate : IComponentData
{
    public float TimeBetweenShots;
    public float TimeTillNextShot;
}