using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class BloonPopSystem : SystemBase
{
    EntityQuery Query;
    EntityQuery TowerQuery;
    EntityQuery ProjectileQuery;
    protected override void OnCreate()
    {
        RequireForUpdate<PathWaypoint>();
        Query = EntityManager.CreateEntityQuery(typeof(HitTag));
        TowerQuery = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<TowerTag>()
            .WithAll<Target>()
            .Build(this);
        ProjectileQuery = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<ProjectileTag>()
            .WithAll<Target>()
            .Build(this);
    }
    protected override void OnUpdate()
    {
        NativeArray<Entity> bloons = Query.ToEntityArray(Allocator.Temp);
        NativeArray<Entity> towers = TowerQuery.ToEntityArray(Allocator.Temp);
        NativeArray<Entity> projectiles = ProjectileQuery.ToEntityArray(Allocator.Temp);
        NativeArray<PathWaypoint> pathWaypoints = SystemAPI.GetSingletonBuffer<PathWaypoint>(true).ToNativeArray(Allocator.Temp);

        foreach (Entity bloon in bloons)
        {
            LocalTransform localTransform = EntityManager.GetComponentData<LocalTransform>(bloon);
            PathProgress pathProgress = EntityManager.GetComponentData<PathProgress>(bloon);

            NativeArray<BloonChild> bloonChildren = EntityManager.GetBuffer<BloonChild>(bloon).ToNativeArray(Allocator.Temp);

            float gap = 0.1f;
            float3 direction;
            int pathProgressValue = pathProgress.Value;

            if (0 < pathProgressValue && pathProgressValue < pathWaypoints.Length)
            {
                float3 waypointNext = pathWaypoints[pathProgressValue].Value;
                float3 waypointBefore = pathWaypoints[pathProgressValue - 1].Value;
                direction = math.normalizesafe(waypointNext - waypointBefore);
            }
            else if (pathProgressValue == 0)
            {
                float3 waypointNext = pathWaypoints[pathProgressValue + 1].Value;
                float3 waypointBefore = pathWaypoints[pathProgressValue].Value;
                direction = math.normalizesafe(waypointNext - waypointBefore);
            }
            else
            {
                direction = float3.zero;
            }

            float3 gapVector = gap * direction;

            int i = 0;

            foreach (BloonChild child in bloonChildren)
            {
                float3 gapCopy = gapVector * i++;
                LocalTransform childTransform = localTransform;
                childTransform.Position = localTransform.Position - gapCopy;

                Entity childBloon = EntityManager.Instantiate(child.Value);
                EntityManager.SetComponentData(childBloon, childTransform);
                EntityManager.SetComponentData(childBloon, pathProgress);
            }

            foreach (Entity tower in towers)
            {
                if (!EntityManager.HasComponent<Target>(tower)) continue;
                Target target = EntityManager.GetComponentData<Target>(tower);
                if (target.Value == bloon)
                {
                    EntityManager.RemoveComponent<Target>(tower);
                }
            }
            foreach (Entity projectile in projectiles)
            {
                if (!EntityManager.HasComponent<Target>(projectile)) continue;
                Target target = EntityManager.GetComponentData<Target>(projectile);
                if (target.Value == bloon)
                {
                    EntityManager.RemoveComponent<Target>(projectile);
                }
            }

            EntityManager.DestroyEntity(bloon);
            bloonChildren.Dispose();
        }

        bloons.Dispose();
        towers.Dispose();
        projectiles.Dispose();
        pathWaypoints.Dispose();
    }
}