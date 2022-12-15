using System.Collections.Generic;
using UnityEngine;

namespace Rendering
{
    public class TransformationGrid : MonoBehaviour
    {
        public Transform prefab;

        public int gridResolution;

        private Transform[] _grid;

        private List<Transformation> _transformations = new List<Transformation>();

        private void Awake()
        {
            _grid = new Transform[gridResolution * gridResolution * gridResolution];

            for (int i = 0, z = 0; z < gridResolution; z++)
            {
                for (int y = 0; y < gridResolution; y++)
                {
                    for (int x = 0; x < gridResolution; x++, i++)
                    {
                        _grid[i] = CreateGridPoint(x, y, z);
                    }
                }
            }
        }

        private void Update()
        {
            GetComponents(_transformations);
            int index = 0;
            for (int z = 0; z < gridResolution; z++)
            {
                for (int y = 0; y < gridResolution; y++)
                {
                    for (int x = 0; x < gridResolution; x++)
                    {
                        _grid[index].localPosition = TransformPoint(x, y, z);
                        index++;
                    }
                }
            }
        }

        private Vector3 TransformPoint(int x, int y, int z)
        {
            ///点的原坐标
            Vector3 coordinates = GetCoordinates(x, y, z);

            for (int i = 0; i < _transformations.Count; i++)
            {   
                coordinates = _transformations[i].Apply(coordinates);
            }

            return coordinates;
        }

        private Transform CreateGridPoint(int x, int y, int z)
        {
            Transform point = Instantiate(prefab);
            point.localPosition = GetCoordinates(x, y, z);
            point.GetComponent<MeshRenderer>().material.color = new Color((float)x / gridResolution,
                (float)y / gridResolution, (float)z / gridResolution);

            return point;
        }

        /// <summary>
        /// 获取坐标
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        private Vector3 GetCoordinates(int x, int y, int z)
        {
            return new Vector3(
                x - (gridResolution - 1) * 0.5f,
                y - (gridResolution - 1) * 0.5f,
                z - (gridResolution - 1) * 0.5f);
        }
    }
}