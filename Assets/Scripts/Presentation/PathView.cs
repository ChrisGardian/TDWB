using Unity.Entities;
using UnityEngine;

public class PathView : MonoBehaviour
{
    LineRenderer lineRenderer;
    EntityManager entityManager;
    EntityQuery query;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        query = entityManager.CreateEntityQuery(typeof(PathWaypoint));
    }

    // Update is called once per frame
    void Update()
    {
        if (query.IsEmpty) return;
        Entity path = query.GetSingletonEntity();
        DynamicBuffer<PathWaypoint> waypoints = entityManager.GetBuffer<PathWaypoint>(path, true);
        lineRenderer.positionCount = waypoints.Length;
        for (int i = 0; i < waypoints.Length; i++)
        {
            lineRenderer.SetPosition(i, waypoints[i].Value);
        }   
    }
}
