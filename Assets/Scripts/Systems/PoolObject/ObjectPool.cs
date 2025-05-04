using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private T _prefab;
    private Transform _container;

    private readonly List<T> _enabledPool = new List<T>();
    private readonly List<T> _disabledPool = new List<T>();

    private Coroutine _lifeTimer;

    public List<T> EnabledPool => _enabledPool;
    public List<T> DisabledPool => _disabledPool;

    public void InitializePool(T prefab, Transform container, int sizeOfPool = 5)
    {
        _container = container;
        _prefab = prefab;
        for (int i = 0; i < sizeOfPool; i++)
        {
            T obj = Object.Instantiate(_prefab, _container, true);
            obj.gameObject.SetActive(false);
            _disabledPool.Add(obj);          
        }
    }
    public T GetFreeComponent(bool shitchOn = true)
    {
        T obj;
        if (_disabledPool.Count > 0)
        {
            obj = _disabledPool[^1];
            _disabledPool.Remove(obj);
        }
        else
        {
            obj = Object.Instantiate(_prefab, _container, true);
        }
            _enabledPool.Add(obj);
        if (shitchOn)
        { 
            obj.gameObject.SetActive(true);
        }
        return obj;
    }

    public void DisableComponent(T obj)
    {
        
        obj.gameObject.SetActive(false);
        _disabledPool.Add(obj);
        _enabledPool.Remove(obj);
    }
}