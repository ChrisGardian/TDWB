using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class PathFollowerView : MonoBehaviour
{
    private EntityManager entityManager;
    private Entity trackedEntity;
    private bool hasEntity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasEntity)
        {
            EntityQuery query = entityManager.CreateEntityQuery(typeof(PathFollowerTag));
            if (query.CalculateEntityCount() == 0)
                return;

            trackedEntity = query.GetSingletonEntity();
            hasEntity = true;
        }

        LocalTransform entityTransform = entityManager.GetComponentData<LocalTransform>(trackedEntity);
        transform.position = entityTransform.Position;
    }
}
