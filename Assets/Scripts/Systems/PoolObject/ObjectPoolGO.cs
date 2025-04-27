using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolGO : MonoBehaviour
{
    private GameObject _prefab;
    private Transform _container;

    private readonly List<GameObject> _enabledPool = new List<GameObject>();
    private readonly List<GameObject> _disabledPool = new List<GameObject>();

    public List<GameObject> EnabledPool => _enabledPool;
    public List<GameObject> DisabledPool => _disabledPool;

    private bool _lifeTimerState;

    public void InitializePool(GameObject prefab, Transform container, int sizeOfPool = 5, bool lifeTimerState = false)
    {
        _lifeTimerState = lifeTimerState;
        _container = container;
        _prefab = prefab;
        for (int i = 0; i < sizeOfPool; i++)
        {
            GameObject obj = Object.Instantiate(_prefab, _container, true);
            obj.SetActive(false);
            _disabledPool.Add(obj);
        }
    }

    public GameObject GetFreeComponent(bool shitchOn = true)
    {
        GameObject obj;
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
            obj.SetActive(true);
        }
        return obj;
    }

    public void DisableComponent(GameObject obj)
    {

        obj.SetActive(false);
        _disabledPool.Add(obj);
        _enabledPool.Remove(obj);
    }
}
