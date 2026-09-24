using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class ProjectileHitSystem : SystemBase
{
    private EntityQuery ProjectileQuery;
    protected override void OnCreate()
    {
        ProjectileQuery = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<ProjectileTag>()
            .WithAll<Target>()
            .Build(this);
    }
    protected override void OnUpdate()
    {
        NativeArray<Entity> projectiles = ProjectileQuery.ToEntityArray(Allocator.Temp);
        foreach (Entity projectile in projectiles)
        {
            Entity target = EntityManager.GetComponentData<Target>(projectile).Value;
            float3 targetPos = EntityManager.GetComponentData<LocalTransform>(target).Position;
            float targetSize = EntityManager.GetComponentData<Size>(target).Value;

            float3 projectilePos = EntityManager.GetComponentData<LocalTransform>(projectile).Position;
            float projectileSize = EntityManager.GetComponentData<Size>(projectile).Value;

            float distance = math.distance(targetPos, projectilePos);
            if (distance < targetSize + projectileSize)
            {
                EntityManager.AddComponent<HitTag>(target);
                EntityManager.DestroyEntity(projectile);
            }
        }
        projectiles.Dispose();
    }
}