using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Grid : MonoBehaviour
{
    public int xSize, ySize;

    private Vector3[] _vertices;

    private Mesh _mesh;

    private void Awake()
    {
        StartCoroutine(Generate());
    }

    private IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
        _mesh.name = "Procedural Grid";

        ///共用顶点，x*y的矩形，实际的顶点是（x+1）*(y+1)
        _vertices = new Vector3[(xSize + 1) * (ySize + 1)];

        int index = 0;
        
        for (int y = 0; y < ySize; y++)
        {
            for (int x = 0; x < xSize; x++)
            {
                _vertices[index] = new Vector3(x, y);
                index++;
                yield return null;
            }
        }

        _mesh.vertices = _vertices;

        int[] triangles = new int[xSize * 6 * ySize];


        index = 0;

        for (int j = 0; j < ySize-1; j++)
        {
            for (int i = 0; i < xSize-1; i++)
            {
                int temp = i + j * xSize;
                triangles[index] = temp;
                triangles[index + 1] = triangles[index + 3] = temp + xSize;
                triangles[index + 2] = triangles[index + 5] = temp + 1;
                triangles[index + 4] = temp + xSize + 1;

                index += 6;
            }
        }


        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();

        Vector2[] uv = new Vector2[_vertices.Length];

        index = 0;
        ///要使纹理适合整个网格，只需将顶点的位置除以网格尺寸即可
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                uv[index] = new Vector2((float)j / xSize, (float)i / ySize);
                index++;
            }
        }

        _mesh.uv = uv;

        Vector4[] tangents = new Vector4[_vertices.Length];

        Vector4 tangent = new Vector4(1, 0, 0, -1);
        
        index = 0;
        ///要使纹理适合整个网格，只需将顶点的位置除以网格尺寸即可
        for (int i = 0; i < ySize; i++)
        {
            for (int j = 0; j < xSize; j++)
            {
                tangents[index] = tangent;
                index++;
            }
        }

        _mesh.tangents = tangents;

    }

    private void OnDrawGizmos()
    {
        if (_vertices == null)
        {
            return;
        }

        Gizmos.color = Color.black;

        Gizmos.matrix = transform.localToWorldMatrix;

        for (int i = 0; i < _vertices.Length; i++)
        {
            Gizmos.DrawSphere(_vertices[i], 0.1f);
        }
    }
}