using Unity.Entities;
using UnityEngine;

public class PathAuthoring : MonoBehaviour
{
}

public class PathBaker : Baker<PathAuthoring>
{
    public override void Bake(PathAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);

        DynamicBuffer<PathWaypoint> waypoints = AddBuffer<PathWaypoint>(entity);

        foreach (GameObject child in GetChildren())
        {
            Transform childTransform = GetComponent<Transform>(child);
            waypoints.Add(new PathWaypoint { Value = childTransform.position });
        }
    } 
}
