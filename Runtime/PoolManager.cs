using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace AdvancedObjectPooling
{
    // A tiny component attached at runtime so the object remembers its pool ID
    public class PooledItemTracker : MonoBehaviour
    {
        public ulong PoolId { get; set; }
    }

    public static class PoolManager
    {
        // V1.1 Update: Changed from Type to ulong for variant support & ECS future-proofing
        private static Dictionary<ulong, object> _pools = new Dictionary<ulong, object>();

        public static void InitializePool<T>(ulong poolId, PrefabFactory<T> factory, int defaultCapacity = 100, int maxSize = 500) where T : Component
        {
            if (!_pools.ContainsKey(poolId))
            {
                var pool = new ObjectPool<T>(
                    createFunc: () =>
                    {
                        T obj = factory.Create();
                        
                        // Automatically give the object a tracker so it knows where it belongs
                        var tracker = obj.gameObject.AddComponent<PooledItemTracker>();
                        tracker.PoolId = poolId;
                        
                        return obj;
                    },
                    actionOnGet: obj => obj.gameObject.SetActive(true),
                    actionOnRelease: obj => obj.gameObject.SetActive(false),
                    actionOnDestroy: obj => Object.Destroy(obj.gameObject),
                    defaultCapacity: defaultCapacity,
                    maxSize: maxSize
                );

                _pools.Add(poolId, pool);
            }
        }

        public static T Get<T>(ulong poolId) where T : Component
        {
            if (_pools.TryGetValue(poolId, out var poolObj))
            {
                return ((ObjectPool<T>)poolObj).Get();
            }
            
            Debug.LogError($"Pool with ID {poolId} is not initialized.");
            return null;
        }

        public static void Release<T>(T obj) where T : Component
        {
            // Read the tracker to find the exact pool to return to
            if (obj.TryGetComponent<PooledItemTracker>(out var tracker))
            {
                if (_pools.TryGetValue(tracker.PoolId, out var poolObj))
                {
                    ((ObjectPool<T>)poolObj).Release(obj);
                    return;
                }
            }
            
            Debug.LogWarning($"Object {obj.name} is not tracked by the PoolManager. Destroying instead.");
            Object.Destroy(obj.gameObject);
        }
    }
}
