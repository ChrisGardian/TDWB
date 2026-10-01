using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class PathFollowSystem : SystemBase
{
    protected override void OnCreate()
    {
        RequireForUpdate<BloonSpeedScale>();
        RequireForUpdate<PathWaypoint>();
    }

    protected override void OnUpdate()
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        float speedScale = SystemAPI.GetSingleton<BloonSpeedScale>().Value;

        DynamicBuffer<PathWaypoint> waypoints = SystemAPI.GetSingletonBuffer<PathWaypoint>(true);

        foreach (var (transform, progress, speed) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<PathProgress>, RefRO<MoveSpeed>>().WithAll<BloonTag>())
        {
            if (progress.ValueRO.Value >= waypoints.Length)
                continue;
            
            float3 target = waypoints[progress.ValueRO.Value].Value;
            float3 current = transform.ValueRO.Position;
            float distance = math.distance(current, target);
            float step = speed.ValueRO.Value * speedScale * deltaTime;

            if (step >= distance)
            {
                transform.ValueRW.Position = target;
                progress.ValueRW.Value++;                
            }
            else
            {
                float3 direction = math.normalize(target - current);
                transform.ValueRW.Position = current + direction * step;
            }
        }
    }
}