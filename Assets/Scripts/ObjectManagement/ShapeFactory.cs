using System.Collections.Generic;
using Tower_Defense;
using UnityEngine;

namespace ObjectManagement
{
    [CreateAssetMenu]
    public class ShapeFactory : GameObjecFactory
    {
        [SerializeField] private Shape[] _prefabs;

        [SerializeField] private Material[] _materials;

        [SerializeField] private bool _recycle;
    
        private List<Shape>[] _pools;

    
        [System.NonSerialized]
        private int _factoryId = int.MinValue;

        public int FactoryId
        {
            get => _factoryId;
            set
            {
                if (_factoryId == int.MinValue && value != int.MinValue)
                {
                    _factoryId = value;
                }
                else
                {
                    Debug.Log("Not allowed to change factory Id");
                }
            }
        }
    
    
        void CreatePools()
        {
            _pools = new List<Shape>[_prefabs.Length];

            for (int i = 0; i < _pools.Length; i++)
            {
                _pools[i] = new List<Shape>();
            }
        }


        public Shape Get(int shapeId = 0, int materialId = 0)
        {
            Shape instance;

            if (_recycle)
            {
                if (_pools == null)
                {
                    CreatePools();
                }

                List<Shape> pool = _pools[shapeId];

                if (pool.Count > 0)
                {
                    int lastIndex = pool.Count - 1;

                    instance = pool[lastIndex];

                    instance.gameObject.SetActive(true);

                    pool.RemoveAt(lastIndex);
                }
                else
                {
                    instance = CreateGameObjectInstance(_prefabs[shapeId]);

                    instance.OriginFactory = this;
                
                    instance.ShapeId = shapeId;
                
                }
            }
            else
            {
                instance = CreateGameObjectInstance(_prefabs[shapeId]);
                instance.ShapeId = shapeId;
            }

            instance.SetMaterial(_materials[materialId], materialId);
        
            return instance;
        }

        public void Reclaim(Shape shapeToRecycle)
        {
            if (_recycle)
            {
                if (_pools == null)
                {
                    CreatePools();
                }

                _pools[shapeToRecycle.ShapeId].Add(shapeToRecycle);
                shapeToRecycle.gameObject.SetActive(false);
            }
            else
            {
                Destroy(shapeToRecycle.gameObject);
            }
        }


        public Shape GetRandom()
        {
            return Get(Random.Range(0, _prefabs.Length), Random.Range(0, _materials.Length));
        }
    }
}