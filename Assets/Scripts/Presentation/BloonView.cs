using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class BloonView : MonoBehaviour
{
    [SerializeField] private List<GameObject> bloonPrefabs;
    private Dictionary<Entity, GameObject> entityViews;
    private EntityManager entityManager;
    private EntityQuery query;
    private List<Entity> toDestroy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entityViews = new Dictionary<Entity, GameObject>();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        query = entityManager.CreateEntityQuery(typeof(BloonTag));
        toDestroy = new List<Entity>();
    }

    // Update is called once per frame
    void Update()
    {
        NativeArray<Entity> bloons = query.ToEntityArray(Allocator.Temp);

        foreach (Entity bloon in bloons)
        {
            LocalTransform localTransform = entityManager.GetComponentData<LocalTransform>(bloon);
            if (entityViews.TryGetValue(bloon, out GameObject go))
            {
                go.transform.position = localTransform.Position;
            }
            else
            {
                Size size = entityManager.GetComponentData<Size>(bloon);
                BloonType type = entityManager.GetComponentData<BloonType>(bloon);
                int index = (int)type.Value;
                GameObject bloonPrefab = bloonPrefabs[index];
                GameObject newGo = Instantiate(bloonPrefab, localTransform.Position, Quaternion.identity, transform);
                newGo.transform.localScale = Vector3.one * (2f * size.Value);
                entityViews[bloon] = newGo;
            }
        }

        bloons.Dispose();

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
