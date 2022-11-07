using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class RoundCube : MonoBehaviour
{
    public int xSize, ySize, zSize;

    /// <summary>
    /// 不要大于xyz的一半！
    /// </summary>
    public int roundness;

    private Mesh _mesh;

    private Vector3[] _vertices;

    private Vector3[] _normals;

    private Color32[] _cubeUV;

    private WaitForSeconds _wait = null;

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

        int faceVertices = ((xSize - 1) * (ySize - 1) + (xSize - 1) * (zSize - 1) + (ySize - 1) * (zSize - 1)) * 2;

        _vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];
        _normals = new Vector3[_vertices.Length];
        _cubeUV = new Color32[_vertices.Length];
        
        
        int index = 0;


        #region 生成整体

        for (int y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                SetVertex(index++,x, y,0);
                yield return _wait;
            }

            for (int z = 1; z <= zSize; z++)
            {
                SetVertex(index++,xSize, y,z);
                yield return _wait;
            }

            for (int x = xSize - 1; x >= 0; x--)
            {
                SetVertex(index++,x, y,zSize);
                yield return _wait;
            }


            for (int z = zSize - 1; z > 0; z--)
            {
                SetVertex(index++,0, y,z);
                yield return _wait;
            }
        }

        #endregion

        #region 顶部和底部加盖

        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                SetVertex(index++,x, ySize,z);
                yield return _wait;
            }
        }

        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                SetVertex(index++,x, 0,z);
                yield return _wait;
            }
        }
        #endregion
        
        _mesh.vertices = _vertices;
        _mesh.normals = _normals;
        _mesh.colors32 = _cubeUV;

    }

    private void SetVertex(int index, int x, int y, int z)
    {
        Vector3 inner = _vertices[index] = new Vector3(x, y, z);

        ///判断朝哪个方向圆
        /// 当点在立方体的左后下时，inner的值大于实际点的值，朝左后下圆
        /// 当点在立方体的右前上时，inner的值小于实际点的值，朝右前上圆
        if (x<roundness)
        {
            inner.x = roundness;
        }
        else if (x>xSize-roundness)
        {
            inner.x = xSize - roundness;
        }

        if (y<roundness)
        {
            inner.y = roundness;
        }
        else if (y>ySize-roundness)
        {
            inner.y = ySize - roundness;
        }
        
        if (z<roundness)
        {
            inner.z = roundness;
        }
        else if (z>zSize-roundness)
        {
            inner.z = zSize - roundness;
        }
        
        
        _normals[index] = (_vertices[index] - inner).normalized;

        _vertices[index] = inner + _normals[index] * roundness;
        ///顶点颜色组件存储为一个字节，整个颜色的大小为四个字节，与单个浮点数相同
        _cubeUV[index] = new Color32((byte) x, (byte) y, (byte) z, 0);
    }


    private void GenerateColliders()
    {
        AddBoxCollider(xSize,ySize-roundness*2,zSize-roundness*2);
        AddBoxCollider(xSize-roundness*2,ySize,zSize-roundness*2);
        AddBoxCollider(xSize-roundness*2,ySize-roundness*2,zSize);
        
        
        Vector3 min = Vector3.one * roundness;
        Vector3 half = new Vector3(xSize, ySize, zSize) * 0.5f; 
        Vector3 max = new Vector3(xSize, ySize, zSize) - min;

        AddCapsuleCollider(0, half.x, min.y, min.z);
        AddCapsuleCollider(0, half.x, min.y, max.z);
        AddCapsuleCollider(0, half.x, max.y, min.z);
        AddCapsuleCollider(0, half.x, max.y, max.z);
		
        AddCapsuleCollider(1, min.x, half.y, min.z);
        AddCapsuleCollider(1, min.x, half.y, max.z);
        AddCapsuleCollider(1, max.x, half.y, min.z);
        AddCapsuleCollider(1, max.x, half.y, max.z);
		      
        AddCapsuleCollider(2, min.x, min.y, half.z);
        AddCapsuleCollider(2, min.x, max.y, half.z);
        AddCapsuleCollider(2, max.x, min.y, half.z);
        AddCapsuleCollider(2, max.x, max.y, half.z);



    }


    private void AddBoxCollider(float x,float y,float z)
    {
        BoxCollider c = gameObject.AddComponent<BoxCollider>();
        c.size = new Vector3(x, y, z);
    }

    private void AddCapsuleCollider(int direction, float x, float y, float z)
    {
        CapsuleCollider c = gameObject.AddComponent<CapsuleCollider>();

        c.center = new Vector3(x, y, z);
        c.direction = direction;
        c.radius = roundness;
        c.height = c.center[direction] * 2f;


    }
    

    private IEnumerator GenerateTriangles()
    {
        int[] trianglesZ = new int[xSize * ySize * 12];
        int[] trianglesX = new int[ySize * zSize * 12];
        int[] trianglesY = new int[xSize * zSize * 12];

        int ring = (xSize + zSize) * 2;

        int v = 0,tZ=0,tX=0,tY=0;


        #region 四面包围
        
        for (int y = 0; y < ySize; y++, v++) {
            for (int q = 0; q < xSize; q++, v++) {
                tZ = SetQuad(trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
            }
            for (int q = 0; q < zSize; q++, v++) {
                tX = SetQuad(trianglesX, tX, v, v + 1, v + ring, v + ring + 1);
            }
            for (int q = 0; q < xSize; q++, v++) {
                tZ = SetQuad(trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
            }
            for (int q = 0; q < zSize - 1; q++, v++) {
                tX = SetQuad(trianglesX, tX, v, v + 1, v + ring, v + ring + 1);
            }
            tX = SetQuad(trianglesX, tX, v, v - ring + 1, v + ring, v + 1);
        }
        #endregion

        #region 顶部和底部

         tY = CreateTopFace(trianglesY, tY, ring);

         tY = CreateBottomFace(trianglesY, tY, ring);
         
         
        #endregion


        yield return _wait;

        _mesh.subMeshCount=3;
        _mesh.SetTriangles(trianglesZ,0);
        _mesh.SetTriangles(trianglesX,1);
        _mesh.SetTriangles(trianglesY,2);
        
    }

    /// <summary>
    /// 底面和顶面有一些细微的不同之处，即顶点索引不同
    /// 我们还必须改变顶点的时钟方向，使三角形面朝下而不是向上
    /// </summary>
    /// <param name="triangles"></param>
    /// <param name="index"></param>
    /// <param name="ring"></param>
    /// <returns></returns>
    private int CreateBottomFace(int[] triangles, int t, int ring)
    {
        int v = 1;
        int vMid = _vertices.Length - (xSize - 1) * (zSize - 1);
        t = SetQuad(triangles, t, ring - 1, vMid, 0, 1);
        for (int x = 1; x < xSize - 1; x++, v++, vMid++) {
            t = SetQuad(triangles, t, vMid, vMid + 1, v, v + 1);
        }
        t = SetQuad(triangles, t, vMid, v + 2, v, v + 1);
        
        int vMin = ring - 2;
        vMid -= xSize - 2;
        int vMax = v + 2;
        
        for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
            t = SetQuad(triangles, t, vMin, vMid + xSize - 1, vMin + 1, vMid);
            for (int x = 1; x < xSize - 1; x++, vMid++) {
                t = SetQuad(
                    triangles, t,
                    vMid + xSize - 1, vMid + xSize, vMid, vMid + 1);
            }
            t = SetQuad(triangles, t, vMid + xSize - 1, vMax + 1, vMid, vMax);
        }
        
        int vTop = vMin - 1;
        t = SetQuad(triangles, t, vTop + 1, vTop, vTop + 2, vMid);
        for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
            t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vMid + 1);
        }
        t = SetQuad(triangles, t, vTop, vTop - 1, vMid, vTop - 2);
		
        return t;
    }


    private int CreateTopFace(int[] triangles, int index, int ring)
    {
        int start = ring * ySize;

        for (int x = 0; x < xSize - 1; x++)
        {
            index = SetQuad(triangles, index, start, start + 1, start + ring - 1, start + ring);
            start++;
        }

        index = SetQuad(triangles, index, start, start + 1, start + ring - 1, start + 2);

        #region 隔得太久这段已经看不明白了......,反正就是找顶点

        int vMin = ring * (ySize + 1) - 1;
        int vMid = vMin + 1;
        int vMax = start + 2;
        
        for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++) {
            index = SetQuad(triangles, index, vMin, vMid, vMin - 1, vMid + xSize - 1);
            for (int x = 1; x < xSize - 1; x++, vMid++) {
                index = SetQuad(
                    triangles, index,
                    vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
            }
            index = SetQuad(triangles, index, vMid, vMax, vMid + xSize - 1, vMax + 1);
        }
        
        int vTop = vMin - 2;
        index = SetQuad(triangles, index, vMin, vMid, vTop + 1, vTop);
        for (int x = 1; x < xSize - 1; x++, vTop--, vMid++) {
            index = SetQuad(triangles, index, vMid, vMid + 1, vTop, vTop - 1);
        }
        index = SetQuad(triangles, index, vMid, vTop - 2, vTop, vTop - 1);

        #endregion


        return index;
    }

    private IEnumerator Generate()
    {
        GetComponent<MeshFilter>().mesh = _mesh = new Mesh();

        _mesh.name = "Procedural Cube";

        yield return GenerateVertices();

        yield return GenerateTriangles();
        
        GenerateColliders();
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
        // if (_vertices == null)
        // {
        //     return;
        // }
        //
        // Gizmos.color = Color.black;
        //
        // for (int i = 0; i < _vertices.Length; i++)
        // {
        //     Gizmos.color = Color.black;;
        //     Gizmos.DrawSphere(_vertices[i], 0.1f);
        //     
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawRay(_vertices[i],_normals[i]);
        // }
    }
}