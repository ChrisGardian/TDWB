
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class ProjectileView : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    private Dictionary<Entity, GameObject> entityViews;
    private EntityManager entityManager;
    private EntityQuery query;
    private List<Entity> toDestroy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entityViews = new Dictionary<Entity, GameObject>();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        query = entityManager.CreateEntityQuery(typeof(ProjectileTag));
        toDestroy = new List<Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        NativeArray<Entity> projectiles = query.ToEntityArray(Allocator.Temp);

        foreach (Entity projectile in projectiles)
        {
            LocalTransform localTransform = entityManager.GetComponentData<LocalTransform>(projectile);
            if (entityViews.TryGetValue(projectile, out GameObject go))
            {
                go.transform.position = localTransform.Position;
            }
            else
            {
                GameObject newGo = Instantiate(projectilePrefab, localTransform.Position, Quaternion.identity, transform);
                entityViews[projectile] = newGo;
            }
        }

        projectiles.Dispose();

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
