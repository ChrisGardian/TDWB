using Unity.Entities;
using UnityEngine;

public class BloonRegistryAuthoring : MonoBehaviour
{
    public GameObject RedBloon;
}

public class BloonRegistryBaker : Baker<BloonRegistryAuthoring>
{
    public override void Bake(BloonRegistryAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);
        Entity redBloonPrefab = GetEntity(authoring.RedBloon, TransformUsageFlags.Dynamic);

        AddComponent(entity, new BloonPrefabRegistry { RedBloon = redBloonPrefab });
    } 
}
