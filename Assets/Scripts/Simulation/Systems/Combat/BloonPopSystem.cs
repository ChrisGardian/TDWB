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

        foreach (Entity bloon in bloons)
        {
            LocalTransform localTransform = EntityManager.GetComponentData<LocalTransform>(bloon);
            PathProgress pathProgress = EntityManager.GetComponentData<PathProgress>(bloon);

            NativeArray<BloonChild> bloonChildren = EntityManager.GetBuffer<BloonChild>(bloon).ToNativeArray(Allocator.Temp);

            float3 decalage = new float3(0.1f, 0f, 0f);
            int i = 0;

            foreach (BloonChild child in bloonChildren)
            {
                float3 decalageCopy = decalage * i++;
                localTransform.Position = localTransform.Position - decalageCopy;

                Entity childBloon = EntityManager.Instantiate(child.Value);
                EntityManager.SetComponentData(childBloon, localTransform);
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
    }
}