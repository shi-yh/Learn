using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Cube : MonoBehaviour
{
    public int xSize, ySize, zSize;

    private Mesh _mesh;

    private Vector3[] _vertices;

    private WaitForSeconds _wait = new WaitForSeconds(0.1f);

    private void Awake()
    {
        StartCoroutine(Generate());
    }

    private IEnumerator GenerateVertices()
    {
        ///首先有八个角顶点
        int cornerVertices = 8;

        ///十二条边，每条边的顶点数为size，对顶点去重，每条边去两个点
        /// (x+y+z-6)*4
        /// 但是每个顶点都被减了两次
        /// （x+u+z-6）*4+12
        int edgeVertices = (xSize + ySize + zSize - 3) * 4;

        int faceVertices = ((xSize - 1) * (ySize - 1) + (xSize - 1) * (zSize - 1) + (ySize - 1 + zSize - 1)) * 2;

        _vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];

        int index = 0;


        #region 生成整体

        for (int y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                _vertices[index++] = new Vector3(x, y, 0);
                yield return _wait;
            }

            for (int z = 1; z <= zSize; z++)
            {
                _vertices[index++] = new Vector3(xSize, y, z);
                yield return _wait;
            }

            for (int x = xSize - 1; x >= 0; x--)
            {
                _vertices[index++] = new Vector3(x, y, zSize);
                yield return _wait;
            }


            for (int z = zSize - 1; z > 0; z--)
            {
                _vertices[index++] = new Vector3(0, y, z);
                yield return _wait;
            }
        }

        #endregion

        #region 顶部和底部加盖

        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                _vertices[index++] = new Vector3(x, ySize, z);
                yield return _wait;
            }
        }

        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                _vertices[index++] = new Vector3(x, 0, z);
                yield return _wait;
            }
        }

        _mesh.vertices = _vertices;

        #endregion
    }

    private IEnumerator GenerateTriangles()
    {
        int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
        int[] triangles = new int[quads * 6];
        _mesh.triangles = triangles;

        int ring = (xSize + zSize) * 2;

        int index = 0;


        #region 四面包围

        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < ring - 1; x++)
            {
                index = SetQuad(triangles, index, y * ring + x, y * ring + x + 1, (y + 1) * ring + x, x + (y + 1) * ring + 1);
            }

            ///最后一个三角形做特殊处理
            index = SetQuad(triangles, index, y * ring + ring - 1, y * ring, (y + 1) * ring + ring - 1, (y + 1) * ring);
        }

        #endregion

        #region 顶部和底部

        index = CreateTopFace(triangles, index, ring);

        #endregion


        yield return _wait;

        _mesh.triangles = triangles;
    }


    private int CreateTopFace(int[] triangles, int index, int ring)
    {
        int start = ring * ySize;

        for (int x = 0; x < xSize-1; x++)
        {
            index = SetQuad(triangles, index, start, start + 1, start + ring - 1, start + ring);
            start++;
        }

        index = SetQuad(triangles, index, start, start + 1, start + ring - 1, start + 2);
    


        return index;
    }

    private IEnumerator Generate()
    {
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();

        _mesh.name = "Procedural Cube";

        yield return GenerateVertices();

        yield return GenerateTriangles();
    }

    private int SetQuad(int[] triangles, int index, int v00, int v10, int v01, int v11)
    {
        triangles[index] = v00;
        triangles[index + 1] = triangles[index + 4] = v01;
        triangles[index + 2] = triangles[index + 3] = v10;
        triangles[index + 5] = v11;
        return index + 6;
    }


    private void OnDrawGizmos()
    {
        if (_vertices == null)
        {
            return;
        }

        Gizmos.color = Color.black;

        for (int i = 0; i < _vertices.Length; i++)
        {
            Gizmos.DrawSphere(_vertices[i], 0.1f);
        }
    }
}