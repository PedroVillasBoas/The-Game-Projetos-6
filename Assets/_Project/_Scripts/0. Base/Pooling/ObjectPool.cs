using UnityEngine;
using System.Collections.Generic;
using GoodVillageGames.Game.Enums.Pooling;

namespace GoodVillageGames.Game.Core.Pooling
{
    public class ObjectPool
    {
        private Queue<GameObject> _pool = new();
        private GameObject _prefab;
        private Transform _parent;
        private bool _autoExpand;
        private int _expandAmount;
        private PoolID _poolID;

        public ObjectPool(PoolID poolID, GameObject prefab, int initialSize, Transform parent = null, bool autoExpand = true, int expandAmount = 1)
        {
            _poolID = poolID;
            _prefab = prefab;
            _parent = parent;
            _autoExpand = autoExpand;
            _expandAmount = expandAmount;
            Preload(initialSize);
        }

        private void Preload(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject obj = CreateNewObject();
                obj.SetActive(false);
                _pool.Enqueue(obj);
            }
        }

        private GameObject CreateNewObject()
        {
            GameObject obj = Object.Instantiate(_prefab, _parent);
            if (!obj.TryGetComponent<PooledObject>(out var pooledComponent))
            {
                pooledComponent = obj.AddComponent<PooledObject>();
            }
            pooledComponent.SetPoolId(_poolID);
            return obj;
        }

        public GameObject GetGameObject()
        {
            if (_pool.Count == 0)
            {
                if (_autoExpand)
                {
                    ExpandPool(_expandAmount);
                }
                else
                {
                    Debug.LogWarning("Pool is empty and auto expansion is disabled.");
                    return null;
                }
            }

            GameObject obj = _pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }

        public void ReturnGameObject(GameObject obj)
        {
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }

        public void ExpandPool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject obj = CreateNewObject();
                obj.SetActive(false);
                _pool.Enqueue(obj);
            }
        }
    }
}
