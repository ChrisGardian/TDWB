using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class PathFollowerView : MonoBehaviour
{
    private EntityManager entityManager;
    private Entity trackedEntity;
    private EntityQuery query;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        query = entityManager.CreateEntityQuery(typeof(BloonTag));
    }

    // Update is called once per frame
    void Update()
    {
        if (!query.IsEmpty)
        {
            trackedEntity = query.GetSingletonEntity();
            LocalTransform entityTransform = entityManager.GetComponentData<LocalTransform>(trackedEntity);
            transform.position = entityTransform.Position;
        }
    }
}
