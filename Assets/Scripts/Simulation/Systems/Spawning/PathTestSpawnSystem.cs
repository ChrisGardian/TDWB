using Unity.Entities;
using Unity.Transforms;

public partial class PathTestSpawnSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<BloonPrefabRegistry>();

        Entity tower = EntityManager.CreateEntity();

        EntityManager.AddComponent<TowerTag>(tower);
        EntityManager.AddComponentData(tower, LocalTransform.FromPosition(3f, 3f, 0f));
        EntityManager.AddComponentData(tower, new FireRate { TimeBetweenShots = 0.5f, TimeTillNextShot = 0f});
        EntityManager.AddComponentData(tower, new Range { Value = 3f});
    }

    protected override void OnUpdate()
    {
        BloonPrefabRegistry registry = SystemAPI.GetSingleton<BloonPrefabRegistry>();

        Entity redBloonPrefab = registry.RedBloon;

        Entity redBloon = EntityManager.Instantiate(redBloonPrefab);

        DynamicBuffer<PathWaypoint> waypoints = SystemAPI.GetSingletonBuffer<PathWaypoint>(true);

        EntityManager.SetComponentData(redBloon, LocalTransform.FromPosition(waypoints[0].Value));

        Enabled = false;
    }
}