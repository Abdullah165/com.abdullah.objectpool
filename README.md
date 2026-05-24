# Advanced Object Pooling for Unity

A highly scalable, generic object pooling system built on `UnityEngine.Pool`. This package uses the Factory Pattern and ID-based tracking to efficiently manage all your prefabs and variants (Bullets, Enemies, UI, etc.) through a single, centralized manager.

## Features

* **Eliminates GC Spikes:** Pre-warms object memory to prevent frequent `Instantiate` and `Destroy` overhead during the runtime gameplay loop.
* **Full Variant Support (V1.1):** Uses `Dictionary<ulong, object>` tied to `GetInstanceID()`. This allows you to pool multiple different prefabs that share the exact same script (e.g., a "FireBullet" prefab and an "IceBullet" prefab, both using `Bullet.cs`).
* **Factory Pattern Driven:** Decouples the instantiation logic from the memory management logic.
* **Auto-Tracking Architecture:** Pooled objects automatically remember their origin pool, allowing for a completely clean `Release()` API without needing to pass IDs back manually.

## Installation (Unity Package Manager)

You can install this tool directly into your Unity project using the Package Manager.

1. Open Unity and go to **Window > Package Manager**.
2. Click the **+** icon in the top left and select **Add package from git URL...**
3. Paste the following link and click Add:

`https://github.com/Abdullah165/com.abdullah.objectpool.git`

## Quick Start Guide

Here is a complete example showing how to initialize multiple variants of a pool, ask for a specific variant when shooting, and return the object seamlessly.

### 1. Initialization (Game Manager)
Pass the prefab's instance ID to create unique pools for different prefabs sharing the same script.

```csharp
using UnityEngine;
using AdvancedObjectPooling;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Bullet _fireBulletPrefab;
    [SerializeField] private Bullet _iceBulletPrefab;

    private void Awake()
    {
        // Initialize Fire Bullets
        var fireFactory = new PrefabFactory<Bullet>(_fireBulletPrefab);
        ulong fireId = (ulong)_fireBulletPrefab.GetInstanceID();
        PoolManager.InitializePool(fireId, fireFactory, defaultCapacity: 100, maxSize: 500);

        // Initialize Ice Bullets (Same script, completely separate pool!)
        var iceFactory = new PrefabFactory<Bullet>(_iceBulletPrefab);
        ulong iceId = (ulong)_iceBulletPrefab.GetInstanceID();
        PoolManager.InitializePool(iceId, iceFactory, defaultCapacity: 100, maxSize: 500);
    }
}
```

### 2. Spawning (Weapon)
The spawner keeps a reference to the prefab to know exactly which ID to request from the manager.

```csharp
using UnityEngine;
using AdvancedObjectPooling;

public class Weapon : MonoBehaviour
{
    [SerializeField] private Bullet _equippedBulletPrefab;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        ulong bulletId = (ulong)_equippedBulletPrefab.GetInstanceID();
        Bullet newBullet = PoolManager.Get<Bullet>(bulletId);
        
        newBullet.transform.position = transform.position + transform.forward;
        newBullet.transform.rotation = transform.rotation;

        Rigidbody rb = newBullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(transform.forward * 20, ForceMode.Impulse);
        }
    }
}
```

### 3. Releasing (Bullet)
Because the `PoolManager` automatically handles ID tracking under the hood, your gameplay scripts remain completely agnostic to the dictionary mechanics.

```csharp
using UnityEngine;
using AdvancedObjectPooling;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Simply return to the pool instead of calling Destroy(). 
        // The manager handles the routing!
        PoolManager.Release(this);
    }
}
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
