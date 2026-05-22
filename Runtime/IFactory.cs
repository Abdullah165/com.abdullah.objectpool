using UnityEngine;

public interface IFactory<T> where T : Component
{
    T Create();
}
