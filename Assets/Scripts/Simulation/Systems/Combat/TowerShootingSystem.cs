using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial class TowerShootingSystem : SystemBase
{
    private EntityQuery TowerQuery;
    private EntityQuery BloonQuery;
    protected override void OnCreate()
    {
        TowerQuery = new EntityQueryBuilder(Allocator.Temp)
            .WithAll<TowerTag>()
            .WithAll<Target>()
            .Build(this);
        BloonQuery = EntityManager.CreateEntityQuery(typeof(BloonTag));
    }
    protected override void OnUpdate()
    {
        NativeArray<Entity> towers = TowerQuery.ToEntityArray(Allocator.Temp);
        float deltaTime = SystemAPI.Time.DeltaTime;

        foreach (Entity tower in towers)
        {
            FireRate fireRate = EntityManager.GetComponentData<FireRate>(tower);
            if (EntityManager.GetComponentData<FireRate>(tower).TimeTillNextShot <= 0)
            {
                Entity projectile = EntityManager.CreateEntity();

                Debug.Log("Projectile Launched");

                Entity target = EntityManager.GetComponentData<Target>(tower).Value;

                EntityManager.AddComponent<ProjectileTag>(projectile);
                EntityManager.AddComponentData(projectile, LocalTransform.FromPosition(EntityManager.GetComponentData<LocalTransform>(tower).Position));
                EntityManager.AddComponentData(projectile, new MoveSpeed { Value = 20f });
                EntityManager.AddComponentData(projectile, new Lifetime { Value = 5f });
                EntityManager.AddComponentData(projectile, new Size { Value = .1f });
                EntityManager.AddComponentData(projectile, new Target { Value = target });

                
                float3 targetPosition = EntityManager.GetComponentData<LocalTransform>(target).Position;
                float3 towerPosition = EntityManager.GetComponentData<LocalTransform>(tower).Position;
                float3 direction = math.normalize(targetPosition - towerPosition);

                EntityManager.AddComponentData(projectile, new Direction { Value = direction});

                EntityManager.SetComponentData<FireRate>(tower, new FireRate{ TimeBetweenShots = fireRate.TimeBetweenShots, TimeTillNextShot = fireRate.TimeBetweenShots});
            }
            else
            {
                EntityManager.SetComponentData<FireRate>(tower, new FireRate{ TimeBetweenShots = fireRate.TimeBetweenShots, TimeTillNextShot = fireRate.TimeTillNextShot - deltaTime });
            }
        }

        towers.Dispose();
    }
}