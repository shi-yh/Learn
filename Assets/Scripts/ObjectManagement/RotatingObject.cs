using UnityEngine;

namespace ObjectManagement
{
    public class RotatingObject : PersistableObject
    {
        [SerializeField] private Vector3 _angularVelocity;

        private void Update()
        {
            transform.Rotate(_angularVelocity * Time.deltaTime);
        }
    }
}