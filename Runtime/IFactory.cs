using UnityEngine;

namespace AdvancedObjectPooling
{
    public interface IFactory<T> where T : Component
    {
        T Create();
    }
}