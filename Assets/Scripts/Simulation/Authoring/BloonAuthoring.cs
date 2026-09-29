using Unity.Entities;
using UnityEngine;

public class BloonAuthoring : MonoBehaviour
{
    public float MoveSpeed;
    public float Size;
    public int CurrentLayer;
}

public class BloonBaker : Baker<BloonAuthoring>
{
    public override void Bake(BloonAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent<BloonTag>(entity);
        AddComponent(entity, new MoveSpeed { Value = authoring.MoveSpeed });
        AddComponent(entity, new Size { Value = authoring.Size });
        AddComponent(entity, new CurrentLayer { CurrentLayerIndex = authoring.CurrentLayer });
        AddComponent(entity, new PathProgress());
    } 
}
