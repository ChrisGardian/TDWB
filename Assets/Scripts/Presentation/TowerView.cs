using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class TowerView : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        EntityQuery query = entityManager.CreateEntityQuery(typeof(TowerTag));
        NativeArray<Entity> towers = query.ToEntityArray(Allocator.Temp);
        foreach (Entity tower in towers)
        {
            LocalTransform entityTransform = entityManager.GetComponentData<LocalTransform>(tower);
            transform.position = entityTransform.Position;
        }
        towers.Dispose();
    }
}
