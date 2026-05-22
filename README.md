\# Advanced Object Pooling for Unity 



A highly scalable, generic object pooling system built on `UnityEngine.Pool`. This package uses the Factory Pattern and Type Abstraction to manage all your prefabs (Bullets, Enemies, UI, etc.) through a single, centralized manager.



\## Features

\* \*\*Zero Garbage Collection:\*\* Prevents FPS drops and lag spikes during heavy gameplay.

\* \*\*Completely Generic:\*\* Uses `Dictionary<Type, object>` so you don't need to write separate pooling scripts for different objects.

\* \*\*Factory Pattern Driven:\*\* Decouples the instantiation logic from the memory management logic.

\* \*\*Plug \& Play:\*\* Drop it into any project and start pooling instantly.



\##  Installation (Unity Package Manager)



You can install this tool directly into your Unity project using the Package Manager.



1\. Open Unity and go to \*\*Window > Package Manager\*\*.

2\. Click the \*\*+\*\* icon in the top left and select \*\*Add package from git URL...\*\*

3\. Paste the following link and click Add:

&#x20;  `https://github.com/Abdullah165/Advanced-Object-Pooling.git`



\##  Quick Start Guide



\### 1. Initialize the Pool

Set up your pools once (e.g., in your `GameManager`'s `Awake` method) by passing in a PrefabFactory for your specific types.



```csharp

using UnityEngine;



public class GameManager : MonoBehaviour

{

&#x20;   \[SerializeField] private Bullet \_bulletPrefab;



&#x20;   private void Awake()

&#x20;   {

&#x20;       var bulletFactory = new PrefabFactory<Bullet>(\_bulletPrefab);

&#x20;       PoolManager.InitializePool(bulletFactory, defaultCapacity: 100, maxSize: 500);

&#x20;   }

}

