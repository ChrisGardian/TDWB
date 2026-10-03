using Unity.Entities;
using UnityEngine;

public class BloonAuthoring : MonoBehaviour
{
    public float MoveSpeed;
    public float Size;
    public GameObject[] Children;
    public BloonKind Kind;
}

public class BloonBaker : Baker<BloonAuthoring>
{
    public override void Bake(BloonAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        DynamicBuffer<BloonChild> children = AddBuffer<BloonChild>(entity);
        foreach (GameObject child in authoring.Children)
        {
            Entity childPrefab = GetEntity(child, TransformUsageFlags.Dynamic);
            children.Add(new BloonChild { Value = childPrefab });
        }

        AddComponent<BloonTag>(entity);
        AddComponent(entity, new MoveSpeed { Value = authoring.MoveSpeed });
        AddComponent(entity, new Size { Value = authoring.Size });
        AddComponent(entity, new PathProgress());
        AddComponent(entity, new BloonType { Value = authoring.Kind });
    } 
}
