using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class PathFollowSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (var (transform, progress, speed, waypoints) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<PathProgress>, RefRO<MoveSpeed>, DynamicBuffer<PathWaypoint>>())
        {
            if (progress.ValueRO.CurrentWaypointIndex >= waypoints.Length)
                       continue;
            
            float3 target = waypoints[progress.ValueRO.CurrentWaypointIndex].Value;
            float3 current = transform.ValueRO.Position;
            float distance = math.distance(current, target);
            float step = speed.ValueRO.Value * deltaTime;

            if (step >= distance)
            {
                transform.ValueRW.Position = target;
                progress.ValueRW.CurrentWaypointIndex++;                
            }
            else
            {
                float3 direction = math.normalize(target - current);
                transform.ValueRW.Position = current + direction * step;
            }
        }
    }
}