using System.Collections.Generic;
using UnityEngine;

public class ObjectGarbageCollector : MonoBehaviour
{
    [SerializeField] private List<GameObject> _gameEnteties; // Объекты, которые нужно проверять
    [SerializeField] private Transform _player;              // Игрок (задать в инспекторе)
    [SerializeField] private float _distanceBelowPlayer = 10f; // Расстояние, на котором объекты удаляются

    void Update()
    {
        // Кэшируем позицию игрока по Y
        float thresholdY = _player.position.y - _distanceBelowPlayer;

        // Используем временный список, чтобы избежать модификации во время итерации
        List<GameObject> toRemove = new List<GameObject>();

        foreach (GameObject obj in _gameEnteties)
        {
            if (obj != null && obj.transform.position.y < thresholdY)
            {
                Destroy(obj);
                toRemove.Add(obj);
            }
        }

        // Удаляем уничтоженные объекты из списка
        foreach (GameObject obj in toRemove)
        {
            _gameEnteties.Remove(obj);
        }
    }

    /// <summary>
    /// Добавление объекта в список отслеживаемых
    /// </summary>
    public void RegisterObject(GameObject obj)
    {
        if (!_gameEnteties.Contains(obj))
        {
            _gameEnteties.Add(obj);
        }
    }
}
