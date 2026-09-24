using Unity.Entities;

public struct CurrentLayer : IComponentData
{
    // 0 being red balloon the least strong one and going up for the next. So blue will be 1
    public int CurrentLayerIndex;
}