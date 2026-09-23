using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class PathTestSpawnSystem : SystemBase
{
    protected override void OnCreate()
    {
        Entity bloon = EntityManager.CreateEntity();

        EntityManager.AddComponentData(bloon, new PathProgress { CurrentWaypointIndex = 0 });
        EntityManager.AddComponentData(bloon, new MoveSpeed { Value = 1f });
        EntityManager.AddComponentData(bloon, LocalTransform.FromPosition(0f, 0f, 0f));
        EntityManager.AddComponentData(bloon, new CurrentLayer { CurrentLayerIndex = 2 });
        EntityManager.AddComponent<PathFollowerTag>(bloon);

        DynamicBuffer<PathWaypoint> waypoints = EntityManager.AddBuffer<PathWaypoint>(bloon);
        waypoints.Add(new PathWaypoint { Value = new float3(0f, 0f, 0f)});
        waypoints.Add(new PathWaypoint { Value = new float3(5f, 0f, 0f)});
        waypoints.Add(new PathWaypoint { Value = new float3(5f, 3f, 0f)});

        Enabled = false;
    }

    protected override void OnUpdate()
    {
    }
}