using Unity.Entities;
using UnityEngine;

public class BloonRegistryAuthoring : MonoBehaviour
{
    public GameObject Bloon;
}

public class BloonRegistryBaker : Baker<BloonRegistryAuthoring>
{
    public override void Bake(BloonRegistryAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);
        Entity BloonPrefab = GetEntity(authoring.Bloon, TransformUsageFlags.Dynamic);

        AddComponent(entity, new BloonPrefabRegistry { Bloon = BloonPrefab });
    } 
}
