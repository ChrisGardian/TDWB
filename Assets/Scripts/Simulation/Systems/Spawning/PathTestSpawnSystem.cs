using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

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

        EntityManager.SetComponentData(redBloon, LocalTransform.FromPosition(0f, 0f, 0f));

        DynamicBuffer<PathWaypoint> waypoints = EntityManager.AddBuffer<PathWaypoint>(redBloon);
        waypoints.Add(new PathWaypoint { Value = new float3(0f, 0f, 0f)});
        waypoints.Add(new PathWaypoint { Value = new float3(5f, 0f, 0f)});
        waypoints.Add(new PathWaypoint { Value = new float3(5f, 3f, 0f)});

        Enabled = false;
    }
}