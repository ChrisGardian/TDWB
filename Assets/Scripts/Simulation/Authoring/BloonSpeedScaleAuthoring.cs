using Unity.Entities;
using UnityEngine;

public class BloonSpeedScaleAuthoring : MonoBehaviour
{
    public float Value;
}

public class BloonSpeedScaleBaker : Baker<BloonSpeedScaleAuthoring>
{
    public override void Bake(BloonSpeedScaleAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.None);

        AddComponent(entity, new BloonSpeedScale { Value = authoring.Value });
    } 
}
