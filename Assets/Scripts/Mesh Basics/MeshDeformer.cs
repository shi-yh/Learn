using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
   public float springForce = 5;

   public float damping = 3f;
   
   private Mesh _deformingMesh;

   private Vector3[] _originVertices, _displacedVertices;

   /// <summary>
   /// 顶点速度
   /// </summary>
   private Vector3[] _vertexVelocities;

   private float _uniformScale = 1f;
   
   
   private void Start()
   {
      _deformingMesh = GetComponent<MeshFilter>().mesh;

      _originVertices = _deformingMesh.vertices;
      _displacedVertices = new Vector3[_originVertices.Length];
      _vertexVelocities = new Vector3[_originVertices.Length];

      for (int i = 0; i < _originVertices.Length; i++)
      {
         _displacedVertices[i] = _originVertices[i];
      }
      
   }

   public void AddDeformingForce(Vector3 point, float force)
   {
      point = transform.InverseTransformPoint(point);
      for (int i = 0; i < _displacedVertices.Length; i++)
      {
         AddForceToVertex(i, point, force);
      }
   }

   private void AddForceToVertex(int i, Vector3 point, float force)
   {
      ///得到每个顶点形变力的方向和距离
      Vector3 pointToVertex = _displacedVertices[i] - point;

      pointToVertex *= _uniformScale;
      
      ///可以用逆平方定律找到衰减力Fv = F/d²
      /// 实际上除以1+距离平方，确保力在距离0的地方为本身强度，而不是无穷大
      float attenuateForce = force / (1 + pointToVertex.sqrMagnitude);

      ///a = F/m,δv = aδt,此处忽略质量，即δv= f*δt
      float velocity = attenuateForce * Time.deltaTime;

      _vertexVelocities[i] += pointToVertex.normalized * velocity;
   }

   private void Update()
   {
      _uniformScale = transform.localScale.x;
      for (int i = 0; i < _displacedVertices.Length; i++)
      {
         UpdateVertex(i);
      }

      _deformingMesh.vertices = _displacedVertices;
      _deformingMesh.RecalculateNormals();

   }

   private void UpdateVertex(int i)
   {
      Vector3 velocity = _vertexVelocities[i];
     
      Vector3 displacement = _displacedVertices[i] - _originVertices[i];
      displacement *= _uniformScale;
      
      velocity -= displacement * springForce * Time.deltaTime;
      
      velocity *= 1f - damping * Time.deltaTime;
      
      _vertexVelocities[i] = velocity;

      _displacedVertices[i] += velocity * Time.deltaTime;

   }
}
