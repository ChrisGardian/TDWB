using Unity.Collections;
using Unity.Entities;

public partial class LayerPopSystem : SystemBase
{
    EntityQuery Query;
    protected override void OnCreate()
    {
        Query = EntityManager.CreateEntityQuery(typeof(HitTag));
    }
    protected override void OnUpdate()
    {
        NativeArray<Entity> bloons = Query.ToEntityArray(Allocator.Temp);

        foreach (Entity bloon in bloons)
        {
            CurrentLayer layer = EntityManager.GetComponentData<CurrentLayer>(bloon);
            if (layer.CurrentLayerIndex == 0) // bloons will get destroyed
            {
                EntityManager.DestroyEntity(bloon);
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