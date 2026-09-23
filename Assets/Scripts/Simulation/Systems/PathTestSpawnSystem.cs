using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial class PathTestSpawnSystem : SystemBase
{
    protected override void OnCreate()
    {
        Entity entity = EntityManager.CreateEntity();

        EntityManager.AddComponentData(entity, new PathProgress { CurrentWaypointIndex = 0 });
        EntityManager.AddComponentData(entity, new MoveSpeed { Value = 1f });
        EntityManager.AddComponentData(entity, LocalTransform.FromPosition(0f, 0f, 0f));
        EntityManager.AddComponent<PathFollowerTag>(entity);

        DynamicBuffer<PathWaypoint> waypoints = EntityManager.AddBuffer<PathWaypoint>(entity);
        waypoints.Add(new PathWaypoint { Value = new float3(0f, 0f, 0f)});
        waypoints.Add(new PathWaypoint { Value = new float3(5f, 0f, 0f)});
        waypoints.Add(new PathWaypoint { Value = new float3(5f, 3f, 0f)});

        Enabled = false;
    }

    protected override void OnUpdate()
    {
    }
}