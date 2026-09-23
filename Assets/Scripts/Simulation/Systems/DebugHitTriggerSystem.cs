using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class DebugHitTriggerSystem : SystemBase
{
    protected override void OnUpdate()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Bloon hit");
            EntityQuery bloonsQuery = EntityManager.CreateEntityQuery(typeof(CurrentLayer));
            NativeArray<Entity> bloons = bloonsQuery.ToEntityArray(Allocator.Temp);

            foreach (Entity bloon in bloons)
            {
                EntityManager.AddComponent<HitTag>(bloon);
            }

            bloons.Dispose();
        }
        
    }
}