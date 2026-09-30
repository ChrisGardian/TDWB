using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class TowerView : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;
    private Dictionary<Entity, GameObject> entityViews;
    private EntityManager entityManager;
    private EntityQuery query;
    private List<Entity> toDestroy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entityViews = new Dictionary<Entity, GameObject>();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        query = entityManager.CreateEntityQuery(typeof(TowerTag));
        toDestroy = new List<Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        NativeArray<Entity> towers = query.ToEntityArray(Allocator.Temp);

        foreach (Entity tower in towers)
        {
            LocalTransform localTransform = entityManager.GetComponentData<LocalTransform>(tower);
            if (entityViews.TryGetValue(tower, out GameObject go))
            {
                go.transform.position = localTransform.Position;
            }
            else
            {
                GameObject newGo = Instantiate(towerPrefab, localTransform.Position, Quaternion.identity, transform);
                entityViews[tower] = newGo;
            }
        }

        towers.Dispose();

        foreach (Entity entity in entityViews.Keys)
        {
            if (!entityManager.Exists(entity))
            {
                toDestroy.Add(entity);
            }
        }
        foreach (Entity entity in toDestroy)
        {
            GameObject go = entityViews[entity];
            Destroy(go);
            entityViews.Remove(entity);
        }
        toDestroy.Clear();
    }
}
