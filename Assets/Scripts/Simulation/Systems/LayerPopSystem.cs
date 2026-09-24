using Unity.Collections;
using Unity.Entities;

public partial class LayerPopSystem : SystemBase
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

        foreach (Entity bloon in bloons)
        {
            CurrentLayer layer = EntityManager.GetComponentData<CurrentLayer>(bloon);
            if (layer.CurrentLayerIndex == 0) // bloons will get destroyed
            {
                NativeArray<Entity> towers = TowerQuery.ToEntityArray(Allocator.Temp);
                foreach (Entity tower in towers)
                {
                    if (EntityManager.GetComponentData<Target>(tower).Value == bloon)
                    {
                        EntityManager.RemoveComponent<Target>(tower);
                    }
                }
                NativeArray<Entity> projectiles = ProjectileQuery.ToEntityArray(Allocator.Temp);
                foreach (Entity projectile in projectiles)
                {
                    if (EntityManager.GetComponentData<Target>(projectile).Value == bloon)
                    {
                        EntityManager.RemoveComponent<Target>(projectile);
                    }
                }
                EntityManager.DestroyEntity(bloon);
                towers.Dispose();
                projectiles.Dispose();
            } 
            else
            {
                EntityManager.SetComponentData(bloon, new CurrentLayer{ CurrentLayerIndex = layer.CurrentLayerIndex - 1});
                EntityManager.RemoveComponent<HitTag>(bloon);
            }
        }

        bloons.Dispose();
    }
}