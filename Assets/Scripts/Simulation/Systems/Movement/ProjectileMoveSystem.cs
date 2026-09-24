using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

public partial class ProjectileMoveSystem : SystemBase
{
    protected override void OnUpdate()
    {
        float deltaTime = SystemAPI.Time.DeltaTime;

        EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);

        foreach (var (transform, speed, direction, lifetime, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveSpeed>, RefRO<Direction>, RefRW<Lifetime>>().WithEntityAccess())
        {
            transform.ValueRW.Position += direction.ValueRO.Value * speed.ValueRO.Value * deltaTime;
            if (lifetime.ValueRO.Value > 0)
            {
                lifetime.ValueRW.Value -= deltaTime; 
            }
            else
            {
                ecb.DestroyEntity(entity);
            }
        }

        ecb.Playback(EntityManager);
        ecb.Dispose();
    }
}