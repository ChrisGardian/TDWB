using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial class TargetSystem : SystemBase
{
    private EntityQuery TowerQuery;
    private EntityQuery BloonQuery;
    protected override void OnCreate()
    {
        TowerQuery = EntityManager.CreateEntityQuery(typeof(TowerTag));
        BloonQuery = EntityManager.CreateEntityQuery(typeof(BloonTag));
    }
    protected override void OnUpdate()
    {
        NativeArray<Entity> towers = TowerQuery.ToEntityArray(Allocator.Temp);
        NativeArray<Entity> bloons = BloonQuery.ToEntityArray(Allocator.Temp);
        foreach (Entity tower in towers)
        {
            LocalTransform towerTransform = EntityManager.GetComponentData<LocalTransform>(tower);
            Range range = EntityManager.GetComponentData<Range>(tower);
            if (EntityManager.HasComponent<Target>(tower))
            {
                Entity targetBloon = EntityManager.GetComponentData<Target>(tower).Value;
                LocalTransform bloonTransform = EntityManager.GetComponentData<LocalTransform>(targetBloon);
                float distance = math.distance(towerTransform.Position, bloonTransform.Position);
                if (distance > range.Value)
                {
                    EntityManager.RemoveComponent<Target>(tower);
                }
            }
            if (!EntityManager.HasComponent<Target>(tower))
            {
                foreach (Entity bloon in bloons)
                    {
                        LocalTransform bloonTransform = EntityManager.GetComponentData<LocalTransform>(bloon);
                        float distance = math.distance(towerTransform.Position, bloonTransform.Position);
                        if (distance < range.Value)
                        {
                            EntityManager.AddComponentData(tower, new Target { Value = bloon});
                            break;
                        }
                    }
            }
                
        }
        towers.Dispose();
        bloons.Dispose();
    }
}