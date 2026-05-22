using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace AdvancedObjectPooling
{
    public static class PoolManager
    {
        private static readonly Dictionary<Type, object> _pools = new Dictionary<Type, object>();

        public static void InitializePool<T>(IFactory<T> factory, int defaultCapacity, int maxSize) where T : Component
        {
            if (_pools.ContainsKey(typeof(T))) return;

            var pool = new ObjectPool<T>(
                createFunc: factory.Create,
                actionOnGet: obj => obj.gameObject.SetActive(true),
                actionOnRelease: obj => obj.gameObject.SetActive(false),
                actionOnDestroy: UnityEngine.Object.Destroy,
                collectionCheck: true,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );

            _pools[typeof(T)] = pool;
        }

        public static T Get<T>() where T : Component
        {
            if (_pools.TryGetValue(typeof(T), out var pool))
            {
                return ((ObjectPool<T>)pool).Get();
            }
            
            Debug.LogError($"[PoolManager] No pool initialized for type {typeof(T)}. Call InitializePool first.");
            return null;
        }

        public static void Release<T>(T obj) where T : Component
        {
            if (_pools.TryGetValue(typeof(T), out var pool))
            {
                ((ObjectPool<T>)pool).Release(obj);
            }
            else
            {
                Debug.LogWarning($"[PoolManager] Attempted to release an object of type {typeof(T)}, but no pool exists.");
            }
        }
    }
}