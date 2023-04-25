using UnityEngine;
using Zenject;

public abstract class ZenjectDynamicObject<T> : MonoBehaviour
{
    public class Factory : PlaceholderFactory<T>
    {
        
    }
}