# Advanced Object Pooling for Unity

A highly scalable, generic object pooling system built on `UnityEngine.Pool`. This package uses the Factory Pattern and Type Abstraction to manage all your prefabs (Bullets, Enemies, UI, etc.) through a single, centralized manager.

## Features

* **Eliminates GC Spikes:** Pre-warms object memory to prevent frequent `Instantiate` and `Destroy` overhead during intensive gameplay loops.
* **Completely Generic:** Uses `Dictionary<Type, object>` so you don't need to write separate pooling scripts for different objects.
* **Factory Pattern Driven:** Decouples the instantiation logic from the memory management logic.
* **Plug & Play:** Drop it into any project and start pooling instantly.

## Installation (Unity Package Manager)

You can install this tool directly into your Unity project using the Package Manager.

1. Open Unity and go to **Window > Package Manager**.
2. Click the **+** icon in the top left and select **Add package from git URL...**
3. Paste the following link and click Add:

`https://github.com/Abdullah165/com.abdullah.objectpool.git`

## Quick Start Guide

Here is a complete example showing how to initialize the pool in your manager, ask for an object when shooting, and return the object upon collision.

```csharp

using UnityEngine;
using AdvancedObjectPooling;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Bullet _bulletPrefab;

    private void Awake()
    {
        var bulletFactory = new PrefabFactory<Bullet>(_bulletPrefab);
        PoolManager.InitializePool(bulletFactory, defaultCapacity: 100, maxSize: 500);
    }
}


using UnityEngine;
using AdvancedObjectPooling;

public class Weapon : MonoBehaviour
{
    public void Shoot()
    {
        // Ask the pool for a bullet
        Bullet newBullet = PoolManager.Get<Bullet>();
        newBullet.transform.position = transform.position;
    }
}


using UnityEngine;
using AdvancedObjectPooling;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Return to the pool instead of calling Destroy()
        PoolManager.Release(this);
    }
}
```

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
